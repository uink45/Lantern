using Lantern.Types.Containers;
using Lantern.Types.Crypto;
using Lantern.Config;
using Lantern.Core;
namespace Lantern.SSZ.Types.Capella;

public class CapellaHeader
{
    [SszElement(0, "Container")]
    public BeaconBlockHeaderSSZ Beacon { get; set; }

    [SszElement(1, "Container")]
    public CapellaExecutionPayloadHeader Execution { get; set; }

    [SszElement(2, "Vector[Vector[uint8, 32], EXECUTION_PAYLOAD_DEPTH]")]
    public byte[][] ExecutionBranch { get; set; }

    public static CapellaHeader Serialize(LightClientHeader header)
    {
        CapellaHeader headerSSZ = new CapellaHeader();
        headerSSZ.Beacon = BeaconBlockHeaderSSZ.Serialize(header.Beacon);
        headerSSZ.Execution = CapellaExecutionPayloadHeader.Serialize(header.Execution);
        headerSSZ.ExecutionBranch = Enumerable.Range(0, Helpers.FloorLog2(Constants.EXECUTION_PAYLOAD_INDEX))
            .Select(i => header.ExecutionBranch[i].AsSpan().ToArray())
            .ToArray();
        return headerSSZ;
    }

    public static LightClientHeader Deserialize(CapellaHeader headerSSZ)
    {
        return new LightClientHeader(BeaconBlockHeaderSSZ.Deserialize(headerSSZ.Beacon),
            CapellaExecutionPayloadHeader.Deserialize(headerSSZ.Execution),
            Enumerable.Range(0, Helpers.FloorLog2(Constants.EXECUTION_PAYLOAD_INDEX))
                .Select(i => new Bytes32(headerSSZ.ExecutionBranch[i].AsSpan().ToArray()))
                .ToArray());
    }
}