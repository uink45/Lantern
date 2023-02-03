using Lantern.Types.Crypto.BLS;
using Lantern.Config;
using Lantern.Core;
using Lantern.Types.Basic;

namespace Lantern.Types.Containers
{
    public class SyncCommittee : IEquatable<SyncCommittee>
    {
        public readonly BlsPublicKey[] PublicKeys;
        public readonly BlsPublicKey AggregatePublicKey;

        public static readonly SyncCommittee Zero =
            new SyncCommittee(Enumerable.Range(0, Preset.SYNC_COMMITTEE_SIZE)
                                        .Select(i => new BlsPublicKey())
                                        .ToArray(), BlsPublicKey.Zero);

        public SyncCommittee(BlsPublicKey[] pubKeys, BlsPublicKey aggregatePublicKey)
        {
            PublicKeys = pubKeys;
            AggregatePublicKey = aggregatePublicKey;
        }

        public bool Equals(SyncCommittee? other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(other, this))
            {
                return true;
            }

            return PublicKeys.SequenceEqual(other.PublicKeys)
                && AggregatePublicKey.Equals(other.AggregatePublicKey);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PublicKeys, AggregatePublicKey);
        }

        public override bool Equals(object? obj)
        {
            var other = obj as SyncCommittee;
            return !(other is null) && Equals(other);
        }
    }
}
