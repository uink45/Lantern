using IronSnappy;
using Lantern.Config;
using Lantern.Core;
using Lantern.SSZ;
using Lantern.SSZ.Types;
using Lantern.SSZ.Types.Altair;
using Lantern.SSZ.Types.Bellatrix;
using Lantern.SSZ.Types.Capella;
using Lantern.SSZ.Types.Deneb;
using Lantern.Tests.Models;
using Lantern.Types.Basic;
using Lantern.Types.Containers;
using Lantern.Types.Crypto;
using Lantern.Types.Crypto.BLS;
using NUnit.Framework;
using YamlDotNet.Serialization;

namespace Lantern.Tests;

[TestFixture]
public class CoreTests
{
    [SetUp]
    public void Setup()
    {
        Preset.SetPreset<Minimal>();
    }
    private readonly string _testsLocation = Path.Combine(AppContext.BaseDirectory, "MockupData", "minimal");

    [Test]
    public void ExecuteProofTests()
    {
        var sszFiles = Directory.GetFiles(_testsLocation, "object.ssz_snappy", SearchOption.AllDirectories);
        var proofFiles = Directory.GetFiles(_testsLocation, "proof.yaml", SearchOption.AllDirectories);
        var fileGroups = sszFiles.Zip(proofFiles, (sszFile, proofFile) => (sszFile, proofFile));

        foreach (var (sszFile, proofFile) in fileGroups)
        {
            using var reader = new StreamReader(proofFile);
            var deserializer = new DeserializerBuilder().Build();
            var proofData = deserializer.Deserialize<ProofData>(reader);
            var leaf = new Root(Bytes.FromHexString(proofData.leaf));
            var depth = Helpers.FloorLog2(proofData.leaf_index);
            var index = Helpers.GetSubtreeIndex(proofData.leaf_index);
            var branch = proofData.branch.Select(b => new Bytes32(Bytes.FromHexString(b))).ToArray();
            var root = GetRootFromSszObject(sszFile);
            var result = Helpers.IsValidMerkleBranch(leaf, branch, depth, index, root ?? throw new Exception());
            Assert.That(result, Is.EqualTo(true), "IsValidMerkleBranch should return true for the test case.");
        }
    }

    [Test]
    public void ExecuteSyncTests()
    {
        var bootstrapFiles = Directory.GetFiles(_testsLocation, "bootstrap.ssz_snappy", SearchOption.AllDirectories);
        var configFiles = Directory.GetFiles(_testsLocation, "config.yaml", SearchOption.AllDirectories);
        var metaFiles = Directory.GetFiles(_testsLocation, "meta.yaml", SearchOption.AllDirectories)
            .Where(file => !file.Contains("update_ranking")).ToArray();
        var stepsFiles = Directory.GetFiles(_testsLocation, "steps.yaml", SearchOption.AllDirectories);
        var updateFiles = Directory.GetFiles(_testsLocation, "update*.ssz_snappy", SearchOption.AllDirectories).Where(file => !file.Contains("update_ranking")).ToArray();
        var fileGroups = bootstrapFiles.Select((bs, i) => (bs, configFiles[i], metaFiles[i], stepsFiles[i]));
        var blsUtility = new BLSUtility();

        foreach (var (bootstrapFile, configFile, metaFile, stepsFile) in fileGroups)
        {
            var deserializer = new DeserializerBuilder().Build();
            SetPreset(configFile, deserializer);
            var bootstrapData = DeserializeYaml<BootstrapData>(metaFile, deserializer);
            var bootstrap = GetBootstrap(bootstrapFile, bootstrapData.genesis_validators_root, bootstrapData.bootstrap_fork_digest);
            var trustedRoot = new Root(Bytes.FromHexString(bootstrapData.trusted_block_root));
            var genesisValidatorsRoot = new Root(Bytes.FromHexString(bootstrapData.genesis_validators_root));
            var store = LightClientStore.InitializeLightClientStore(trustedRoot, bootstrap ?? throw new Exception(),
                SizePreset.MinimalPreset);
            using var reader = new StreamReader(stepsFile);
            var steps = deserializer.Deserialize<List<Step>>(reader);

            foreach (var step in steps.Where(step => !step.Equals(null)))
                if (step.process_update != null)
                {
                    var updateFile = updateFiles.First(file => file.Contains(step.process_update.update) && file.Contains(DetermineTestType(bootstrapFile)));
                    var update = GetUpdate(updateFile, bootstrapData.genesis_validators_root, step.process_update.update_fork_digest);

                    Processors.ProcessLightClientUpdate(store, update ?? throw new Exception(), (Slot)step.process_update.current_slot,
                        genesisValidatorsRoot, blsUtility, SizePreset.MinimalPreset);
                    var result = LightClientStoreChecks(store, step.process_update.checks);

                    Assert.That(result, Is.EqualTo(true));
                }
                else if (step.force_update != null)
                {
                    Processors.ProcessLightClientStoreForceUpdate(store, (Slot)step.force_update.current_slot);
                    var result = LightClientStoreChecks(store, step.force_update.checks);
                    Assert.That(result, Is.EqualTo(true));
                }
        }
    }

