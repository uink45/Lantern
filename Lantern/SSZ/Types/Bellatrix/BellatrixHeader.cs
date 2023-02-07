using Lantern.Types.Containers;
namespace Lantern.SSZ.Types.Bellatrix;

public class BellatrixHeader
{
    [SszElement(0, "Container")]
    public BeaconBlockHeaderSSZ Beacon { get; set; }

    public static BellatrixHeader Serialize(LightClientHeader header)
    {
        BellatrixHeader headerSSZ = new BellatrixHeader();
        headerSSZ.Beacon = BeaconBlockHeaderSSZ.Serialize(header.Beacon);
        return headerSSZ;
    }

    public static LightClientHeader Deserialize(BellatrixHeader headerSSZ)
    {
        return new LightClientHeader(BeaconBlockHeaderSSZ.Deserialize(headerSSZ.Beacon));
    }
}