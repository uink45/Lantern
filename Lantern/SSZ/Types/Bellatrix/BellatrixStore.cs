using Lantern.Types.Basic;
using Lantern.Types.Containers;
namespace Lantern.SSZ.Types.Bellatrix;

public class BellatrixStore
{
        [SszElement(0, "Container")]
        public BellatrixHeader FinalizedHeader { get; set; }

        [SszElement(1, "Container")]
        public SyncCommitteeSSZ CurrentSyncCommittee { get; set; }

        [SszElement(2, "Container")]
        public SyncCommitteeSSZ NextSyncCommittee { get; set; }

        [SszElement(3, "Container")]
        public BellatrixUpdate BestValidUpdate { get; set; }

        [SszElement(4, "Container")]
        public BellatrixHeader OptimisticHeader { get; set; }

        [SszElement(5, "uint64")]
        public ulong PreviousMaxActiveParticipants { get; set; }

        [SszElement(6, "uint64")]
        public ulong CurrentMaxActiveParticipants { get; set; }

        public static BellatrixStore Serialize(LightClientStore store)
        {
            BellatrixStore storeSSZ = new BellatrixStore();
            storeSSZ.FinalizedHeader = BellatrixHeader.Serialize(store.FinalizedHeader);
            storeSSZ.CurrentSyncCommittee = SyncCommitteeSSZ.Serialize(store.CurrentSyncCommittee);
            storeSSZ.NextSyncCommittee = SyncCommitteeSSZ.Serialize(store.NextSyncCommittee);
            storeSSZ.BestValidUpdate = BellatrixUpdate.Serialize(store.BestValidUpdate);
            storeSSZ.OptimisticHeader = BellatrixHeader.Serialize(store.OptimisticHeader);
            storeSSZ.PreviousMaxActiveParticipants = store.PreviousMaxActiveParticipants;
            storeSSZ.CurrentMaxActiveParticipants = store.CurrentMaxActiveParticipants;

            return storeSSZ;
        }

        public static LightClientStore Deserialize(BellatrixStore storeSSZ)
        {
            return new LightClientStore(BellatrixHeader.Deserialize(storeSSZ.FinalizedHeader),
                SyncCommitteeSSZ.Deserialize(storeSSZ.CurrentSyncCommittee),
                SyncCommitteeSSZ.Deserialize(storeSSZ.NextSyncCommittee),
                BellatrixUpdate.Deserialize(storeSSZ.BestValidUpdate),
                BellatrixHeader.Deserialize(storeSSZ.OptimisticHeader),
                new Slot(storeSSZ.PreviousMaxActiveParticipants),
                new Slot(storeSSZ.CurrentMaxActiveParticipants));
        }
}