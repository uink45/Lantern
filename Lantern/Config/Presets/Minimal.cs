using System;
using Lantern.Types.Basic;
using Lantern.Types.Crypto;

namespace Lantern.Config
{
    public class Minimal : IPreset
    {
        public int SYNC_COMMITTEE_SIZE { get; } = 32;

        public Epoch EPOCHS_PER_SYNC_COMMITTEE_PERIOD { get; } = new Epoch(8);

        public int SECONDS_PER_SLOT { get; } = 6;

        public Slot SLOTS_PER_EPOCH { get; } = new Slot(8);

        public int UPDATE_TIMEOUT { get; } = 64;

        public ForkVersion GENESIS_FORK_VERSION { get; } = new ForkVersion(Bytes.FromHexString("0x00000001"));

        public ForkVersion ALTAIR_FORK_VERSION { get; } = new ForkVersion(Bytes.FromHexString("0x01000001"));

        public ForkVersion BELLATRIX_FORK_VERSION { get; } = new ForkVersion(Bytes.FromHexString("0x02000001"));

        public ForkVersion CAPELLA_FORK_VERSION { get; } = new ForkVersion(Bytes.FromHexString("0x03000001"));

        public ForkVersion EIP4844_FORK_VERSION { get; } = new ForkVersion(Bytes.FromHexString("0x04000001"));

        public Epoch ALTAIR_FORK_EPOCH { get; } = new Epoch(0);

        public Epoch BELLATRIX_FORK_EPOCH { get; } = new Epoch(0);

        public Epoch CAPELLA_FORK_EPOCH { get; } = new Epoch(0);

        public Epoch EIP4844_FORK_EPOCH { get; } = new Epoch(4);
    }
}
