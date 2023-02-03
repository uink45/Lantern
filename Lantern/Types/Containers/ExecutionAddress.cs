using System;
using Lantern.Types.Crypto;
using System.Buffers.Binary;
using System.Diagnostics;

namespace Lantern.Types.Containers
{
	public class ExecutionAddress
	{
        public const int Length = 20;

        private readonly byte[] _bytes;

        public ExecutionAddress()
        {
            _bytes = new byte[Length];
        }

        public static ExecutionAddress Wrap(byte[] bytes)
        {
            return new ExecutionAddress(bytes);
        }
        
        public byte[] Unwrap()
        {
            return _bytes;
        }
        
        private ExecutionAddress(byte[] bytes)
        {
            if (bytes.Length != Length)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), bytes.Length,
                    $"{nameof(ExecutionAddress)} must have exactly {Length} bytes");
            }

            _bytes = bytes;
        }
        
        public ExecutionAddress(ReadOnlySpan<byte> span)
        {
            if (span.Length != Length)
            {
                throw new ArgumentOutOfRangeException(nameof(span), span.Length,
                    $"{nameof(ExecutionAddress)} must have exactly {Length} bytes");
            }

            _bytes = span.ToArray();
        }

        public static ExecutionAddress Zero { get; } = new ExecutionAddress(new byte[Length]);

        public ReadOnlySpan<byte> AsSpan()
        {
            return new ReadOnlySpan<byte>(_bytes);
        }

        public static bool operator ==(ExecutionAddress left, ExecutionAddress right)
        {
            return left.Equals(right);
        }

        public static explicit operator ExecutionAddress(ReadOnlySpan<byte> span) => new ExecutionAddress(span);

        public static explicit operator ReadOnlySpan<byte>(ExecutionAddress value) => value.AsSpan();

        public static bool operator !=(ExecutionAddress left, ExecutionAddress right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return AsSpan().ToHexString(true);
        }

        public bool Equals(ExecutionAddress? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _bytes.SequenceEqual(other._bytes);
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is ExecutionAddress other && Equals(other);
        }

        public override int GetHashCode()
        {
            return BinaryPrimitives.ReadInt32LittleEndian(AsSpan().Slice(16, 4));
        }
    }
}

