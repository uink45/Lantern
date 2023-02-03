using System;
using Lantern.Types.Basic;
using Lantern.Types.Containers;
using Lantern.Types.Crypto;
using Lantern.Config;
using Lantern.Core;
using System.Security.Cryptography.X509Certificates;

namespace Lantern.Types.Containers
{
	public class LightClientUpdate : IEquatable<LightClientUpdate>
    {
		public readonly LightClientHeader AttestedHeader;
		public readonly SyncCommittee NextSyncCommittee;
		public readonly Bytes32[] NextSyncCommitteeBranch;
		public readonly LightClientHeader FinalizedHeader;
		public readonly Bytes32[] FinalityBranch;
		public readonly SyncAggregate SyncAggregate;
		public readonly Slot SignatureSlot;

		public static readonly LightClientUpdate Zero = new LightClientUpdate(LightClientHeader.Zero, SyncCommittee.Zero,
            Enumerable.Range(0, Helpers.FloorLog2(Constants.NEXT_SYNC_COMMITTEE_INDEX)).Select(i => new Bytes32()).ToArray(), LightClientHeader.Zero,
			Enumerable.Range(0, Helpers.FloorLog2(Constants.FINALISED_ROOT_INDEX)).Select(i => new Bytes32()).ToArray(), SyncAggregate.Zero, Slot.Zero);
 
        public LightClientUpdate(LightClientHeader attestedHeader, SyncCommittee nextSyncCommittee,
			Bytes32[] nextSyncCommitteeBranch, LightClientHeader finalizedHeader, Bytes32[] finalityBranch,
			SyncAggregate syncAggregate, Slot signatureSlot)
		{

			AttestedHeader = attestedHeader;
			NextSyncCommittee = nextSyncCommittee;
			NextSyncCommitteeBranch = nextSyncCommitteeBranch;
			FinalizedHeader = finalizedHeader;
			FinalityBranch = finalityBranch;
			SyncAggregate = syncAggregate;
			SignatureSlot = signatureSlot;
        }

        public bool Equals(LightClientUpdate? other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(other, this))
            {
                return true;
            }

            return AttestedHeader.Equals(other.AttestedHeader)
                && NextSyncCommittee.Equals(other.NextSyncCommittee)
                && NextSyncCommitteeBranch.SequenceEqual(other.NextSyncCommitteeBranch)
                && FinalizedHeader.Equals(other.FinalizedHeader)
                && FinalityBranch.SequenceEqual(other.FinalityBranch)
                && SyncAggregate.Equals(other.SyncAggregate)
                && SignatureSlot.Equals(other.SignatureSlot);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AttestedHeader, NextSyncCommittee, NextSyncCommitteeBranch, FinalizedHeader, FinalityBranch, SyncAggregate, SignatureSlot);
        }

        public override bool Equals(object? obj)
        {
            var other = obj as LightClientHeader;
            return other is not null && Equals(other);
        }
    }
}

