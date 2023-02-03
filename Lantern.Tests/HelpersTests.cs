using IronSnappy;
using Lantern.Config;
using Lantern.Core;
using Lantern.SSZ;
using Lantern.SSZ.Consensus;
using Lantern.Tests.Models;
using Lantern.Types.Basic;
using Lantern.Types.Containers;
using Lantern.Types.Crypto;
using NUnit.Framework;
using YamlDotNet.Serialization;

namespace Lantern.Tests;

[TestFixture]
public class HelpersTests
{
    [SetUp]
    public void SetUp()
    {
        Preset.SetPreset<Minimal>();
        const string specVersion = "capella";
        _syncTestsLocation = Path.Combine(AppContext.BaseDirectory, "MockupData", "minimal", specVersion, "light_client", "sync",
            "pyspec_tests");
        _proofTestsLocation = Path.Combine(AppContext.BaseDirectory, "MockupData", "minimal", specVersion, "light_client",
            "single_merkle_proof", "BeaconState");
        _updateFiles = new List<string>(Directory.GetFiles(Path.Combine(_syncTestsLocation, "light_client_sync"),
            "update_*.ssz_snappy"));
    }

    private string _syncTestsLocation = "";
    private string _proofTestsLocation = "";
    private List<string> _updateFiles = new();

    [Test]
    public void ComputeSyncCommitteePeriodAtSlot()
    {
        var slot = new Slot(5612900);
        Assert.That(Helpers.ComputeSyncCommitteePeriodAtSlot(slot), Is.EqualTo(87701),
            "ComputeSyncCommitteePeriodAtSlot should return the correct value for the given input");
    }

    [Test]
    public void IsValidMerkleBranch()
    {
        TestIsValidMerkleProof("current_sync_committee_merkle_proof");
        TestIsValidMerkleProof("finality_root_merkle_proof");
        TestIsValidMerkleProof("next_sync_committee_merkle_proof");
    }

    [Test]
    public void IsValidLightClientHeader()
    {
        var result = Helpers.IsValidLightClientHeader(LightClientHeader.Zero);
        Assert.That(result, Is.EqualTo(false),
            "IsValidLightClientHeader should return 'true' for an empty LightClientHeader");
    }

    [Test]
    public void TestIsSyncCommitteeUpdate()
    {
        TestUpdate(true, _updateFiles[0], Helpers.IsSyncCommitteeUpdate);
    }

    [Test]
    public void TestIsFinalityUpdate()
    {
        TestUpdate(true, _updateFiles[0], Helpers.IsFinalityUpdate);
    }

    [Test]
    public void IsBetterUpdate()
    {
        var firstUpdate = ConstructSszObject<LightClientUpdateSSZ>(_updateFiles[1]);
        var secondUpdate = ConstructSszObject<LightClientUpdateSSZ>(_updateFiles[0]);

        var result = Helpers.IsBetterUpdate(LightClientUpdateSSZ.Deserialize(firstUpdate),
            LightClientUpdateSSZ.Deserialize(secondUpdate));
        Assert.That(result, Is.EqualTo(true),
            "IsBetterUpdate should return true if the first update is considered better than the second update");
    }

    [Test]
    public void IsNextSyncCommitteeKnown()
    {
        var result = Helpers.IsNextSyncCommitteeKnown(LightClientStore.Zero);
        Assert.That(result, Is.EqualTo(false),
            "IsNextSyncCommitteeKnown should return false when the next sync committee is not known");

        var updateSsz = ConstructSszObject<LightClientUpdateSSZ>(_updateFiles[0]);
        var store = new LightClientStore(LightClientHeader.Zero,
            SyncCommittee.Zero,
            SyncCommitteeSSZ.Deserialize(updateSsz.NextSyncCommittee),
            LightClientUpdate.Zero,
            LightClientHeader.Zero,
            0,
            0);

        result = Helpers.IsNextSyncCommitteeKnown(store);
        Assert.That(result, Is.EqualTo(true),
            $"IsNextSyncCommitteeKnown should return true for update file {_updateFiles[0]} when the next sync committee is known");
    }

