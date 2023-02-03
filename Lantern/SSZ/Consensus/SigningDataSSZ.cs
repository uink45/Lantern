using System;
using Lantern.Types.Basic;
using Lantern.Types.Containers;
using Lantern.Types.Crypto;

namespace Lantern.SSZ.Consensus
{
	public class SigningDataSSZ
	{
        [SszElement(0, "Vector[uint8, 32]")]
        public byte[] ObjectRoot { get; set; }

        [SszElement(1, "Vector[uint8, 32]")]
        public byte[] Domain { get; set; }

        public static SigningDataSSZ Serialize(SigningData signingData)
        {
            SigningDataSSZ signingDataSSZ = new SigningDataSSZ();
            signingDataSSZ.ObjectRoot = signingData.ObjectRoot.Bytes;
            signingDataSSZ.Domain = signingData.Domain.Bytes;

            return signingDataSSZ;
        }

        public static SigningData Deserialize(SigningDataSSZ signingDataSSZ)
        {
            return new SigningData(new Root(signingDataSSZ.ObjectRoot),
                new Domain(signingDataSSZ.Domain.AsSpan().ToArray()));
        }
    }
}

