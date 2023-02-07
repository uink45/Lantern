using IronSnappy;
using Lantern.Config;
using Lantern.Core;
using Lantern.SSZ;
using Lantern.SSZ.Types;
using Lantern.SSZ.Types.Capella;
using Lantern.SSZ.Types.Deneb;
using Lantern.Types.Basic;
using Lantern.Types.Containers;
using Lantern.Types.Crypto;
using NUnit.Framework;

namespace Lantern.Tests;

[TestFixture]
public class HelpersTests
{
    [SetUp]
    public void SetUp()
    {
        Preset.SetPreset<Minimal>();
    }

    private readonly string _testsLocation = Path.Combine(AppContext.BaseDirectory, "MockupData", "minimal");
    
    [Test]
    public void ComputeSyncCommitteePeriodAtSlot()
    {
        var slot = new Slot(5612900);
        Assert.That(Helpers.ComputeSyncCommitteePeriodAtSlot(slot), Is.EqualTo(87701),
            "ComputeSyncCommitteePeriodAtSlot should return the correct value for the given input");
    }

    [Test]
    public void IsValidLightClientHeader()
    {
        var result = Helpers.IsValidLightClientHeader(LightClientHeader.Zero);
        Assert.That(result, Is.EqualTo(false),
            "IsValidLightClientHeader should return 'true' for an empty LightClientHeader");
    }

    [Test]
    public void TestUpdateChecks()
    {
        var updateFile = GetUpdateFileLocation("update_0xe7c1b621e495c3987e97fd40456b2dd8e233e37b5ef487d48d3f44665d783ccf_sf.ssz_snappy");
        TestUpdate(true, updateFile, Helpers.IsSyncCommitteeUpdate);
        TestUpdate(true, updateFile, Helpers.IsFinalityUpdate);
    }

    [Test]
    public void TestIsBetterUpdate()
    {
        var firstUpdate = ConstructSszObject<CapellaUpdate>(GetUpdateFileLocation("update_0xe7c1b621e495c3987e97fd40456b2dd8e233e37b5ef487d48d3f44665d783ccf_sf.ssz_snappy"));
        var secondUpdate = ConstructSszObject<CapellaUpdate>(GetUpdateFileLocation("update_0x955cc1e5b12fff32f44794afc64a5bdbebc324edd649e922c0e18693b92a4f75_sf.ssz_snappy"));
        var result = Helpers.IsBetterUpdate(CapellaUpdate.Deserialize(firstUpdate),
            CapellaUpdate.Deserialize(secondUpdate));
        Assert.That(result, Is.EqualTo(true),
            "IsBetterUpdate should return true if the first update is considered better than the second update");
    }

    [Test]
    public void IsNextSyncCommitteeKnown()
    {
        var updateFile =ConstructSszObject<CapellaUpdate>(GetUpdateFileLocation("update_0xe7c1b621e495c3987e97fd40456b2dd8e233e37b5ef487d48d3f44665d783ccf_sf.ssz_snappy"));
        var result = Helpers.IsNextSyncCommitteeKnown(LightClientStore.Zero);
        Assert.That(result, Is.EqualTo(false),
            "IsNextSyncCommitteeKnown should return false when the next sync committee is not known");
        
        var store = new LightClientStore(LightClientHeader.Zero,
            SyncCommittee.Zero,
            SyncCommitteeSSZ.Deserialize(updateFile.NextSyncCommittee),
            LightClientUpdate.Zero,
            LightClientHeader.Zero,
            0,
            0);

        result = Helpers.IsNextSyncCommitteeKnown(store);
        Assert.That(result, Is.EqualTo(true),
            $"IsNextSyncCommitteeKnown should return true for update file {updateFile} when the next sync committee is known");
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
        var updateSsz = ConstructSszObject<DenebUpdate>(GetUpdateFileLocation("update_0x42a429738075c8e3a16d6d4f9772148b627eb494d65c6e8b7dbd45ea2ce5dda1_sf.ssz_snappy"));
        var update = DenebUpdate.Deserialize(updateSsz);
        var hashTreeRoot = new Root(Merkleizer.HashTreeRoot(
            SszContainer.GetContainer<DenebExecutionPayloadHeader>(SizePreset.MinimalPreset),
            updateSsz.AttestedHeader.Execution));
        var result = Helpers.GetLcExecutionRoot(update.AttestedHeader);
        Assert.That(hashTreeRoot, Is.EqualTo(result),
            "GetLcExecutionRoot should return the hash tree root of ExecutionPayloadHeader");
    }

    private void TestUpdate(bool expectedResult, string updateFilePath, Func<LightClientUpdate, bool> updateCheckMethod)
    {
        var updateSsz = ConstructSszObject<CapellaUpdate>(updateFilePath);
        var result = updateCheckMethod(CapellaUpdate.Deserialize(updateSsz));
        Assert.That(result, Is.EqualTo(expectedResult),
            $"{updateCheckMethod.Method.Name} should return {expectedResult} for {updateFilePath}");
    }

    private static T ConstructSszObject<T>(string filePath)
    {
        return Deserialize.DeserialiseSSZObject<T>(Snappy.Decode(File.ReadAllBytes(filePath)),
            SizePreset.MinimalPreset);
    }

    private string GetUpdateFileLocation(string name)
    {
        return Directory.GetFiles(_testsLocation, name, SearchOption.AllDirectories).First();
    }
}