using System.Numerics;
using Lantern.Types.Crypto;
using Lantern.Types.Containers;

namespace Lantern.SSZ.Types.Deneb;

public class DenebExecutionPayloadHeader
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
        
    [SszElement(12, "uint256")]
    public BigInteger ExcessDataGas { get; set; }

    [SszElement(13, "Vector[uint8, 32]")]
    public byte[] BlockHash { get; set; }

    [SszElement(14, "Vector[uint8, 32]")]
    public byte[] TransactionsRoot { get; set; }

    [SszElement(15, "Vector[uint8, 32]")]
    public byte[] WithdrawalsRoot { get; set; }
    
    public static DenebExecutionPayloadHeader Serialize(ExecutionPayloadHeader executionHeader)
        {            
            DenebExecutionPayloadHeader executionHeaderSsz = new DenebExecutionPayloadHeader
            {
                ParentHash = executionHeader.ParentHash.Bytes,
                FeeRecipient = executionHeader.FeeRecipient.AsSpan().ToArray(),
                StateRoot = executionHeader.StateRoot.AsSpan().ToArray(),
                ReceiptsRoot = executionHeader.ReceiptsRoot.AsSpan().ToArray(),
                LogsBloom = executionHeader.LogsBloom.ToArray(),
                PrevRandao = executionHeader.PrevRandao.AsSpan().ToArray(),
                BlockNumber = executionHeader.BlockNumber,
                GasLimit = executionHeader.GasLimit,
                GasUsed = executionHeader.GasUsed,
                Timestamp = executionHeader.Timestamp,
                ExtraData = executionHeader.ExtraData.ToArray(),
                BaseFeePerGas = executionHeader.BaseFeePerGas,
                ExcessDataGas = executionHeader.ExcessDataGas,
                BlockHash = executionHeader.BlockHash.AsSpan().ToArray(),
                TransactionsRoot = executionHeader.TransactionsRoot.Bytes,
                WithdrawalsRoot = executionHeader.WithdrawalsRoot.Bytes
            };

            return executionHeaderSsz;
        }

        public static ExecutionPayloadHeader Deserialize(DenebExecutionPayloadHeader header)
        {
            return new ExecutionPayloadHeader(new Root(header.ParentHash),
                new ExecutionAddress(header.FeeRecipient),
                new Bytes32(header.StateRoot),
                new Bytes32(header.ReceiptsRoot),
                header.LogsBloom,
                new Bytes32(header.PrevRandao),
                header.BlockNumber,
                header.GasLimit,
                header.GasUsed,
                header.Timestamp,
                header.ExtraData.ToList(),
                new UInt256(header.BaseFeePerGas),
                new UInt256(header.ExcessDataGas),
                new Bytes32(header.BlockHash),
                new Root(header.TransactionsRoot),
                new Root(header.WithdrawalsRoot));
        }
}