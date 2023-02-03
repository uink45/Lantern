using System;
using Lantern.Types.Crypto;
using System.Runtime.InteropServices;

namespace Lantern.Types.Basic
{
	public struct ForkDigest
	{
        public const int Length = sizeof(uint);

        private readonly uint _number;

        public ReadOnlySpan<byte> AsSpan()
        {
            return MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref this, 1));
        }

        public ForkDigest(Span<byte> span)
        {
            if (span.Length != Length)
            {
                throw new ArgumentOutOfRangeException(nameof(span), span.Length,
                    $"{nameof(DomainType)} must have exactly {Length} bytes");
            }
            _number = MemoryMarshal.Cast<byte, uint>(span)[0];
        }

        public static bool operator ==(ForkDigest a, ForkDigest b)
        {
            return a._number == b._number;
        }

        public static bool operator !=(ForkDigest a, ForkDigest b)
        {
            return !(a == b);
        }

        public bool Equals(ForkDigest other)
        {
            return _number == other._number;
        }

        public override bool Equals(object? obj)
        {
            return obj is DomainType other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _number.GetHashCode();
        }

        public override string ToString()
        {
            return AsSpan().ToHexString(true);
        }
    }
}