    [Test]
    public void ExecuteRankingTests()
    {
        var hardForks = new string[4]{ "altair", "bellatrix", "capella", "eip4844" };
        
        foreach (var hardForkName in hardForks)
        {
            var updateFiles = Directory.GetFiles(_testsLocation, "update*.ssz_snappy", SearchOption.AllDirectories)
                .Where(file => file.Contains("update_ranking") && file.Contains(hardForkName))
                .OrderBy(ExtractUpdateNumber)
                .ToArray();
            var updates = new List<LightClientUpdate>();

            foreach (var updateFile in updateFiles)
            {
                if (hardForkName == "altair")
                {
                    updates.Add(AltairUpdate.Deserialize(ConstructSszObject<AltairUpdate>(updateFile)));
                }
                
                if (hardForkName == "bellatrix")
                {
                    updates.Add(BellatrixUpdate.Deserialize(ConstructSszObject<BellatrixUpdate>(updateFile)));
                }
                
                if (hardForkName == "capella")
                {
                    updates.Add(CapellaUpdate.Deserialize(ConstructSszObject<CapellaUpdate>(updateFile)));
                }
                
                if (hardForkName == "eip4844")
                {
                    updates.Add(DenebUpdate.Deserialize(ConstructSszObject<DenebUpdate>(updateFile)));
                }
            }

            for (int i = 0; i < updates.Count - 1; i++)
            {
                if (!Helpers.IsBetterUpdate(updates[i], updates[i + 1]))
                {
                    throw new Exception($"Updates are not ranked in descending order for hard fork: {hardForkName}");
                }
            }

        }
        
    }

    private static void SetPreset(string configFile, IDeserializer deserializer)
    {
        var data = DeserializeYaml<ConfigData>(configFile, deserializer);
        Preset.ALTAIR_FORK_EPOCH = new Epoch(ulong.Parse(data.ALTAIR_FORK_EPOCH ?? "0"));
        Preset.ALTAIR_FORK_VERSION = new ForkVersion(Bytes.FromHexString(data.ALTAIR_FORK_VERSION ?? "0x01000001"));
        Preset.BELLATRIX_FORK_EPOCH = new Epoch(ulong.Parse(data.BELLATRIX_FORK_EPOCH ?? "18446744073709551615"));
        Preset.BELLATRIX_FORK_VERSION = new ForkVersion(Bytes.FromHexString(data.BELLATRIX_FORK_VERSION ?? "0x02000001"));
        Preset.CAPELLA_FORK_EPOCH = new Epoch(ulong.Parse(data.CAPELLA_FORK_EPOCH  ?? "18446744073709551615"));
        Preset.CAPELLA_FORK_VERSION = new ForkVersion(Bytes.FromHexString(data.CAPELLA_FORK_VERSION ?? "0x03000001"));
        Preset.GENESIS_FORK_VERSION = new ForkVersion(Bytes.FromHexString(data.GENESIS_FORK_VERSION));
        Preset.EIP4844_FORK_EPOCH = new Epoch(ulong.Parse(data.EIP4844_FORK_EPOCH ?? "18446744073709551615"));
        Preset.EIP4844_FORK_VERSION = new ForkVersion(Bytes.FromHexString(data.EIP4844_FORK_VERSION ?? "0x04000001"));
    }

    private static T DeserializeYaml<T>(string filePath, IDeserializer deserializer)
    {
        using var reader = new StreamReader(filePath);
        return deserializer.Deserialize<T>(reader);
    }
    
    private static T ConstructSszObject<T>(string filePath)
    {
        return Deserialize.DeserialiseSSZObject<T>(Snappy.Decode(File.ReadAllBytes(filePath)),
            SizePreset.MinimalPreset);
    }

    private static Root? GetRootFromSszObject(string filePath)
    {
        if (filePath.Contains("BeaconState"))
        {
            if (filePath.Contains("altair"))
                return new Root(Merkleizer.HashTreeRoot(
                    SszContainer.GetContainer<AltairState>(SizePreset.MinimalPreset),
                    ConstructSszObject<AltairState>(filePath)));

            if (filePath.Contains("bellatrix"))
                return new Root(Merkleizer.HashTreeRoot(
                    SszContainer.GetContainer<BellatrixState>(SizePreset.MinimalPreset),
                    ConstructSszObject<BellatrixState>(filePath)));

            if (filePath.Contains("capella"))
                return new Root(Merkleizer.HashTreeRoot(
                    SszContainer.GetContainer<CapellaState>(SizePreset.MinimalPreset),
                    ConstructSszObject<CapellaState>(filePath)));

            if (filePath.Contains("eip4844"))
                return new Root(Merkleizer.HashTreeRoot(
                    SszContainer.GetContainer<DenebState>(SizePreset.MinimalPreset),
                    ConstructSszObject<DenebState>(filePath)));
        }
        else if (filePath.Contains("BeaconBlockBody"))
        {
            if (filePath.Contains("capella"))
                return new Root(Merkleizer.HashTreeRoot(
                    SszContainer.GetContainer<CapellaBlockBody>(SizePreset.MinimalPreset),
                    ConstructSszObject<CapellaBlockBody>(filePath)));

            if (filePath.Contains("eip4844"))
                return new Root(Merkleizer.HashTreeRoot(
                    SszContainer.GetContainer<DenebBlockBody>(SizePreset.MinimalPreset),
                    ConstructSszObject<DenebBlockBody>(filePath)));
        }

        return Root.Zero;
    }

