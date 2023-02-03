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
	public class LightClientOptimisticUpdateSSZ
	{
        [SszElement(0, "Container")]
        public LightClientHeaderSSZ AttestedHeader { get; set; }

        [SszElement(1, "Container")]
        public SyncAggregateSSZ SyncAggregate { get; set; }

        [SszElement(2, "uint64")]
        public ulong Slot { get; set; }

        public static LightClientOptimisticUpdateSSZ Serialize(LightClientOptimisticUpdate update)
        {
            LightClientOptimisticUpdateSSZ updateSSZ = new LightClientOptimisticUpdateSSZ();
            updateSSZ.AttestedHeader = LightClientHeaderSSZ.Serialize(update.AttestedHeader);
            updateSSZ.SyncAggregate = SyncAggregateSSZ.Serialize(update.SyncAggregate);
            updateSSZ.Slot = update.SignatureSlot;

            return updateSSZ;
        }

        public static LightClientOptimisticUpdate Deserialize(LightClientOptimisticUpdateSSZ updateSSZ)
        {
            return new LightClientOptimisticUpdate(LightClientHeaderSSZ.Deserialize(updateSSZ.AttestedHeader),
                SyncAggregateSSZ.Deserialize(updateSSZ.SyncAggregate),
                new Slot(updateSSZ.Slot));
        }
    }
}

