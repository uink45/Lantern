using Lantern.Types.Basic;
using Lantern.Types.Containers;
using Lantern.Types.Crypto;
using Lantern.Config;
using Lantern.Core;
namespace Lantern.SSZ.Types.Altair;

public class AltairUpdate
{
        [SszElement(0, "Container")]
        public AltairHeader AttestedHeader { get; set; }

        [SszElement(1, "Container")]
        public SyncCommitteeSSZ NextSyncCommittee { get; set; }

        [SszElement(2, "Vector[Vector[uint8, 32], NEXT_SYNC_COMMITTEE_DEPTH]")]
        public byte[][] NextSyncCommitteeBranch { get; set; }

        [SszElement(3, "Container")]
        public AltairHeader FinalizedHeader { get; set; }

        [SszElement(4, "Vector[Vector[uint8, 32], FINALIZED_ROOT_DEPTH]")]
        public byte[][] FinalityBranch { get; set; }

        [SszElement(5, "Container")]
        public SyncAggregateSSZ SyncAggregate { get; set; }

        [SszElement(6, "uint64")]
        public ulong SignatureSlot { get; set; }

        public static AltairUpdate Serialize(LightClientUpdate update)
        {
            AltairUpdate updateSSZ = new AltairUpdate();
            updateSSZ.AttestedHeader = AltairHeader.Serialize(update.AttestedHeader);
            updateSSZ.NextSyncCommittee = SyncCommitteeSSZ.Serialize(update.NextSyncCommittee);
            updateSSZ.NextSyncCommitteeBranch = Enumerable.Range(0, Helpers.FloorLog2(Constants.NEXT_SYNC_COMMITTEE_INDEX))
                                    .Select(i => update.NextSyncCommitteeBranch[i].AsSpan().ToArray())
                                    .ToArray();
            updateSSZ.FinalizedHeader = AltairHeader.Serialize(update.FinalizedHeader);
            updateSSZ.FinalityBranch = Enumerable.Range(0, Helpers.FloorLog2(Constants.FINALISED_ROOT_INDEX))
                                    .Select(i => update.FinalityBranch[i].AsSpan().ToArray())
                                    .ToArray();
            updateSSZ.SyncAggregate = SyncAggregateSSZ.Serialize(update.SyncAggregate);
            updateSSZ.SignatureSlot = update.SignatureSlot;

            return updateSSZ;
        }

        public static LightClientUpdate Deserialize(AltairUpdate updateSSZ)
        {
            LightClientUpdate update = new LightClientUpdate(AltairHeader.Deserialize(updateSSZ.AttestedHeader),
                SyncCommitteeSSZ.Deserialize(updateSSZ.NextSyncCommittee),
                Enumerable.Range(0, Helpers.FloorLog2(Constants.NEXT_SYNC_COMMITTEE_INDEX))
                    .Select(i => new Bytes32(updateSSZ.NextSyncCommitteeBranch[i]))
                    .ToArray(),
                AltairHeader.Deserialize(updateSSZ.FinalizedHeader),
                Enumerable.Range(0, Helpers.FloorLog2(Constants.FINALISED_ROOT_INDEX))
                    .Select(i => new Bytes32(updateSSZ.FinalityBranch[i]))
                    .ToArray(),
                SyncAggregateSSZ.Deserialize(updateSSZ.SyncAggregate),
                new Slot(updateSSZ.SignatureSlot));

            return update;
        }
}