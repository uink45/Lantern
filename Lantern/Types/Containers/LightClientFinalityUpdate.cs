using Lantern.Types.Containers;
using Lantern.Types.Basic;
using Lantern.Types.Crypto;
using Lantern.Config;
using Lantern.Core;

namespace Lantern.Types.Containers
{
	public class LightClientFinalityUpdate
	{
		public readonly LightClientHeader AttestedHeader;
		public readonly LightClientHeader FinalizedHeader;
		public readonly Bytes32[] FinalityBranch;
		public readonly SyncAggregate SyncAggregate;
		public readonly Slot SignatureSlot;

		public static readonly LightClientFinalityUpdate Zero =
			new LightClientFinalityUpdate(LightClientHeader.Zero, LightClientHeader.Zero,
				new Bytes32[Helpers.FloorLog2(Constants.FINALISED_ROOT_INDEX)], SyncAggregate.Zero, Slot.Zero);

		public LightClientFinalityUpdate(LightClientHeader attestedHeader,
            LightClientHeader finalizedHeader, Bytes32[] finalityBranch,
			SyncAggregate syncAggregate, Slot signatureSlot)
		{
			AttestedHeader = attestedHeader;
			FinalizedHeader = finalizedHeader;
			FinalityBranch = finalityBranch;
			SyncAggregate = syncAggregate;
			SignatureSlot = signatureSlot;
		}
    }
}

