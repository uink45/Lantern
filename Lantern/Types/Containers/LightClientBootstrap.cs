using Lantern.Types.Containers;
using Lantern.Types.Crypto;
using Lantern.Config;
using Lantern.Core;

namespace Lantern.Types.Containers
{
    public class LightClientBootstrap
    {
        public readonly LightClientHeader Header;
        public readonly SyncCommittee CurrentSyncCommittee;
        public readonly Bytes32[] CurrentSyncCommitteeBranch;

        public static readonly LightClientBootstrap Zero =
            new LightClientBootstrap(LightClientHeader.Zero, SyncCommittee.Zero, new Bytes32[Helpers.FloorLog2(Constants.CURRENT_SYNC_COMMITTEE_INDEX)]);

        public LightClientBootstrap(LightClientHeader header, SyncCommittee currentSyncCommittee, Bytes32[] currentSyncCommitteeBranch)
        {
            Header = header;
            CurrentSyncCommittee = currentSyncCommittee;
            CurrentSyncCommitteeBranch = currentSyncCommitteeBranch;
        }
    }
}
