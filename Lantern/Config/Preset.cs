using System;
using Lantern.Types.Basic;
using Lantern.Config;

namespace Lantern.Config
{
	public static class Preset
	{
        public static int SYNC_COMMITTEE_SIZE { get; set; }

        public static Epoch EPOCHS_PER_SYNC_COMMITTEE_PERIOD { get; set; }

        public static int SECONDS_PER_SLOT { get; set; }

        public static Slot SLOTS_PER_EPOCH { get; set; }

        public static int UPDATE_TIMEOUT { get; set; }

        public static ForkVersion GENESIS_FORK_VERSION { get; set; }

        public static ForkVersion ALTAIR_FORK_VERSION { get; set; }

        public static ForkVersion BELLATRIX_FORK_VERSION { get; set; }

        public static ForkVersion CAPELLA_FORK_VERSION { get; set; }

        public static ForkVersion EIP4844_FORK_VERSION { get; set; }

        public static Epoch ALTAIR_FORK_EPOCH { get; set; }

        public static Epoch BELLATRIX_FORK_EPOCH { get; set; }

        public static Epoch CAPELLA_FORK_EPOCH { get; set; }

        public static Epoch EIP4844_FORK_EPOCH { get; set; }

        public static void SetPreset<T>() where T : IPreset, new()
        {
            var preset = new T();
            SYNC_COMMITTEE_SIZE = preset.SYNC_COMMITTEE_SIZE;
            EPOCHS_PER_SYNC_COMMITTEE_PERIOD = preset.EPOCHS_PER_SYNC_COMMITTEE_PERIOD;
            SECONDS_PER_SLOT = preset.SECONDS_PER_SLOT;
            SLOTS_PER_EPOCH = preset.SLOTS_PER_EPOCH;
            UPDATE_TIMEOUT = preset.UPDATE_TIMEOUT;
            GENESIS_FORK_VERSION = preset.GENESIS_FORK_VERSION;
            ALTAIR_FORK_VERSION = preset.ALTAIR_FORK_VERSION;
            BELLATRIX_FORK_VERSION = preset.BELLATRIX_FORK_VERSION;
            CAPELLA_FORK_VERSION = preset.CAPELLA_FORK_VERSION;
            EIP4844_FORK_VERSION = preset.EIP4844_FORK_VERSION;
            ALTAIR_FORK_EPOCH = preset.ALTAIR_FORK_EPOCH;
            BELLATRIX_FORK_EPOCH = preset.BELLATRIX_FORK_EPOCH;
            CAPELLA_FORK_EPOCH = preset.CAPELLA_FORK_EPOCH;
            EIP4844_FORK_EPOCH = preset.EIP4844_FORK_EPOCH;         
        }
        
    }
}

