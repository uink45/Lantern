using Lantern.Types.Containers;
namespace Lantern.Core.Upgraders;

public class DenebUpgrader
{
    public static LightClientHeader UpgradeLcHeaderToDeneb(LightClientHeader pre)
    {
        return new LightClientHeader(pre.Beacon,
            new ExecutionPayloadHeader(pre.Execution.ParentHash,
                pre.Execution.FeeRecipient,
                pre.Execution.StateRoot,
                pre.Execution.ReceiptsRoot,
                pre.Execution.LogsBloom,
                pre.Execution.PrevRandao,
                pre.Execution.BlockNumber,
                pre.Execution.GasLimit,
                pre.Execution.GasUsed,
                pre.Execution.Timestamp,
                pre.Execution.ExtraData,
                pre.Execution.BaseFeePerGas,
                pre.Execution.BlockHash,
                pre.Execution.TransactionsRoot,
                pre.Execution.WithdrawalsRoot), pre.ExecutionBranch);
    }

    public static LightClientBootstrap UpgradeLcBootstrapToDeneb(LightClientBootstrap pre)
    {
        return new LightClientBootstrap(UpgradeLcHeaderToDeneb(pre.Header), pre.CurrentSyncCommittee,
            pre.CurrentSyncCommitteeBranch);
    }

    public static LightClientUpdate UpgradeLcUpdateToDeneb(LightClientUpdate pre)
    {
        return new LightClientUpdate(UpgradeLcHeaderToDeneb(pre.AttestedHeader),
            pre.NextSyncCommittee,
            pre.NextSyncCommitteeBranch,
            UpgradeLcHeaderToDeneb(pre.FinalizedHeader),
            pre.FinalityBranch,
            pre.SyncAggregate,
            pre.SignatureSlot);
    }

    public static LightClientFinalityUpdate UpgradeLcFinalityUpdateToDeneb(LightClientFinalityUpdate pre)
    {
        return new LightClientFinalityUpdate(UpgradeLcHeaderToDeneb(pre.AttestedHeader),
            UpgradeLcHeaderToDeneb(pre.FinalizedHeader),
            pre.FinalityBranch,
            pre.SyncAggregate,
            pre.SignatureSlot);
    }

    public static LightClientOptimisticUpdate UpgradeLcOptimisticUpdateToDeneb(LightClientOptimisticUpdate pre)
    {
        return new LightClientOptimisticUpdate(UpgradeLcHeaderToDeneb(pre.AttestedHeader),
            pre.SyncAggregate,
            pre.SignatureSlot);
    }

    public static LightClientStore UpgradeLcStoreToDeneb(LightClientStore pre)
    {
        return new LightClientStore(UpgradeLcHeaderToDeneb(pre.FinalizedHeader),
            pre.CurrentSyncCommittee,
            pre.NextSyncCommittee,
            pre.BestValidUpdate,
            UpgradeLcHeaderToDeneb(pre.OptimisticHeader),
            pre.PreviousMaxActiveParticipants,
            pre.CurrentMaxActiveParticipants);
    }
}