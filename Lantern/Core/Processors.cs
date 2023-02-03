using System;
using System.Linq;
using Lantern.Types.Containers;
using Lantern.Types.Basic;
using Lantern.Types.Crypto;
using Lantern.Types.Crypto.BLS;
using Lantern.Config;
using System.Diagnostics;
using Lantern.SSZ;
using Lantern.SSZ.Consensus;
using System.Collections;

namespace Lantern.Core
{
	public static class Processors
	{
        public static void ValidateLightClientUpdate(LightClientStore store, LightClientUpdate update, Slot currentSlot, Root genesisValidatorsRoot, BLSUtility bLS, SizePreset sszPreset)
        {
            SyncAggregate syncAggregate = update.SyncAggregate;
            Debug.Assert(Helpers.Sum(syncAggregate.SyncCommitteeBits) >= Constants.MIN_SYNC_COMMITTEE_PARTICIPANTS);

            bool result = Helpers.IsValidLightClientHeader(update.AttestedHeader);
            Debug.Assert(result == true);

            Slot updateAttesedSlot = update.AttestedHeader.Beacon.Slot;
            Slot updateFinalizedSlot = update.FinalizedHeader.Beacon.Slot;
            Debug.Assert(currentSlot >= update.SignatureSlot && update.SignatureSlot > updateAttesedSlot && updateAttesedSlot >= updateFinalizedSlot);

            ulong storePeriod = Helpers.ComputeSyncCommitteePeriodAtSlot(store.FinalizedHeader.Beacon.Slot);
            ulong updateSignaturePeriod = Helpers.ComputeSyncCommitteePeriodAtSlot(update.SignatureSlot);

            if (Helpers.IsNextSyncCommitteeKnown(store))
            {
                Debug.Assert(updateSignaturePeriod == storePeriod || updateSignaturePeriod == storePeriod + 1);
            }
            else
            {
                Debug.Assert(updateSignaturePeriod == storePeriod);
            }

            ulong updateAttestedPeriod = Helpers.ComputeSyncCommitteePeriodAtSlot(updateAttesedSlot);
            bool updateHasNextSyncCommittee = !Helpers.IsNextSyncCommitteeKnown(store) && (Helpers.IsSyncCommitteeUpdate(update) && updateAttestedPeriod == storePeriod);
            Debug.Assert(updateAttesedSlot > store.FinalizedHeader.Beacon.Slot || updateHasNextSyncCommittee);

            if (!Helpers.IsFinalityUpdate(update))
            {
                Debug.Assert(update.FinalizedHeader.Equals(LightClientHeader.Zero));
            }
            else
            {
                Root finalizedRoot;

                if(updateFinalizedSlot == Constants.GENESIS_SlOT)
                {
                    Debug.Assert(update.NextSyncCommittee.Equals(store.NextSyncCommittee));
                    finalizedRoot = Root.Zero;
                }
                else
                {
                    result = Helpers.IsValidLightClientHeader(update.FinalizedHeader);
                    Debug.Assert(result);

                    finalizedRoot = new Root(Merkleizer.HashTreeRoot(SszContainer.GetContainer<BeaconBlockHeaderSSZ>(sszPreset), BeaconBlockHeaderSSZ.Serialize(update.FinalizedHeader.Beacon)));
                }

                result = Helpers.IsValidMerkleBranch(finalizedRoot,
                    update.FinalityBranch,
                    Helpers.FloorLog2(Constants.FINALISED_ROOT_INDEX),
                    Helpers.GetSubtreeIndex(Constants.FINALISED_ROOT_INDEX),
                    update.AttestedHeader.Beacon.StateRoot);
                Debug.Assert(result);
            }
            
            if (!Helpers.IsSyncCommitteeUpdate(update))
            {
                Debug.Assert(update.NextSyncCommittee.Equals(SyncCommittee.Zero));
            }
            else
            {
                if (updateAttestedPeriod == storePeriod && Helpers.IsNextSyncCommitteeKnown(store))
                {
                    Debug.Assert(update.NextSyncCommittee.Equals(store.NextSyncCommittee));
                }

                Root leaf = new Root(Merkleizer.HashTreeRoot(SszContainer.GetContainer<SyncCommitteeSSZ>(sszPreset), SyncCommitteeSSZ.Serialize(update.NextSyncCommittee)));
                result = Helpers.IsValidMerkleBranch(leaf,
                    update.NextSyncCommitteeBranch,
                    Helpers.FloorLog2(Constants.NEXT_SYNC_COMMITTEE_INDEX),
                    Helpers.GetSubtreeIndex(Constants.NEXT_SYNC_COMMITTEE_INDEX),
                    update.AttestedHeader.Beacon.StateRoot);
                Debug.Assert(result);
            }

            SyncCommittee syncCommittee;

            if(updateSignaturePeriod == storePeriod)
            {
                syncCommittee = store.CurrentSyncCommittee;
            }
            else
            {
                syncCommittee = store.NextSyncCommittee;                
            }

            BlsPublicKey[] participantPubkeys = new BlsPublicKey[Helpers.Sum(syncAggregate.SyncCommitteeBits)];
            int count = 0;
            for (int i = 0; i < syncAggregate.SyncCommitteeBits.Length; i++)
            {
                if (syncAggregate.SyncCommitteeBits[i])
                {
                    participantPubkeys[count] = syncCommittee.PublicKeys[i];
                    count++;
                }
            }

            ForkVersion forkVersion = Helpers.ComputeForkVersion(Helpers.ComputeEpochAtSlot(update.SignatureSlot));
            Domain domain = Helpers.ComputeDomain(Constants.DOMAIN_SYNC_COMMITTEE, forkVersion, genesisValidatorsRoot);
            Root signingRoot = Helpers.ComputeSigningRoot<BeaconBlockHeaderSSZ>(BeaconBlockHeaderSSZ.Serialize(update.AttestedHeader.Beacon), domain, sszPreset);
            result = bLS.BlsFastAggregateVerify(participantPubkeys, signingRoot, syncAggregate.SyncCommitteeSignature);
            Debug.Assert(result);
        }

