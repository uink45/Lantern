using System;
using Lantern.Types.Basic;

namespace Lantern.Config
{
	public interface IPreset
	{
        public int SYNC_COMMITTEE_SIZE { get; }

        public Epoch EPOCHS_PER_SYNC_COMMITTEE_PERIOD { get; }

        public int SECONDS_PER_SLOT { get; }

        public Slot SLOTS_PER_EPOCH { get; }

        public int UPDATE_TIMEOUT { get; }

        public ForkVersion GENESIS_FORK_VERSION { get; }

        public ForkVersion ALTAIR_FORK_VERSION { get; }

        public ForkVersion BELLATRIX_FORK_VERSION { get; }

        public ForkVersion CAPELLA_FORK_VERSION { get; }

        public ForkVersion EIP4844_FORK_VERSION { get; }

        public Epoch ALTAIR_FORK_EPOCH { get; }

        public Epoch BELLATRIX_FORK_EPOCH { get; }

        public Epoch CAPELLA_FORK_EPOCH { get; }

        public Epoch EIP4844_FORK_EPOCH { get; }
    }
}

