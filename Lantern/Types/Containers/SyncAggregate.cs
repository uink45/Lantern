using System.Collections;
using Lantern.Types.Crypto.BLS;
using Lantern.Config;
using System.Security.Cryptography.X509Certificates;

namespace Lantern.Types.Containers
{
    public class SyncAggregate : IEquatable<SyncAggregate>
    {
        public readonly BitArray SyncCommitteeBits;
        public readonly BlsSignature SyncCommitteeSignature;

        public static readonly SyncAggregate Zero =
            new SyncAggregate(new BitArray(Preset.SYNC_COMMITTEE_SIZE), BlsSignature.Zero);

        public SyncAggregate(BitArray syncCommitteeBits, BlsSignature syncCommitteeSignature)
        {
            SyncCommitteeBits = syncCommitteeBits;
            SyncCommitteeSignature = syncCommitteeSignature;
        }

        public bool Equals(SyncAggregate? other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(other, this))
            {
                return true;
            }

            return SyncCommitteeBits.Equals(other.SyncCommitteeBits)
                && SyncCommitteeSignature.Equals(other.SyncCommitteeSignature);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SyncCommitteeBits, SyncCommitteeSignature);
        }

        public override bool Equals(object? obj)
        {
            var other = obj as SyncAggregate;
            return !(other is null) && Equals(other);
        }
    }
}