        public static void ApplyLightClientUpdate(LightClientStore store, LightClientUpdate update)
        {
            ulong storePeriod = Helpers.ComputeSyncCommitteePeriodAtSlot(store.FinalizedHeader.Beacon.Slot);
            ulong updateFinalizedPeriod = Helpers.ComputeSyncCommitteePeriodAtSlot(update.FinalizedHeader.Beacon.Slot);

            if (!Helpers.IsNextSyncCommitteeKnown(store))
            {                
                Debug.Assert(updateFinalizedPeriod == storePeriod);
                store.SetNextSyncCommittee(update.NextSyncCommittee);
            }
            else if (updateFinalizedPeriod == storePeriod + 1)
            {
                store.SetCurrentSyncCommittee(store.NextSyncCommittee);
                store.SetNextSyncCommittee(update.NextSyncCommittee);
                store.SetPreviousMaxActiveParticipants(store.CurrentMaxActiveParticipants);
                store.SetCurrentMaxActiveParticipants(0);
            }

            if (update.FinalizedHeader.Beacon.Slot > store.FinalizedHeader.Beacon.Slot)
            {
                store.SetFinalizedHeader(update.FinalizedHeader);

                if (store.FinalizedHeader.Beacon.Slot > store.OptimisticHeader.Beacon.Slot)
                {
                    store.SetOptimisticHeader(store.FinalizedHeader);
                }
            }
        }

        public static void ProcessLightClientStoreForceUpdate(LightClientStore store, Slot currentSlot)
        {
            if (currentSlot > (store.FinalizedHeader.Beacon.Slot + (Slot)Preset.UPDATE_TIMEOUT) && !store.BestValidUpdate.Equals(LightClientUpdate.Zero))
            {
                if(store.BestValidUpdate.FinalizedHeader.Beacon.Slot <= store.FinalizedHeader.Beacon.Slot)
                {
                    LightClientUpdate update = new LightClientUpdate(store.BestValidUpdate.AttestedHeader,
                                               store.BestValidUpdate.NextSyncCommittee,
                                               store.BestValidUpdate.NextSyncCommitteeBranch,
                                               store.BestValidUpdate.AttestedHeader,
                                               store.BestValidUpdate.FinalityBranch,
                                               store.BestValidUpdate.SyncAggregate,
                                               store.BestValidUpdate.SignatureSlot);

                    store.SetBestValidUpdate(update);
                }
                ApplyLightClientUpdate(store, store.BestValidUpdate);
                store.SetBestValidUpdate(LightClientUpdate.Zero);
            }
        }

