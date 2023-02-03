using System;
using System.Numerics;
using Lantern.Types.Crypto;
using Lantern.Types.Containers;

namespace Lantern.SSZ.Consensus
{
	public class ExecutionPayloadHeaderSSZ
	{
        [SszElement(0, "Vector[uint8, 32]")]
        public byte[] ParentHash { get; set; }

        [SszElement(1, "Vector[uint8, 20]")]
        public byte[] FeeRecipient { get; set; }

        [SszElement(2, "Vector[uint8, 32]")]
        public byte[] StateRoot { get; set; }

        [SszElement(3, "Vector[uint8, 32]")]
        public byte[] ReceiptsRoot { get; set; }

        [SszElement(4, "Vector[uint8, BYTES_PER_LOGS_BLOOM]")]
        public byte[] LogsBloom { get; set; }

        [SszElement(5, "Vector[uint8, 32]")]
        public byte[] PrevRandao { get; set; }

        [SszElement(6, "uint64")]
        public ulong BlockNumber { get; set; }

        [SszElement(7, "uint64")]
        public ulong GasLimit { get; set; }

        [SszElement(8, "uint64")]
        public ulong GasUsed { get; set; }

        [SszElement(9, "uint64")]
        public ulong Timestamp { get; set; }

        [SszElement(10, "List[uint8, MAX_EXTRA_DATA_BYTES]")]
        public byte[] ExtraData { get; set; }

        [SszElement(11, "uint256")]
        public BigInteger BaseFeePerGas { get; set; }

        [SszElement(12, "Vector[uint8, 32]")]
        public byte[] BlockHash { get; set; }

        [SszElement(13, "Vector[uint8, 32]")]
        public byte[] TransactionsRoot { get; set; }

        [SszElement(14, "Vector[uint8, 32]")]
        public byte[] WithdrawalsRoot { get; set; }

        public static ExecutionPayloadHeaderSSZ Serialize(ExecutionPayloadHeader executionHeader)
        {            
            ExecutionPayloadHeaderSSZ executionHeaderSSZ = new ExecutionPayloadHeaderSSZ();
            executionHeaderSSZ.ParentHash = executionHeader.ParentHash.Bytes;
            executionHeaderSSZ.FeeRecipient = executionHeader.FeeRecipient.AsSpan().ToArray();
            executionHeaderSSZ.StateRoot = executionHeader.StateRoot.AsSpan().ToArray();
            executionHeaderSSZ.ReceiptsRoot = executionHeader.ReceiptsRoot.AsSpan().ToArray();
            executionHeaderSSZ.LogsBloom = executionHeader.LogsBloom.ToArray();
            executionHeaderSSZ.PrevRandao = executionHeader.PrevRandao.AsSpan().ToArray();
            executionHeaderSSZ.BlockNumber = executionHeader.BlockNumber;
            executionHeaderSSZ.GasLimit = executionHeader.GasLimit;
            executionHeaderSSZ.GasUsed = executionHeader.GasUsed;
            executionHeaderSSZ.Timestamp = executionHeader.Timestamp;
            executionHeaderSSZ.ExtraData = executionHeader.ExtraData.ToArray();
            executionHeaderSSZ.BaseFeePerGas = executionHeader.BaseFeePerGas;
            executionHeaderSSZ.BlockHash = executionHeader.BlockHash.AsSpan().ToArray();
            executionHeaderSSZ.TransactionsRoot = executionHeader.TransactionsRoot.Bytes;
            executionHeaderSSZ.WithdrawalsRoot = executionHeader.WithdrawalsRoot.Bytes;

            return executionHeaderSSZ;
        }

        public static ExecutionPayloadHeader Deserialize(ExecutionPayloadHeaderSSZ executionHeaderSSZ)
        {
            
            return new ExecutionPayloadHeader(new Root(executionHeaderSSZ.ParentHash),
                new ExecutionAddress(executionHeaderSSZ.FeeRecipient),
                new Bytes32(executionHeaderSSZ.StateRoot),
                new Bytes32(executionHeaderSSZ.ReceiptsRoot),
                executionHeaderSSZ.LogsBloom,
                new Bytes32(executionHeaderSSZ.PrevRandao),
                executionHeaderSSZ.BlockNumber,
                executionHeaderSSZ.GasLimit,
                executionHeaderSSZ.GasUsed,
                executionHeaderSSZ.Timestamp,
                executionHeaderSSZ.ExtraData.ToList(),
                new UInt256(executionHeaderSSZ.BaseFeePerGas),
                new Bytes32(executionHeaderSSZ.BlockHash),
                new Root(executionHeaderSSZ.TransactionsRoot),
                new Root(executionHeaderSSZ.WithdrawalsRoot));
        }
    }
}

