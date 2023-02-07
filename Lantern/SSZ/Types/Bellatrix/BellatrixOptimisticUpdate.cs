using Lantern.Types.Basic;
using Lantern.Types.Containers;
namespace Lantern.SSZ.Types.Bellatrix;

public class BellatrixOptimisticUpdate
{
    [SszElement(0, "Container")]
    public BellatrixHeader AttestedHeader { get; set; }

    [SszElement(1, "Container")]
    public SyncAggregateSSZ SyncAggregate { get; set; }

    [SszElement(2, "uint64")]
    public ulong Slot { get; set; }

    public static BellatrixOptimisticUpdate Serialize(LightClientOptimisticUpdate update)
    {
        BellatrixOptimisticUpdate updateSSZ = new BellatrixOptimisticUpdate();
        updateSSZ.AttestedHeader = BellatrixHeader.Serialize(update.AttestedHeader);
        updateSSZ.SyncAggregate = SyncAggregateSSZ.Serialize(update.SyncAggregate);
        updateSSZ.Slot = update.SignatureSlot;

        return updateSSZ;
    }

    public static LightClientOptimisticUpdate Deserialize(BellatrixOptimisticUpdate updateSSZ)
    {
        return new LightClientOptimisticUpdate(BellatrixHeader.Deserialize(updateSSZ.AttestedHeader),
            SyncAggregateSSZ.Deserialize(updateSSZ.SyncAggregate),
            new Slot(updateSSZ.Slot));
    }
}