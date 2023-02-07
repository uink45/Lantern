namespace Lantern.SSZ.Types.Altair;

public class AltairBlock
{
    [SszElement(0, "uint64")] public ulong Slot { get; set; }
    [SszElement(1, "uint64")] public ulong ValidatorIndex { get; set; }
    [SszElement(2, "Vector[uint8, 32]")] public byte[] ParentRoot { get; set; }
    [SszElement(3, "Vector[uint8, 32]")] public byte[] StateRoot { get; set; }
    [SszElement(4, "Container")] public AltairBlockBody Body { get; set; }
    
}