        public static void ProcessLightClientUpdate(LightClientStore store, LightClientUpdate update, Slot currentSlot, Root genesisValidatorsRoot, BLSUtility bLS, SizePreset sszPreset)
        {
            ValidateLightClientUpdate(store, update, currentSlot, genesisValidatorsRoot, bLS, sszPreset);
            BitArray syncCommitteeBits = update.SyncAggregate.SyncCommitteeBits;

            if(store.BestValidUpdate.Equals(LightClientUpdate.Zero) || Helpers.IsBetterUpdate(update, store.BestValidUpdate))
            {
                store.SetBestValidUpdate(update);
            }

            store.SetCurrentMaxActiveParticipants(Math.Max(store.CurrentMaxActiveParticipants, (ulong)Helpers.Sum(syncCommitteeBits)));

            if(Helpers.Sum(syncCommitteeBits) > Helpers.GetSafetyThreshold(store) && update.AttestedHeader.Beacon.Slot > store.OptimisticHeader.Beacon.Slot)
            {
                store.SetOptimisticHeader(update.AttestedHeader);
            }

            int updateHasFinalizedNextSyncCommittee = Convert.ToInt32(!Helpers.IsNextSyncCommitteeKnown(store)
                && Helpers.IsSyncCommitteeUpdate(update) && Helpers.IsFinalityUpdate(update)
                && (Helpers.ComputeSyncCommitteePeriodAtSlot(update.FinalizedHeader.Beacon.Slot)
                == Helpers.ComputeSyncCommitteePeriodAtSlot(update.AttestedHeader.Beacon.Slot)));

            if(Helpers.Sum(syncCommitteeBits) * 3 >= syncCommitteeBits.Length * 2
                && (update.FinalizedHeader.Beacon.Slot > store.FinalizedHeader.Beacon.Slot
                || Convert.ToBoolean(updateHasFinalizedNextSyncCommittee)))
            {
                ApplyLightClientUpdate(store, update);
                store.SetBestValidUpdate(LightClientUpdate.Zero);
            }
        }

        public static void ProcessLightClientFinalityUpdate(LightClientStore store, LightClientFinalityUpdate finalityUpdate, Slot currentSlot, Root genesisValidatorsRoot, BLSUtility bLS, SizePreset sszPreset)
        {
            LightClientUpdate update = new LightClientUpdate(finalityUpdate.AttestedHeader,
                                       SyncCommittee.Zero,
                                       Enumerable.Range(0, Helpers.FloorLog2(Constants.FINALISED_ROOT_INDEX)).Select(i => new Bytes32()).ToArray(),
                                       finalityUpdate.FinalizedHeader,
                                       finalityUpdate.FinalityBranch,
                                       finalityUpdate.SyncAggregate,
                                       finalityUpdate.SignatureSlot);

            ProcessLightClientUpdate(store, update, currentSlot, genesisValidatorsRoot, bLS, sszPreset);
        }

        public static void ProcessLightClientOptimisticUpdate(LightClientStore store, LightClientOptimisticUpdate optimisticUpdate, Slot currentSlot, Root genesisValidatorsRoot, BLSUtility bLS, SizePreset sszPreset)
        {
            LightClientUpdate update = new LightClientUpdate(optimisticUpdate.AttestedHeader,
                                       SyncCommittee.Zero,
                                       Enumerable.Range(0, Helpers.FloorLog2(Constants.NEXT_SYNC_COMMITTEE_INDEX)).Select(i => new Bytes32()).ToArray(),
                                       LightClientHeader.Zero,
                                       Enumerable.Range(0, Helpers.FloorLog2(Constants.FINALISED_ROOT_INDEX)).Select(i => new Bytes32()).ToArray(),
                                       optimisticUpdate.SyncAggregate,
                                       optimisticUpdate.SignatureSlot);

            ProcessLightClientUpdate(store, update, currentSlot, genesisValidatorsRoot, bLS, sszPreset);
        }
    }
}

