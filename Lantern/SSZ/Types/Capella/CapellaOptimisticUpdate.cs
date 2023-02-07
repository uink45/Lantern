using Lantern.Types.Basic;
using Lantern.Types.Containers;
namespace Lantern.SSZ.Types.Capella;

public class CapellaOptimisticUpdate
{
    [SszElement(0, "Container")]
    public CapellaHeader AttestedHeader { get; set; }

    [SszElement(1, "Container")]
    public SyncAggregateSSZ SyncAggregate { get; set; }

    [SszElement(2, "uint64")]
    public ulong Slot { get; set; }

    public static CapellaOptimisticUpdate Serialize(LightClientOptimisticUpdate update)
    {
        CapellaOptimisticUpdate updateSSZ = new CapellaOptimisticUpdate();
        updateSSZ.AttestedHeader = CapellaHeader.Serialize(update.AttestedHeader);
        updateSSZ.SyncAggregate = SyncAggregateSSZ.Serialize(update.SyncAggregate);
        updateSSZ.Slot = update.SignatureSlot;

        return updateSSZ;
    }

    public static LightClientOptimisticUpdate Deserialize(CapellaOptimisticUpdate updateSSZ)
    {
        return new LightClientOptimisticUpdate(CapellaHeader.Deserialize(updateSSZ.AttestedHeader),
            SyncAggregateSSZ.Deserialize(updateSSZ.SyncAggregate),
            new Slot(updateSSZ.Slot));
    }
}