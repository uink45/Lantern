using Lantern.SSZ;
using Lantern.Config;
using Lantern.Types.Containers;
using Lantern.Types.Crypto.BLS;
using Lantern.Core;

namespace Lantern.SSZ.Types
{
	public class SyncCommitteeSSZ
	{
        [SszElement(0, "Vector[Vector[uint8,48], SYNC_COMMITTEE_SIZE]")]
        public byte[][] Pubkeys { get; set; }

        [SszElement(1, "Vector[uint8,48]")]
        public byte[] AggregatePubkey { get; set; }

        public static SyncCommitteeSSZ Serialize(SyncCommittee syncCommittee)
        {
            SyncCommitteeSSZ syncCommitteeSSZ = new SyncCommitteeSSZ();
            syncCommitteeSSZ.Pubkeys = Enumerable.Range(0, syncCommittee.PublicKeys.Length)
                                    .Select(i => syncCommittee.PublicKeys[i].Bytes.ToArray())
                                    .ToArray();
            syncCommitteeSSZ.AggregatePubkey = syncCommittee.AggregatePublicKey.Bytes;

            return syncCommitteeSSZ;
        }

        public static SyncCommittee Deserialize(SyncCommitteeSSZ syncCommitteeSSZ)
        {
            return new SyncCommittee(Enumerable.Range(0, syncCommitteeSSZ.Pubkeys.Length)
                                        .Select(i => new BlsPublicKey(syncCommitteeSSZ.Pubkeys[i]))
                                        .ToArray(), new BlsPublicKey(syncCommitteeSSZ.AggregatePubkey));
        }
    }
}

