using Lantern.Types.Basic;
using Lantern.Types.Containers;
namespace Lantern.SSZ.Types.Capella;

public class CapellaStore
{
    [SszElement(0, "Container")]
        public CapellaHeader FinalizedHeader { get; set; }

        [SszElement(1, "Container")]
        public SyncCommitteeSSZ CurrentSyncCommittee { get; set; }

        [SszElement(2, "Container")]
        public SyncCommitteeSSZ NextSyncCommittee { get; set; }

        [SszElement(3, "Container")]
        public CapellaUpdate BestValidUpdate { get; set; }

        [SszElement(4, "Container")]
        public CapellaHeader OptimisticHeader { get; set; }

        [SszElement(5, "uint64")]
        public ulong PreviousMaxActiveParticipants { get; set; }

        [SszElement(6, "uint64")]
        public ulong CurrentMaxActiveParticipants { get; set; }

        public static CapellaStore Serialize(LightClientStore store)
        {
            CapellaStore storeSSZ = new CapellaStore();
            storeSSZ.FinalizedHeader = CapellaHeader.Serialize(store.FinalizedHeader);
            storeSSZ.CurrentSyncCommittee = SyncCommitteeSSZ.Serialize(store.CurrentSyncCommittee);
            storeSSZ.NextSyncCommittee = SyncCommitteeSSZ.Serialize(store.NextSyncCommittee);
            storeSSZ.BestValidUpdate = CapellaUpdate.Serialize(store.BestValidUpdate);
            storeSSZ.OptimisticHeader = CapellaHeader.Serialize(store.OptimisticHeader);
            storeSSZ.PreviousMaxActiveParticipants = store.PreviousMaxActiveParticipants;
            storeSSZ.CurrentMaxActiveParticipants = store.CurrentMaxActiveParticipants;

            return storeSSZ;
        }

        public static LightClientStore Deserialize(CapellaStore storeSSZ)
        {
            return new LightClientStore(CapellaHeader.Deserialize(storeSSZ.FinalizedHeader),
                SyncCommitteeSSZ.Deserialize(storeSSZ.CurrentSyncCommittee),
                SyncCommitteeSSZ.Deserialize(storeSSZ.NextSyncCommittee),
                CapellaUpdate.Deserialize(storeSSZ.BestValidUpdate),
                CapellaHeader.Deserialize(storeSSZ.OptimisticHeader),
                new Slot(storeSSZ.PreviousMaxActiveParticipants),
                new Slot(storeSSZ.CurrentMaxActiveParticipants));
        }
}