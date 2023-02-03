using IronSnappy;
using Lantern.Config;
using Lantern.Core;
using Lantern.SSZ;
using Lantern.SSZ.Consensus;
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
    public void SetUp()
    {
        Preset.SetPreset<Minimal>();
        
    }

    private readonly string _testsLocation = Path.Combine(AppContext.BaseDirectory, "MockupData", "minimal");

    private delegate TU DeserializeDelegate<in T, out TU>(T obj);
    
    [Test]
    public void ExecuteTests()
    {
        ExecuteProofTests();
      
    }
    
    
    private void ExecuteProofTests()
    {
        var sszFiles = Directory.GetFiles(_testsLocation, "object.ssz_snappy", SearchOption.AllDirectories);
        var yamlFiles = Directory.GetFiles(_testsLocation, "proof.yaml", SearchOption.AllDirectories);
        
        foreach (var (sszFile, yamlFile) in sszFiles.Zip(yamlFiles, (ssz, yaml) => (ssz, yaml)))
        {
            using var reader = new StreamReader(yamlFile);
            var deserializer = new DeserializerBuilder().Build();
            var proofData = deserializer.Deserialize<ProofData>(reader);
            
            
        }
        
    }

    private void ExecuteSyncTests(string hardForkName)
    {
        
    }

    private void ExecuteRankingTests(string hardForkName)
    {
        
    }
    
    private void ExecuteSyncProcessor(string pyspecTest)
    {
        var updateFilePath = Path.Combine(_testsLocation, pyspecTest);
        var metaFile = Path.Combine(updateFilePath, "meta.yaml");
        var bootstrapFile = Path.Combine(updateFilePath, "bootstrap.ssz_snappy");
        var stepsFile = Path.Combine(updateFilePath, "steps.yaml");

        var deserializer = new DeserializerBuilder().Build();
        var bootstrapData = DeserializeYaml<BootstrapData>(metaFile, deserializer);
        var blsUtility = new BLSUtility();
        var bootstrap =
            DeserializeFromFile<LightClientBootstrapSSZ, LightClientBootstrap>(bootstrapFile,
                LightClientBootstrapSSZ.Deserialize);
        var trustedRoot = new Root(Bytes.FromHexString(bootstrapData.trusted_block_root));
        var genesisValidatorsRoot = new Root(Bytes.FromHexString(bootstrapData.genesis_validators_root));
        var store = LightClientStore.InitializeLightClientStore(trustedRoot, bootstrap, SizePreset.MinimalPreset);

        using var reader = new StreamReader(stepsFile);
        var steps = deserializer.Deserialize<List<Step>>(reader);
        foreach (var step in steps.Where(step => !step.Equals(null)))
        {
            if (step.process_update != null)
            {
                var update = DeserializeFromFile<LightClientUpdateSSZ, LightClientUpdate>(
                    Path.Combine(updateFilePath, step.process_update.update + ".ssz_snappy"),
                    LightClientUpdateSSZ.Deserialize);
                Processors.ProcessLightClientUpdate(store, update, (Slot)step.process_update.current_slot,
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
            else if (step.upgrade_store != null)
            {
                
            }
        }
    }
    
    private static T DeserializeYaml<T>(string filePath, IDeserializer deserializer)
    {
        using var reader = new StreamReader(filePath);
        return deserializer.Deserialize<T>(reader);
    }

    private static TU DeserializeFromFile<T, TU>(string fullFilePath, DeserializeDelegate<T, TU> deserialize)
    {
        var fileBytes = File.ReadAllBytes(fullFilePath);
        var deserializedObject =
            Deserialize.DeserialiseSSZObject<T>(Snappy.Decode(fileBytes), SizePreset.MinimalPreset);
        var obj = deserialize(deserializedObject);
        return obj;
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
    
    
}