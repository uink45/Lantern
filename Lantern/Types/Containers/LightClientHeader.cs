using Lantern.Config;
using Lantern.Core;
using Lantern.Types.Crypto;

namespace Lantern.Types.Containers;

public class LightClientHeader : IEquatable<LightClientHeader>
{
    public static readonly LightClientHeader Zero = new(BeaconBlockHeader.Zero,
        ExecutionPayloadHeader.Zero,
        Enumerable.Range(0, Helpers.FloorLog2(Constants.EXECUTION_PAYLOAD_INDEX))
            .Select(i => new Bytes32())
            .ToArray());

    public readonly BeaconBlockHeader Beacon;
    public readonly ExecutionPayloadHeader Execution;
    public readonly Bytes32[] ExecutionBranch;

    public LightClientHeader(BeaconBlockHeader beacon)
    {
        Beacon = beacon;
        Execution = ExecutionPayloadHeader.Zero;
        ExecutionBranch = Enumerable.Range(0, Helpers.FloorLog2(Constants.EXECUTION_PAYLOAD_INDEX))
            .Select(i => new Bytes32())
            .ToArray();
    }

    public LightClientHeader(BeaconBlockHeader beacon, ExecutionPayloadHeader execution, Bytes32[] executionBranch)
    {
        Beacon = beacon;
        Execution = execution;
        ExecutionBranch = executionBranch;
    }

    public bool Equals(LightClientHeader? other)
    {
        if (ReferenceEquals(other, null)) return false;

        return ReferenceEquals(other, this) || Beacon.Equals(other.Beacon);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Beacon);
    }

    public override bool Equals(object? obj)
    {
        var other = obj as LightClientHeader;
        return other is not null && Equals(other);
    }
}