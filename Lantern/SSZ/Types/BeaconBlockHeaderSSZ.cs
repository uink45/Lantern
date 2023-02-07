using System;
using Lantern.SSZ;
using Lantern.Types.Basic;
using Lantern.Types.Crypto;
using Lantern.Types.Containers;

namespace Lantern.SSZ.Types
{
	public class BeaconBlockHeaderSSZ
	{
        [SszElement(0, "uint64")]
        public ulong Slot { get; set; }

        [SszElement(1, "uint64")]
        public ulong ProposerIndex { get; set; }

        [SszElement(2, "Vector[uint8, 32]")]
        public byte[] ParentRoot { get; set; }

        [SszElement(3, "Vector[uint8, 32]")]
        public byte[] StateRoot { get; set; }

        [SszElement(4, "Vector[uint8, 32]")]
        public byte[] BodyRoot { get; set; }

        public static BeaconBlockHeaderSSZ Serialize(BeaconBlockHeader beaconBlockHeader)
        {
            BeaconBlockHeaderSSZ headerSSZ = new BeaconBlockHeaderSSZ();
            
            headerSSZ.Slot = beaconBlockHeader.Slot;
            headerSSZ.ProposerIndex = beaconBlockHeader.ValidatorIndex;
            headerSSZ.ParentRoot = beaconBlockHeader.ParentRoot.Bytes.ToArray();
            headerSSZ.StateRoot = beaconBlockHeader.StateRoot.Bytes.ToArray();
            headerSSZ.BodyRoot = beaconBlockHeader.BodyRoot.Bytes.ToArray();

            return headerSSZ;
        }

        public static BeaconBlockHeader Deserialize(BeaconBlockHeaderSSZ beaconBlockHeaderSSZ)
        {
            return new BeaconBlockHeader(new Slot(beaconBlockHeaderSSZ.Slot),
                new ValidatorIndex(beaconBlockHeaderSSZ.ProposerIndex),
                new Root(beaconBlockHeaderSSZ.ParentRoot),
                new Root(beaconBlockHeaderSSZ.StateRoot),
                new Root(beaconBlockHeaderSSZ.BodyRoot));
        }
	}
}

