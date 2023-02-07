using Lantern.Types.Containers;
using Lantern.Types.Crypto;
using Lantern.Config;
using Lantern.Core;
namespace Lantern.SSZ.Types.Deneb;

public class DenebHeader
{
    [SszElement(0, "Container")]
    public BeaconBlockHeaderSSZ Beacon { get; set; }

    [SszElement(1, "Container")]
    public DenebExecutionPayloadHeader Execution { get; set; }

    [SszElement(2, "Vector[Vector[uint8, 32], EXECUTION_PAYLOAD_DEPTH]")]
    public byte[][] ExecutionBranch { get; set; }

    public static DenebHeader Serialize(LightClientHeader header)
    {
        DenebHeader headerSSZ = new DenebHeader
        {
            Beacon = BeaconBlockHeaderSSZ.Serialize(header.Beacon),
            Execution = DenebExecutionPayloadHeader.Serialize(header.Execution),
            ExecutionBranch = Enumerable.Range(0, Helpers.FloorLog2(Constants.EXECUTION_PAYLOAD_INDEX))
                .Select(i => header.ExecutionBranch[i].AsSpan().ToArray())
                .ToArray()
        };
        return headerSSZ;
    }

    public static LightClientHeader Deserialize(DenebHeader headerSsz)
    {
        return new LightClientHeader(BeaconBlockHeaderSSZ.Deserialize(headerSsz.Beacon),
            DenebExecutionPayloadHeader.Deserialize(headerSsz.Execution),
            Enumerable.Range(0, Helpers.FloorLog2(Constants.EXECUTION_PAYLOAD_INDEX))
                .Select(i => new Bytes32(headerSsz.ExecutionBranch[i].AsSpan().ToArray()))
                .ToArray());
    }
}