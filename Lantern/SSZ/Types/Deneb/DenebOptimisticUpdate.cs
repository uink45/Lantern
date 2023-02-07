using Lantern.Types.Basic;
using Lantern.Types.Containers;
namespace Lantern.SSZ.Types.Deneb;

public class DenebOptimisticUpdate
{
    [SszElement(0, "Container")]
    public DenebHeader AttestedHeader { get; set; }

    [SszElement(1, "Container")]
    public SyncAggregateSSZ SyncAggregate { get; set; }

    [SszElement(2, "uint64")]
    public ulong Slot { get; set; }

    public static DenebOptimisticUpdate Serialize(LightClientOptimisticUpdate update)
    {
        DenebOptimisticUpdate updateSSZ = new DenebOptimisticUpdate();
        updateSSZ.AttestedHeader = DenebHeader.Serialize(update.AttestedHeader);
        updateSSZ.SyncAggregate = SyncAggregateSSZ.Serialize(update.SyncAggregate);
        updateSSZ.Slot = update.SignatureSlot;

        return updateSSZ;
    }

    public static LightClientOptimisticUpdate Deserialize(DenebOptimisticUpdate updateSSZ)
    {
        return new LightClientOptimisticUpdate(DenebHeader.Deserialize(updateSSZ.AttestedHeader),
            SyncAggregateSSZ.Deserialize(updateSSZ.SyncAggregate),
            new Slot(updateSSZ.Slot));
    }
}