    [Test]
    public void GetSafetyThreshold()
    {
        var store = new LightClientStore(
            LightClientHeader.Zero,
            SyncCommittee.Zero,
            SyncCommittee.Zero,
            LightClientUpdate.Zero,
            LightClientHeader.Zero,
            430,
            458);

        var safetyThreshold = Helpers.GetSafetyThreshold(store);
        Assert.That(safetyThreshold, Is.EqualTo(229),
            "GetSafetyThreshold should return the correct safety threshold value");
    }

    [Test]
    public void GetSubTreeIndex()
    {
        var subtreeIndex = Helpers.GetSubtreeIndex(Constants.CURRENT_SYNC_COMMITTEE_INDEX);
        Assert.That(subtreeIndex, Is.EqualTo(22),
            "GetSubtreeIndex should return the correct sub tree index for CURRENT_SYNC_COMMITTEE_INDEX");

        subtreeIndex = Helpers.GetSubtreeIndex(Constants.NEXT_SYNC_COMMITTEE_INDEX);
        Assert.That(subtreeIndex, Is.EqualTo(23),
            "GetSubtreeIndex should return the correct sub tree index for NEXT_SYNC_COMMITTEE_INDEX");
    }

    [Test]
    public void GetLcExecutionRoot()
    {
        var updateSsz = ConstructSszObject<LightClientUpdateSSZ>(_updateFiles[0]);
        var update = LightClientUpdateSSZ.Deserialize(updateSsz);
        var hashTreeRoot = new Root(Merkleizer.HashTreeRoot(
            SszContainer.GetContainer<ExecutionPayloadHeaderSSZ>(SizePreset.MinimalPreset),
            updateSsz.AttestedHeader.Execution));
        var result = Helpers.GetLcExecutionRoot(update.AttestedHeader);
        Assert.That(hashTreeRoot, Is.EqualTo(result),
            "GetLcExecutionRoot should return the hash tree root of ExecutionPayloadHeader");
    }

    private void TestUpdate(bool expectedResult, string updateFilePath, Func<LightClientUpdate, bool> updateCheckMethod)
    {
        var updateSsz = ConstructSszObject<LightClientUpdateSSZ>(updateFilePath);
        var result = updateCheckMethod(LightClientUpdateSSZ.Deserialize(updateSsz));
        Assert.That(result, Is.EqualTo(expectedResult),
            $"{updateCheckMethod.Method.Name} should return {expectedResult} for {updateFilePath}");
    }

    private void TestIsValidMerkleProof(string testName)
    {
        var objectFilePath = Path.Combine(_proofTestsLocation, testName, "object.ssz_snappy");
        var deserializer = new DeserializerBuilder().Build();

        ProofData proofData;
        using (var reader = new StreamReader(Path.Combine(_proofTestsLocation, testName, "proof.yaml")))
        {
            proofData = deserializer.Deserialize<ProofData>(reader);
        }

        var stateSsz = ConstructSszObject<BeaconStateSSZ>(objectFilePath);
        var leaf = new Root(Bytes.FromHexString(proofData.leaf));

        var depth = Helpers.FloorLog2(proofData.leaf_index);
        var index = Helpers.GetSubtreeIndex(proofData.leaf_index);

        var root = new Root(Merkleizer.HashTreeRoot(SszContainer.GetContainer<BeaconStateSSZ>(SizePreset.MinimalPreset),
            stateSsz));
        var branch = proofData.branch.Select(b => new Bytes32(Bytes.FromHexString(b))).ToArray();

        var result = Helpers.IsValidMerkleBranch(leaf, branch, depth, index, root);
        Assert.That(result, Is.EqualTo(true), $"IsValidMerkleBranch should return true for the test case: {testName}");
    }

    private static T ConstructSszObject<T>(string filePath)
    {
        return Deserialize.DeserialiseSSZObject<T>(Snappy.Decode(File.ReadAllBytes(filePath)),
            SizePreset.MinimalPreset);
    }
}