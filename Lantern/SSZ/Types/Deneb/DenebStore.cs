using Lantern.Types.Basic;
using Lantern.Types.Containers;
namespace Lantern.SSZ.Types.Deneb;

public class DenebStore
{
    [SszElement(0, "Container")]
        public DenebHeader FinalizedHeader { get; set; }

        [SszElement(1, "Container")]
        public SyncCommitteeSSZ CurrentSyncCommittee { get; set; }

        [SszElement(2, "Container")]
        public SyncCommitteeSSZ NextSyncCommittee { get; set; }

        [SszElement(3, "Container")]
        public DenebUpdate BestValidUpdate { get; set; }

        [SszElement(4, "Container")]
        public DenebHeader OptimisticHeader { get; set; }

        [SszElement(5, "uint64")]
        public ulong PreviousMaxActiveParticipants { get; set; }

        [SszElement(6, "uint64")]
        public ulong CurrentMaxActiveParticipants { get; set; }

        public static DenebStore Serialize(LightClientStore store)
        {
            DenebStore storeSSZ = new DenebStore();
            storeSSZ.FinalizedHeader = DenebHeader.Serialize(store.FinalizedHeader);
            storeSSZ.CurrentSyncCommittee = SyncCommitteeSSZ.Serialize(store.CurrentSyncCommittee);
            storeSSZ.NextSyncCommittee = SyncCommitteeSSZ.Serialize(store.NextSyncCommittee);
            storeSSZ.BestValidUpdate = DenebUpdate.Serialize(store.BestValidUpdate);
            storeSSZ.OptimisticHeader = DenebHeader.Serialize(store.OptimisticHeader);
            storeSSZ.PreviousMaxActiveParticipants = store.PreviousMaxActiveParticipants;
            storeSSZ.CurrentMaxActiveParticipants = store.CurrentMaxActiveParticipants;

            return storeSSZ;
        }

        public static LightClientStore Deserialize(DenebStore storeSSZ)
        {
            return new LightClientStore(DenebHeader.Deserialize(storeSSZ.FinalizedHeader),
                SyncCommitteeSSZ.Deserialize(storeSSZ.CurrentSyncCommittee),
                SyncCommitteeSSZ.Deserialize(storeSSZ.NextSyncCommittee),
                DenebUpdate.Deserialize(storeSSZ.BestValidUpdate),
                DenebHeader.Deserialize(storeSSZ.OptimisticHeader),
                new Slot(storeSSZ.PreviousMaxActiveParticipants),
                new Slot(storeSSZ.CurrentMaxActiveParticipants));
        }
}