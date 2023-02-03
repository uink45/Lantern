using System.Security.Cryptography;
using System.Collections;
using Lantern.Config;
using Lantern.Types.Containers;
using Lantern.Types.Basic;
using Lantern.Types.Crypto;
using Lantern.SSZ;
using Lantern.SSZ.Consensus;

namespace Lantern.Core;

public static class Helpers
{
    public static int FloorLog2(int value)
    {
        return (int)Math.Floor(Math.Log2(value));
    }

    public static int Sum(BitArray bitArray)
    {
        return bitArray.OfType<bool>().Count(x => x);
    }

    public static Epoch ComputeEpochAtSlot(Slot slot)
    {
        return new Epoch(slot / Preset.SLOTS_PER_EPOCH);
    }

    public static ulong ComputeSyncCommitteePeriod(Epoch epoch)
    {
        return (ulong)(epoch / Preset.EPOCHS_PER_SYNC_COMMITTEE_PERIOD);
    }

    public static ulong ComputeSyncCommitteePeriodAtSlot(Slot slot)
    {
        return ComputeSyncCommitteePeriod(ComputeEpochAtSlot(slot));
    }

    public static Root ComputeForkDataRoot(ForkVersion currentVersion, Root genesisValidatorsRoot)
    {
        ForkData forkData = new ForkData(currentVersion, genesisValidatorsRoot);
        ForkDataSSZ forkDataSSZ = ForkDataSSZ.Serialize(forkData);

        return new Root(Merkleizer.HashTreeRoot(SszContainer.GetContainer<ForkDataSSZ>(SizePreset.Empty), forkDataSSZ));
    }

    public static ForkVersion ComputeForkVersion(Epoch epoch)
    {
        if (epoch >= Preset.CAPELLA_FORK_EPOCH)
        {
            return Preset.CAPELLA_FORK_VERSION;
        }

        if (epoch >= Preset.BELLATRIX_FORK_EPOCH)
        {
            return Preset.BELLATRIX_FORK_VERSION;
        }

        if (epoch >= Preset.ALTAIR_FORK_EPOCH)
        {
            return Preset.ALTAIR_FORK_VERSION;
        }       
        
        return Preset.GENESIS_FORK_VERSION;
    }

    public static Root ComputeSigningRoot<T>(object sszObject, Domain domain, SizePreset sizePreset)
    {
        var sszType = SszContainer.GetContainer<T>(sizePreset);
        SigningData signingData = new SigningData(new Root(Merkleizer.HashTreeRoot(sszType, sszObject)), domain);
        SigningDataSSZ signingDataSSZ = SigningDataSSZ.Serialize(signingData);
 
        return new Root(Merkleizer.HashTreeRoot(SszContainer.GetContainer<SigningDataSSZ>(SizePreset.Empty), signingDataSSZ));
    }

    public static Domain ComputeDomain(DomainType domainType, ForkVersion forkVersion, Root genesisValidatorsRoot)
    {
        Root forkDataRoot = ComputeForkDataRoot(forkVersion, genesisValidatorsRoot);
        return new Domain(domainType.AsSpan().ToArray().Concat(forkDataRoot.Bytes).Take(32).ToArray());
    }

    public static bool IsValidMerkleBranch(Root leaf, Bytes32[] branch, int depth, int index, Root root)
    {
        byte[] value = leaf.Bytes;

        for (int i = 0; i < depth; i++)
        {
            int currentIndex = (index >> i) & 1;
            if (currentIndex == 1)
            {
                value = SHA256.HashData(branch[i].AsSpan().ToArray().Concat(value).ToArray());                
            }
            else
            {
                value = SHA256.HashData(value.Concat(branch[i].AsSpan().ToArray()).ToArray());
            }
        }

        return value.SequenceEqual(root.Bytes);
    }

    public static bool IsValidLightClientHeader(LightClientHeader header)
    {
        Epoch epoch = ComputeEpochAtSlot(header.Beacon.Slot);

        if (epoch < Preset.CAPELLA_FORK_EPOCH)
        {
            return (header.Execution.Equals(ExecutionPayloadHeader.Zero)
                && header.ExecutionBranch.SequenceEqual(Enumerable.Range(0, FloorLog2(Constants.EXECUTION_PAYLOAD_INDEX)).Select(i => new Bytes32()).ToArray()));
        }

        return IsValidMerkleBranch(GetLcExecutionRoot(header),
            header.ExecutionBranch,
            FloorLog2(Constants.EXECUTION_PAYLOAD_INDEX),
            GetSubtreeIndex(Constants.EXECUTION_PAYLOAD_INDEX),
            header.Beacon.BodyRoot);
    }

    public static bool IsSyncCommitteeUpdate(LightClientUpdate update)
    {        
        return !update.NextSyncCommitteeBranch.SequenceEqual(Enumerable.Range(0, FloorLog2(Constants.NEXT_SYNC_COMMITTEE_INDEX)).Select(i => new Bytes32()).ToArray());
    }

