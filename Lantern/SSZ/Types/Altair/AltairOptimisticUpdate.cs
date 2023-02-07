using Lantern.Types.Basic;
using Lantern.Types.Containers;
namespace Lantern.SSZ.Types.Altair;

public class AltairOptimisticUpdate
{
    [SszElement(0, "Container")]
    public AltairHeader AttestedHeader { get; set; }

    [SszElement(1, "Container")]
    public SyncAggregateSSZ SyncAggregate { get; set; }

    [SszElement(2, "uint64")]
    public ulong Slot { get; set; }

    public static AltairOptimisticUpdate Serialize(LightClientOptimisticUpdate update)
    {
        AltairOptimisticUpdate updateSSZ = new AltairOptimisticUpdate();
        updateSSZ.AttestedHeader = AltairHeader.Serialize(update.AttestedHeader);
        updateSSZ.SyncAggregate = SyncAggregateSSZ.Serialize(update.SyncAggregate);
        updateSSZ.Slot = update.SignatureSlot;

        return updateSSZ;
    }

    public static LightClientOptimisticUpdate Deserialize(AltairOptimisticUpdate updateSSZ)
    {
        return new LightClientOptimisticUpdate(AltairHeader.Deserialize(updateSSZ.AttestedHeader),
            SyncAggregateSSZ.Deserialize(updateSSZ.SyncAggregate),
            new Slot(updateSSZ.Slot));
    }
}