using System;
using System.Diagnostics;
using Lantern.Core;
using Lantern.Config;
using Lantern.Types.Containers;
using Lantern.Types.Basic;
using Lantern.Types.Crypto;
using Lantern.SSZ;
using Lantern.SSZ.Types;

namespace Lantern.Types.Containers
{
    public class LightClientStore
    {
        public LightClientHeader FinalizedHeader { get; private set; }
        public SyncCommittee CurrentSyncCommittee { get; private set; }
        public SyncCommittee NextSyncCommittee { get; private set; }
        public LightClientUpdate BestValidUpdate { get; private set; }
        public LightClientHeader OptimisticHeader { get; private set; }
        public ulong PreviousMaxActiveParticipants { get; private set; }
        public ulong CurrentMaxActiveParticipants { get; private set; }

        public static readonly LightClientStore Zero = new LightClientStore(LightClientHeader.Zero, SyncCommittee.Zero, SyncCommittee.Zero,
            LightClientUpdate.Zero, LightClientHeader.Zero, 0, 0);

        public LightClientStore(LightClientHeader finalizedHeader, SyncCommittee currentSyncCommittee, SyncCommittee nextSyncCommittee,
            LightClientUpdate bestValidUpdate, LightClientHeader optimisticHeader, ulong previousMaxActiveParticipants, ulong currentMaxActiveParticipants)
        {
            FinalizedHeader = finalizedHeader;
            CurrentSyncCommittee = currentSyncCommittee;
            NextSyncCommittee = nextSyncCommittee;
            BestValidUpdate = bestValidUpdate;
            OptimisticHeader = optimisticHeader;
            PreviousMaxActiveParticipants = previousMaxActiveParticipants;
            CurrentMaxActiveParticipants = currentMaxActiveParticipants;
        }

        public void SetFinalizedHeader(LightClientHeader finalizedHeader) => FinalizedHeader = finalizedHeader;

        public void SetCurrentSyncCommittee(SyncCommittee currentSyncCommittee) => CurrentSyncCommittee = currentSyncCommittee;

        public void SetNextSyncCommittee(SyncCommittee nextSyncCommittee) => NextSyncCommittee = nextSyncCommittee;

        public void SetBestValidUpdate(LightClientUpdate bestValidUpdate) => BestValidUpdate = bestValidUpdate;

        public void SetOptimisticHeader(LightClientHeader optimisticHeader) => OptimisticHeader = optimisticHeader;

        public void SetPreviousMaxActiveParticipants(ulong previousMaxActiveParticipants) => PreviousMaxActiveParticipants = previousMaxActiveParticipants;

        public void SetCurrentMaxActiveParticipants(ulong currentMaxActiveParticipants) => CurrentMaxActiveParticipants = currentMaxActiveParticipants;

        public static LightClientStore InitializeLightClientStore(Root trustedBlockRoot, LightClientBootstrap bootstrap, SizePreset sizePreset)
        {
            bool result = Helpers.IsValidLightClientHeader(bootstrap.Header);
            Debug.Assert(result); // Remove Assert.

            Root leaf = new Root(Merkleizer.HashTreeRoot(SszContainer.GetContainer<BeaconBlockHeaderSSZ>(sizePreset), BeaconBlockHeaderSSZ.Serialize(bootstrap.Header.Beacon)));
            Debug.Assert(leaf.Equals(trustedBlockRoot));

            leaf = new Root(Merkleizer.HashTreeRoot(SszContainer.GetContainer<SyncCommitteeSSZ>(sizePreset), SyncCommitteeSSZ.Serialize(bootstrap.CurrentSyncCommittee)));
            result = Helpers.IsValidMerkleBranch(leaf,
                bootstrap.CurrentSyncCommitteeBranch,
                Helpers.FloorLog2(Constants.CURRENT_SYNC_COMMITTEE_INDEX),
                Helpers.GetSubtreeIndex(Constants.CURRENT_SYNC_COMMITTEE_INDEX),
                bootstrap.Header.Beacon.StateRoot);

            return new LightClientStore(bootstrap.Header,
                bootstrap.CurrentSyncCommittee,
                SyncCommittee.Zero,
                LightClientUpdate.Zero,
                bootstrap.Header,
                0,
                0);
        }
    }
}

