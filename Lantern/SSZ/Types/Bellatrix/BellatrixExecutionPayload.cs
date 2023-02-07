using System.Numerics;

namespace Lantern.SSZ.Types.Bellatrix;

public class BellatrixExecutionPayload
{
    [SszElement(0, "Vector[uint8, 32]")] public byte[] Root { get; set; }

    [SszElement(1, "Vector[uint8, 20]")] public byte[] FeeRecipient { get; set; }

    [SszElement(2, "Vector[uint8, 32]")] public byte[] StateRoot { get; set; }

    [SszElement(3, "Vector[uint8, 32]")] public byte[] ReceiptsRoot { get; set; }

    [SszElement(4, "Vector[uint8, BYTES_PER_LOGS_BLOOM]")]
    public byte[] LogsBloom { get; set; }

    [SszElement(5, "Vector[uint8, 32]")] public byte[] PrevRandao { get; set; }

    [SszElement(6, "uint64")] public ulong BlockNumber { get; set; }

    [SszElement(7, "uint64")] public ulong GasLimit { get; set; }

    [SszElement(8, "uint64")] public ulong GasUsed { get; set; }

    [SszElement(9, "uint64")] public ulong Timestamp { get; set; }

    [SszElement(10, "List[uint8, MAX_EXTRA_DATA_BYTES]")]
    public byte[] ExtraData { get; set; }

    [SszElement(11, "uint256")] public BigInteger BaseFeePerGas { get; set; }

    [SszElement(12, "Vector[uint8, 32]")] public byte[] BlockHash { get; set; }

    [SszElement(13, "List[List[uint8, MAX_BYTES_PER_TRANSACTION], MAX_TRANSACTIONS_PER_PAYLOAD]")]
    public List<byte[]> Transactions { get; set; }
}