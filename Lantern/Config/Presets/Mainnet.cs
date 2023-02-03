using System;
using Lantern.Types.Basic;
using Lantern.Types.Crypto;

namespace Lantern.Config
{
    // Update the followiing class so that it can read the parameters from YAML files rather than hardcoding the values, this will improve the maintanance
    public class Mainnet : IPreset
    {
        public int SYNC_COMMITTEE_SIZE { get; } = 512;

        public Epoch EPOCHS_PER_SYNC_COMMITTEE_PERIOD { get; } = new Epoch(256);

        public int SECONDS_PER_SLOT { get; } = 12;

        public Slot SLOTS_PER_EPOCH { get; } = new Slot(32);

        public int UPDATE_TIMEOUT { get; } = 8192;

        public ForkVersion GENESIS_FORK_VERSION { get; } = new ForkVersion(Bytes.FromHexString("0x00000000"));

        public ForkVersion ALTAIR_FORK_VERSION { get; } = new ForkVersion(Bytes.FromHexString("0x01000000"));

        public ForkVersion BELLATRIX_FORK_VERSION { get; } = new ForkVersion(Bytes.FromHexString("0x02000000"));

        public ForkVersion CAPELLA_FORK_VERSION { get; } = new ForkVersion(Bytes.FromHexString("0x03000000"));

        public ForkVersion EIP4844_FORK_VERSION { get; } = new ForkVersion(Bytes.FromHexString("0x04000000"));

        public Epoch ALTAIR_FORK_EPOCH { get; } = new Epoch(74240);

        public Epoch BELLATRIX_FORK_EPOCH { get; } = new Epoch(144896);

        public Epoch CAPELLA_FORK_EPOCH { get; } = new Epoch(18446744073709551615);

        public Epoch EIP4844_FORK_EPOCH { get; } = new Epoch(18446744073709551615);
    }
}

