using Lantern.SSZ;
using Lantern.Types.Crypto.BLS;
using Lantern.Types.Containers;
using System.Collections;

namespace Lantern.SSZ.Types
{
	public class SyncAggregateSSZ
	{
        [SszElement(0, "Bitvector[SYNC_COMMITTEE_SIZE]")]
        public List<bool> SyncCommitteeBits { get; set; }

        [SszElement(1, "Vector[uint8, 96]")]
        public byte[] SyncCommitteeSignature { get; set; }

        public static SyncAggregateSSZ Serialize(SyncAggregate syncAggregate)
        {
            SyncAggregateSSZ syncAggregateSSZ = new SyncAggregateSSZ();
            syncAggregateSSZ.SyncCommitteeBits = syncAggregate.SyncCommitteeBits.Cast<bool>().ToList();
            syncAggregateSSZ.SyncCommitteeSignature = syncAggregate.SyncCommitteeSignature.Bytes;

            return syncAggregateSSZ;
        }

        public static SyncAggregate Deserialize(SyncAggregateSSZ syncAggregateSSZ)
        {
            return new SyncAggregate(new BitArray(syncAggregateSSZ.SyncCommitteeBits.ToArray()), new BlsSignature(syncAggregateSSZ.SyncCommitteeSignature));
        }
    }
}

