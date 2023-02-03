using Lantern.Types.Containers;

namespace Lantern.Core.Upgraders;

public static class CapellaUpgrader
{
    public static LightClientHeader UpgradeLcHeaderToCapella(LightClientHeader pre)
    {
        return new LightClientHeader(pre.Beacon);
    }

    public static LightClientBootstrap UpgradeLcBootstrapToCapella(LightClientBootstrap pre)
    {
        return new LightClientBootstrap(UpgradeLcHeaderToCapella(pre.Header),
            pre.CurrentSyncCommittee,
            pre.CurrentSyncCommitteeBranch);
    }

    public static LightClientUpdate UpgradeLcUpdateToCapella(LightClientUpdate pre)
    {
        return new LightClientUpdate(UpgradeLcHeaderToCapella(pre.AttestedHeader),
            pre.NextSyncCommittee,
            pre.NextSyncCommitteeBranch,
            UpgradeLcHeaderToCapella(pre.FinalizedHeader),
            pre.FinalityBranch,
            pre.SyncAggregate,
            pre.SignatureSlot
        );
    }

    public static LightClientFinalityUpdate UpgradeLcFinalityUpdateToCapella(LightClientFinalityUpdate pre)
    {
        return new LightClientFinalityUpdate(UpgradeLcHeaderToCapella(pre.AttestedHeader),
            UpgradeLcHeaderToCapella(pre.FinalizedHeader),
            pre.FinalityBranch,
            pre.SyncAggregate,
            pre.SignatureSlot);
    }

    public static LightClientOptimisticUpdate UpgradeLcOptimisticUpdateToCapella(LightClientOptimisticUpdate pre)
    {
        return new LightClientOptimisticUpdate(UpgradeLcHeaderToCapella(pre.AttestedHeader),
            pre.SyncAggregate,
            pre.SignatureSlot);
    }

    public static LightClientStore UpgradeLcStoreToCapella(LightClientStore pre)
    {
        var bestValidUpdate = pre.BestValidUpdate.Equals(LightClientUpdate.Zero) ? LightClientUpdate.Zero : UpgradeLcUpdateToCapella(pre.BestValidUpdate);

        return new LightClientStore(UpgradeLcHeaderToCapella(pre.FinalizedHeader),
            pre.CurrentSyncCommittee,
            pre.NextSyncCommittee,
            bestValidUpdate,
            UpgradeLcHeaderToCapella(pre.OptimisticHeader),
            pre.CurrentMaxActiveParticipants,
            pre.CurrentMaxActiveParticipants
            );
    }
    
}