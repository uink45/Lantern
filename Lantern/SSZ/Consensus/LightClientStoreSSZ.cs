using System;
using Lantern.Core;
using Lantern.Types.Basic;
using Lantern.Types.Containers;
using Lantern.Types.Crypto;
using Lantern.Config;

namespace Lantern.SSZ.Consensus
{
	public class LightClientStoreSSZ
	{
        [SszElement(0, "Container")]
        public LightClientHeaderSSZ FinalizedHeader { get; set; }

        [SszElement(1, "Container")]
        public SyncCommitteeSSZ CurrentSyncCommittee { get; set; }

        [SszElement(2, "Container")]
        public SyncCommitteeSSZ NextSyncCommittee { get; set; }

        [SszElement(3, "Container")]
        public LightClientUpdateSSZ BestValidUpdate { get; set; }

        [SszElement(4, "Container")]
        public LightClientHeaderSSZ OptimisticHeader { get; set; }

        [SszElement(5, "uint64")]
        public ulong PreviousMaxActiveParticipants { get; set; }

        [SszElement(6, "uint64")]
        public ulong CurrentMaxActiveParticipants { get; set; }

        public static LightClientStoreSSZ Serialize(LightClientStore store)
        {
            LightClientStoreSSZ storeSSZ = new LightClientStoreSSZ();
            storeSSZ.FinalizedHeader = LightClientHeaderSSZ.Serialize(store.FinalizedHeader);
            storeSSZ.CurrentSyncCommittee = SyncCommitteeSSZ.Serialize(store.CurrentSyncCommittee);
            storeSSZ.NextSyncCommittee = SyncCommitteeSSZ.Serialize(store.NextSyncCommittee);
            storeSSZ.BestValidUpdate = LightClientUpdateSSZ.Serialize(store.BestValidUpdate);
            storeSSZ.OptimisticHeader = LightClientHeaderSSZ.Serialize(store.OptimisticHeader);
            storeSSZ.PreviousMaxActiveParticipants = store.PreviousMaxActiveParticipants;
            storeSSZ.CurrentMaxActiveParticipants = store.CurrentMaxActiveParticipants;

            return storeSSZ;
        }

        public static LightClientStore Deserialize(LightClientStoreSSZ storeSSZ)
        {
            return new LightClientStore(LightClientHeaderSSZ.Deserialize(storeSSZ.FinalizedHeader),
                SyncCommitteeSSZ.Deserialize(storeSSZ.CurrentSyncCommittee),
                SyncCommitteeSSZ.Deserialize(storeSSZ.NextSyncCommittee),
                LightClientUpdateSSZ.Deserialize(storeSSZ.BestValidUpdate),
                LightClientHeaderSSZ.Deserialize(storeSSZ.OptimisticHeader),
                new Slot(storeSSZ.PreviousMaxActiveParticipants),
                new Slot(storeSSZ.CurrentMaxActiveParticipants));
        }
    }
}

