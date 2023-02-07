namespace Lantern.SSZ.Types.Deneb;

public class CapellaBlock
{
    [SszElement(0, "uint64")] public ulong Slot { get; set; }
    [SszElement(1, "uint64")] public ulong ValidatorIndex { get; set; }
    [SszElement(2, "Vector[uint8, 32]")] public byte[] ParentRoot { get; set; }
    [SszElement(3, "Vector[uint8, 32]")] public byte[] StateRoot { get; set; }
    [SszElement(4, "Container")] public DenebBlockBody Body { get; set; }
}