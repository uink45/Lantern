using System;
using Lantern.SSZ;
using Lantern.SSZ.Consensus;
using Lantern.Types.Basic;
using Lantern.Types.Containers;
using Lantern.Types.Crypto;
using Lantern.Config;
using Lantern.Core;

namespace Lantern.SSZ.Consensus
{
	public class LightClientFinalityUpdateSSZ
	{
        [SszElement(0, "Container")]
        public LightClientHeaderSSZ AttestedHeader { get; set; }

        [SszElement(1, "Container")]
        public LightClientHeaderSSZ FinalizedHeader { get; set; }

        [SszElement(2, "Vector[Vector[uint8, 32], FINALIZED_ROOT_DEPTH]")]
        public byte[][] FinalityBranch { get; set; }

        [SszElement(3, "Container")]
        public SyncAggregateSSZ SyncAggregate { get; set; }

        [SszElement(4, "uint64")]
        public ulong SignatureSlot { get; set; }

        public static LightClientFinalityUpdateSSZ Serialize(LightClientFinalityUpdate update)
        {
            LightClientFinalityUpdateSSZ updateSSZ = new LightClientFinalityUpdateSSZ();
            updateSSZ.AttestedHeader = LightClientHeaderSSZ.Serialize(update.AttestedHeader);
            updateSSZ.FinalizedHeader = LightClientHeaderSSZ.Serialize(update.FinalizedHeader);
            updateSSZ.FinalityBranch = Enumerable.Range(0, Helpers.FloorLog2(Constants.FINALISED_ROOT_INDEX))
                                    .Select(i => update.FinalityBranch[i].AsSpan().ToArray())
                                    .ToArray();
            updateSSZ.SyncAggregate = SyncAggregateSSZ.Serialize(update.SyncAggregate);
            updateSSZ.SignatureSlot = update.SignatureSlot;

            return updateSSZ;
        }

        public static LightClientFinalityUpdate Deserialize(LightClientFinalityUpdateSSZ updateSSZ)
        {
            return new LightClientFinalityUpdate(LightClientHeaderSSZ.Deserialize(updateSSZ.AttestedHeader),
                LightClientHeaderSSZ.Deserialize(updateSSZ.FinalizedHeader),
                Enumerable.Range(0, Helpers.FloorLog2(Constants.FINALISED_ROOT_INDEX))
                                        .Select(i => new Bytes32(updateSSZ.FinalityBranch[i]))
                                        .ToArray(),
                SyncAggregateSSZ.Deserialize(updateSSZ.SyncAggregate),
                new Slot(updateSSZ.SignatureSlot));
        }
    }
}

