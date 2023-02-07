using Lantern.Types.Basic;
using Lantern.Types.Containers;
using Lantern.Types.Crypto;
using Lantern.Config;
using Lantern.Core;
namespace Lantern.SSZ.Types.Bellatrix;

public class BellatrixFinalityUpdate
{
    [SszElement(0, "Container")]
    public BellatrixHeader AttestedHeader { get; set; }

    [SszElement(1, "Container")]
    public BellatrixHeader FinalizedHeader { get; set; }

    [SszElement(2, "Vector[Vector[uint8, 32], FINALIZED_ROOT_DEPTH]")]
    public byte[][] FinalityBranch { get; set; }

    [SszElement(3, "Container")]
    public SyncAggregateSSZ SyncAggregate { get; set; }

    [SszElement(4, "uint64")]
    public ulong SignatureSlot { get; set; }

    public static BellatrixFinalityUpdate Serialize(LightClientUpdate update)
    {
        BellatrixFinalityUpdate updateSSZ = new BellatrixFinalityUpdate();
        updateSSZ.AttestedHeader = BellatrixHeader.Serialize(update.AttestedHeader);
        updateSSZ.FinalizedHeader = BellatrixHeader.Serialize(update.FinalizedHeader);
        updateSSZ.FinalityBranch = Enumerable.Range(0, Helpers.FloorLog2(Constants.FINALISED_ROOT_INDEX))
            .Select(i => update.FinalityBranch[i].AsSpan().ToArray())
            .ToArray();
        updateSSZ.SyncAggregate = SyncAggregateSSZ.Serialize(update.SyncAggregate);
        updateSSZ.SignatureSlot = update.SignatureSlot;

        return updateSSZ;
    }

    public static LightClientFinalityUpdate Deserialize(BellatrixFinalityUpdate updateSSZ)
    {
        return new LightClientFinalityUpdate(BellatrixHeader.Deserialize(updateSSZ.AttestedHeader),
            BellatrixHeader.Deserialize(updateSSZ.FinalizedHeader),
            Enumerable.Range(0, Helpers.FloorLog2(Constants.FINALISED_ROOT_INDEX))
                .Select(i => new Bytes32(updateSSZ.FinalityBranch[i]))
                .ToArray(),
            SyncAggregateSSZ.Deserialize(updateSSZ.SyncAggregate),
            new Slot(updateSSZ.SignatureSlot));
    }
}