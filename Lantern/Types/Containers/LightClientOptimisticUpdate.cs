using System;
using Lantern.Types.Containers;
using Lantern.Types.Basic;

namespace Lantern.Types.Containers
{
	public class LightClientOptimisticUpdate
	{
        public readonly LightClientHeader AttestedHeader;
		public readonly SyncAggregate SyncAggregate;
		public readonly Slot SignatureSlot;

		public static readonly LightClientOptimisticUpdate Zero =
			new LightClientOptimisticUpdate(LightClientHeader.Zero, SyncAggregate.Zero, Slot.Zero);

        public LightClientOptimisticUpdate(LightClientHeader attestedHeader, SyncAggregate syncAggregate, Slot signatureSlot)
		{
			AttestedHeader = attestedHeader;
			SyncAggregate = syncAggregate;
			SignatureSlot = signatureSlot;
		}
	}
}

