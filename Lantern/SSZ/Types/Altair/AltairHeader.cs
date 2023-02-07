using Lantern.Types.Containers;

namespace Lantern.SSZ.Types.Altair;

public class AltairHeader
{
    [SszElement(0, "Container")]
    public BeaconBlockHeaderSSZ Beacon { get; set; }
    
    public static AltairHeader Serialize(LightClientHeader header)
    {
        AltairHeader headerSSZ = new AltairHeader();
        headerSSZ.Beacon = BeaconBlockHeaderSSZ.Serialize(header.Beacon);
        return headerSSZ;
    }

    public static LightClientHeader Deserialize(AltairHeader headerSSZ)
    {
        return new LightClientHeader(BeaconBlockHeaderSSZ.Deserialize(headerSSZ.Beacon));
    }
}