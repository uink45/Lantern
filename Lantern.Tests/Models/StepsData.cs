using System;
namespace Lantern.Tests.Models
{
    public class Step
    {
        public ProcessUpdate? process_update { get; set; }
        public ForceUpdate? force_update { get; set; }
        public UpgradeStore? upgrade_store { get; set; }
    }

    public class ProcessUpdate
    {
        public string update_fork_digest { get; set; }
        public string update { get; set; }
        public int current_slot { get; set; }
        public Checks checks { get; set; }
    }

    public class ForceUpdate
    {
        public int current_slot { get; set; }
        public Checks checks { get; set; }
    }

    public class UpgradeStore
    {
        public string store_fork_digest { get; set; }
        public Checks checks { get; set; }
    }

    public class Checks
    {
        public Header finalized_header { get; set; }
        public Header optimistic_header { get; set; }
    }

    public class Header
    {
        public int slot { get; set; }
        public string beacon_root { get; set; }
        public string execution_root { get; set; }
    }
}