    private static LightClientBootstrap? GetBootstrap(string filePath, string genesisValidatorsRoot, string forkDigest) =>
        GetResult<LightClientBootstrap>(filePath, genesisValidatorsRoot, forkDigest, (path, version) =>
        {
            return version switch
            {
                "01000001" => AltairBootstrap.Deserialize(ConstructSszObject<AltairBootstrap>(path)),
                "02000001" => BellatrixBootstrap.Deserialize(ConstructSszObject<BellatrixBootstrap>(path)),
                "03000001" => CapellaBootstrap.Deserialize(ConstructSszObject<CapellaBootstrap>(path)),
                "04000001" => DenebBootstrap.Deserialize(ConstructSszObject<DenebBootstrap>(path)),
                _ => default
            } ?? throw new InvalidOperationException();
        });

    private static LightClientUpdate? GetUpdate(string filePath, string genesisValidatorsRoot, string forkDigest) =>
        GetResult<LightClientUpdate>(filePath, genesisValidatorsRoot, forkDigest, (path, version) =>
        {
            return version switch
            {
                "01000001" => AltairUpdate.Deserialize(ConstructSszObject<AltairUpdate>(path)),
                "02000001" => BellatrixUpdate.Deserialize(ConstructSszObject<BellatrixUpdate>(path)),
                "03000001" => CapellaUpdate.Deserialize(ConstructSszObject<CapellaUpdate>(path)),
                "04000001" => DenebUpdate.Deserialize(ConstructSszObject<DenebUpdate>(path)),
                _ => default
            } ?? throw new InvalidOperationException();
        });

    private static TResult? GetResult<TResult>(string filePath, string genesisValidatorsRoot, string forkDigest, Func<string, string, TResult> selector)
    {
        var forkVersion = ComputeForkVersionFromDigest(genesisValidatorsRoot, forkDigest).AsSpan().ToHexString();
        return selector(filePath, forkVersion);
    }

    private static ForkVersion ComputeForkVersionFromDigest(string genesisValidatorsRoot, string forkDigest)
    {
        var root = new Root(Bytes.FromHexString(genesisValidatorsRoot));
        var digest = new ForkDigest(Bytes.FromHexString(forkDigest));

        if (Helpers.ComputeForkDigest(Preset.ALTAIR_FORK_VERSION, root).Equals(digest))
        {
            return Preset.ALTAIR_FORK_VERSION;
        }

        if (Helpers.ComputeForkDigest(Preset.BELLATRIX_FORK_VERSION, root).Equals(digest))
        {
            return Preset.BELLATRIX_FORK_VERSION;
        }
        
        if (Helpers.ComputeForkDigest(Preset.CAPELLA_FORK_VERSION, root).Equals(digest))
        {
            return Preset.CAPELLA_FORK_VERSION;
        }
        
        return Helpers.ComputeForkDigest(Preset.EIP4844_FORK_VERSION, root).Equals(digest) ? Preset.EIP4844_FORK_VERSION : default;
    }

    private static bool LightClientStoreChecks(LightClientStore store, Checks checks)
    {
        var hashTreeRoot =
            new Root(SszContainer.HashTreeRoot(BeaconBlockHeaderSSZ.Serialize(store.FinalizedHeader.Beacon),
                SizePreset.MinimalPreset));
        var beaconRoot = new Root(Bytes.FromHexString(checks.finalized_header.beacon_root));
        
        if (store.FinalizedHeader.Beacon.Slot != (Slot)checks.finalized_header.slot ||
            !hashTreeRoot.Equals(beaconRoot)) return false;

        hashTreeRoot = new Root(SszContainer.HashTreeRoot(BeaconBlockHeaderSSZ.Serialize(store.OptimisticHeader.Beacon),
            SizePreset.MinimalPreset));
        beaconRoot = new Root(Bytes.FromHexString(checks.optimistic_header.beacon_root));
        
        return store.OptimisticHeader.Beacon.Slot == (Slot)checks.optimistic_header.slot &&
               hashTreeRoot.Equals(beaconRoot);
    }

    private static string DetermineTestType(string filePath)
    {
        var subdirectories = filePath.Split('/');
        return subdirectories[^2];
    }
    
    private static int ExtractUpdateNumber(string file)
    {
        var fileName = Path.GetFileNameWithoutExtension(file);
        var parts = fileName.Split('_');
        return int.Parse(parts[1]);
    }
}