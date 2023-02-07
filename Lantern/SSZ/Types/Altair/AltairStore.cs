using Lantern.Types.Basic;
using Lantern.Types.Containers;
namespace Lantern.SSZ.Types.Altair;

public class AltairStore
{
        [SszElement(0, "Container")]
        public AltairHeader FinalizedHeader { get; set; }

        [SszElement(1, "Container")]
        public SyncCommitteeSSZ CurrentSyncCommittee { get; set; }

        [SszElement(2, "Container")]
        public SyncCommitteeSSZ NextSyncCommittee { get; set; }

        [SszElement(3, "Container")]
        public AltairUpdate BestValidUpdate { get; set; }

        [SszElement(4, "Container")]
        public AltairHeader OptimisticHeader { get; set; }

        [SszElement(5, "uint64")]
        public ulong PreviousMaxActiveParticipants { get; set; }

        [SszElement(6, "uint64")]
        public ulong CurrentMaxActiveParticipants { get; set; }

        public static AltairStore Serialize(LightClientStore store)
        {
            AltairStore storeSSZ = new AltairStore();
            storeSSZ.FinalizedHeader = AltairHeader.Serialize(store.FinalizedHeader);
            storeSSZ.CurrentSyncCommittee = SyncCommitteeSSZ.Serialize(store.CurrentSyncCommittee);
            storeSSZ.NextSyncCommittee = SyncCommitteeSSZ.Serialize(store.NextSyncCommittee);
            storeSSZ.BestValidUpdate = AltairUpdate.Serialize(store.BestValidUpdate);
            storeSSZ.OptimisticHeader = AltairHeader.Serialize(store.OptimisticHeader);
            storeSSZ.PreviousMaxActiveParticipants = store.PreviousMaxActiveParticipants;
            storeSSZ.CurrentMaxActiveParticipants = store.CurrentMaxActiveParticipants;

            return storeSSZ;
        }

        public static LightClientStore Deserialize(AltairStore storeSSZ)
        {
            return new LightClientStore(AltairHeader.Deserialize(storeSSZ.FinalizedHeader),
                SyncCommitteeSSZ.Deserialize(storeSSZ.CurrentSyncCommittee),
                SyncCommitteeSSZ.Deserialize(storeSSZ.NextSyncCommittee),
                AltairUpdate.Deserialize(storeSSZ.BestValidUpdate),
                AltairHeader.Deserialize(storeSSZ.OptimisticHeader),
                new Slot(storeSSZ.PreviousMaxActiveParticipants),
                new Slot(storeSSZ.CurrentMaxActiveParticipants));
        }
}