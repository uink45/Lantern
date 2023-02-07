using System;
using Lantern.Types.Containers;
using Lantern.Types.Basic;
using Lantern.Types.Crypto;

namespace Lantern.SSZ.Types
{
    public class ForkDataSSZ
    {
        [SszElement(0, "Vector[uint8, 4]")]
        public byte[] CurrentVersion { get; set; }

        [SszElement(1, "Vector[uint8, 32]")]
        public byte[] GenesisValidatorsRoot { get; set; }

        public static ForkDataSSZ Serialize(ForkData forkData)
        {
            ForkDataSSZ forkDataSSZ = new ForkDataSSZ();
            forkDataSSZ.CurrentVersion = forkData.CurrentVersion.AsSpan().ToArray();
            forkDataSSZ.GenesisValidatorsRoot = forkData.GenesisValidatorsRoot.Bytes;

            return forkDataSSZ;
        }

        public static ForkData Deserialize(ForkDataSSZ forkDataSSZ)
        {
            return new ForkData(new ForkVersion(forkDataSSZ.CurrentVersion),
                new Root(forkDataSSZ.GenesisValidatorsRoot));
        }
    }
}