    public static bool IsFinalityUpdate(LightClientUpdate update)
    {
        return !update.FinalityBranch.SequenceEqual(Enumerable.Range(0, FloorLog2(Constants.FINALISED_ROOT_INDEX)).Select(i => new Bytes32()).ToArray());
    }

    public static bool IsBetterUpdate(LightClientUpdate newUpdate, LightClientUpdate oldUpdate)
    {
        int maxActiveParticipants = newUpdate.SyncAggregate.SyncCommitteeBits.Count;
        int newNumActiveParticipants = Sum(newUpdate.SyncAggregate.SyncCommitteeBits);
        int oldNumActiveParticipants = Sum(oldUpdate.SyncAggregate.SyncCommitteeBits);
        int newHasSuperMajority = Convert.ToInt32(newNumActiveParticipants * 3 >= maxActiveParticipants * 2);
        int oldHasSuperMajority = Convert.ToInt32(oldNumActiveParticipants * 3 >= maxActiveParticipants * 2);

        if(newHasSuperMajority != oldHasSuperMajority)
        {
            return Convert.ToBoolean(newHasSuperMajority > oldHasSuperMajority);
        }

        if (!Convert.ToBoolean(newHasSuperMajority) && newNumActiveParticipants != oldNumActiveParticipants)
        {
            return Convert.ToBoolean(newNumActiveParticipants > oldNumActiveParticipants);
        }

        int newHasRelevantSyncCommittee = Convert.ToInt32(IsSyncCommitteeUpdate(newUpdate)
            && (ComputeSyncCommitteePeriodAtSlot(newUpdate.AttestedHeader.Beacon.Slot)
            == ComputeSyncCommitteePeriodAtSlot(newUpdate.SignatureSlot)));

        int oldHasRelevantSyncCommittee = Convert.ToInt32(IsSyncCommitteeUpdate(oldUpdate)
            && (ComputeSyncCommitteePeriodAtSlot(oldUpdate.AttestedHeader.Beacon.Slot)
            == ComputeSyncCommitteePeriodAtSlot(oldUpdate.SignatureSlot)));

        if(newHasRelevantSyncCommittee != oldHasRelevantSyncCommittee)
        {
            return Convert.ToBoolean(newHasRelevantSyncCommittee);
        }

        int newHasFinality = Convert.ToInt32(IsFinalityUpdate(newUpdate));
        int oldHasFinality = Convert.ToInt32(IsFinalityUpdate(oldUpdate));

        if(newHasFinality != oldHasFinality)
        {
            return Convert.ToBoolean(newHasFinality);
        }

        if(Convert.ToBoolean(newHasFinality))
        {
            int newHasSyncCommitteeFinality = Convert.ToInt32(ComputeSyncCommitteePeriodAtSlot(newUpdate.FinalizedHeader.Beacon.Slot)
                == ComputeSyncCommitteePeriodAtSlot(newUpdate.AttestedHeader.Beacon.Slot));

            int oldHasSyncCommitteeFinality = Convert.ToInt32(ComputeSyncCommitteePeriodAtSlot(oldUpdate.FinalizedHeader.Beacon.Slot)
                == ComputeSyncCommitteePeriodAtSlot(oldUpdate.AttestedHeader.Beacon.Slot));

            if(newHasSyncCommitteeFinality != oldHasSyncCommitteeFinality)
            {
                return Convert.ToBoolean(newHasSyncCommitteeFinality);
            }
        }

        if(newNumActiveParticipants != oldNumActiveParticipants)
        {
            return Convert.ToBoolean(newNumActiveParticipants > oldNumActiveParticipants);
        }

        if(newUpdate.AttestedHeader.Beacon.Slot != oldUpdate.AttestedHeader.Beacon.Slot)
        {
            return Convert.ToBoolean(newUpdate.AttestedHeader.Beacon.Slot < oldUpdate.AttestedHeader.Beacon.Slot);
        }

        return Convert.ToBoolean(newUpdate.SignatureSlot < oldUpdate.SignatureSlot);
    }

    public static bool IsNextSyncCommitteeKnown(LightClientStore store)
    {
        return !store.NextSyncCommittee.Equals(SyncCommittee.Zero);
    }

    public static int GetSafetyThreshold(LightClientStore store)
    {
        return (int)(Math.Max(store.PreviousMaxActiveParticipants, store.CurrentMaxActiveParticipants) / 2);
    }

    public static int GetSubtreeIndex(int generalizedIndex)
    {
        return (int)(generalizedIndex % Math.Pow(2, FloorLog2(generalizedIndex)));
    }

    public static Root GetLcExecutionRoot(LightClientHeader header)
    {
        Epoch epoch = ComputeEpochAtSlot(header.Beacon.Slot);

        if (epoch >= Preset.CAPELLA_FORK_EPOCH)
        {
            return new Root(Merkleizer.HashTreeRoot(SszContainer.GetContainer<ExecutionPayloadHeaderSSZ>(SizePreset.MinimalPreset), ExecutionPayloadHeaderSSZ.Serialize(header.Execution)));
        }

        return Root.Zero;
    }
}

