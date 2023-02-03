using System;
using Lantern.Types.Crypto;
using Lantern.Config;
using Lantern.Types.Basic;

namespace Lantern.Types.Containers
{
	public class ExecutionPayloadHeader : IEquatable<ExecutionPayloadHeader>
	{
        public readonly Root ParentHash;
        public readonly ExecutionAddress FeeRecipient;
        public readonly Bytes32 StateRoot;
        public readonly Bytes32 ReceiptsRoot;
        public readonly byte[] LogsBloom;
        public readonly Bytes32 PrevRandao;
        public readonly ulong BlockNumber;
        public readonly ulong GasLimit;
        public readonly ulong GasUsed;
        public readonly ulong Timestamp;
        public readonly List<byte> ExtraData;
        public readonly UInt256 BaseFeePerGas;
        public readonly Bytes32 BlockHash;
        public readonly Root TransactionsRoot;
        public readonly Root WithdrawalsRoot;

        public static readonly ExecutionPayloadHeader Zero =
            new ExecutionPayloadHeader(Root.Zero,
                ExecutionAddress.Zero,
                Bytes32.Zero,
                Bytes32.Zero,
                new byte[Constants.BYTES_PER_LOGS_BLOOM],
                Bytes32.Zero,
                0,
                0,
                0,
                0,
                new List<byte>(Constants.MAX_EXTRA_DATA_BYTES),
                0, Bytes32.Zero,
                Root.Zero,
                Root.Zero);

        public ExecutionPayloadHeader(Root parentHash,
            ExecutionAddress feeRecipient,
            Bytes32 stateRoot,
            Bytes32 receiptsRoot,
            byte[] logsBloom,
            Bytes32 prevRandao,
            ulong blockNumber,
            ulong gasLimit,
            ulong gasUsed,
            ulong timestamp,
            List<byte> extraData,
            UInt256 baseFeePerGas,
            Bytes32 blockHash,
            Root transactionsRoot,
            Root withdrawalsRoot)
        {
            ParentHash = parentHash;
            FeeRecipient = feeRecipient;
            StateRoot = stateRoot;
            ReceiptsRoot = receiptsRoot;
            LogsBloom = logsBloom;
            PrevRandao = prevRandao;
            BlockNumber = blockNumber;
            GasLimit = gasLimit;
            GasUsed = gasUsed;
            Timestamp = timestamp;
            ExtraData = extraData;
            BaseFeePerGas = baseFeePerGas;
            BlockHash = blockHash;
            TransactionsRoot = transactionsRoot;
            WithdrawalsRoot = withdrawalsRoot;
        }

        public bool Equals(ExecutionPayloadHeader? other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(other, this))
            {
                return true;
            }

            return ParentHash.Equals(other.ParentHash)
                && FeeRecipient.Equals(other.FeeRecipient)
                && StateRoot.Equals(other.StateRoot)
                && ReceiptsRoot.Equals(other.ReceiptsRoot)
                && LogsBloom.SequenceEqual(other.LogsBloom)
                && PrevRandao.Equals(other.PrevRandao)
                && BlockNumber == other.BlockNumber
                && GasLimit == other.GasLimit
                && GasUsed == other.GasUsed
                && Timestamp == other.Timestamp
                && ExtraData.SequenceEqual(other.ExtraData)
                && BaseFeePerGas.Equals(other.BaseFeePerGas)
                && BlockHash.Equals(other.BlockHash)
                && TransactionsRoot.Equals(other.TransactionsRoot)
                && WithdrawalsRoot.Equals(other.WithdrawalsRoot);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(HashCode.Combine(ParentHash,
                FeeRecipient,
                StateRoot,
                ReceiptsRoot,
                LogsBloom,
                PrevRandao,
                BlockNumber,
                GasLimit), HashCode.Combine(
                GasUsed,
                Timestamp,
                ExtraData,
                BaseFeePerGas,
                BlockHash,
                TransactionsRoot,
                WithdrawalsRoot));
        }

        public override bool Equals(object? obj)
        {
            var other = obj as ExecutionPayloadHeader;
            return !(other is null) && Equals(other);
        }
    }
}

