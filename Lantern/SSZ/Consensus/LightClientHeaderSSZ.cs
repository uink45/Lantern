using System;
using Lantern.SSZ;
using Lantern.SSZ.Consensus;
using Lantern.Types.Basic;
using Lantern.Types.Containers;
using Lantern.Types.Crypto;
using Lantern.Config;
using Lantern.Core;
using System.Reflection.PortableExecutable;

namespace Lantern.SSZ.Consensus
{
	public class LightClientHeaderSSZ
	{
		[SszElement(0, "Container")]
		public BeaconBlockHeaderSSZ Beacon { get; set; }

        [SszElement(1, "Container")]
		public ExecutionPayloadHeaderSSZ Execution { get; set; }

        [SszElement(2, "Vector[Vector[uint8, 32], EXECUTION_PAYLOAD_DEPTH]")]
        public byte[][] ExecutionBranch { get; set; }

        public static LightClientHeaderSSZ Serialize(LightClientHeader header)
        {
            LightClientHeaderSSZ headerSSZ = new LightClientHeaderSSZ();
            headerSSZ.Beacon = BeaconBlockHeaderSSZ.Serialize(header.Beacon);
            headerSSZ.Execution = ExecutionPayloadHeaderSSZ.Serialize(header.Execution);
            headerSSZ.ExecutionBranch = Enumerable.Range(0, Helpers.FloorLog2(Constants.EXECUTION_PAYLOAD_INDEX))
                                    .Select(i => header.ExecutionBranch[i].AsSpan().ToArray())
                                    .ToArray();
            return headerSSZ;
        }

        public static LightClientHeader Deserialize(LightClientHeaderSSZ headerSSZ)
        {
            return new LightClientHeader(BeaconBlockHeaderSSZ.Deserialize(headerSSZ.Beacon),
                ExecutionPayloadHeaderSSZ.Deserialize(headerSSZ.Execution),
                Enumerable.Range(0, Helpers.FloorLog2(Constants.EXECUTION_PAYLOAD_INDEX))
                                    .Select(i => new Bytes32(headerSSZ.ExecutionBranch[i].AsSpan().ToArray()))
                                    .ToArray());
        }
    }
}