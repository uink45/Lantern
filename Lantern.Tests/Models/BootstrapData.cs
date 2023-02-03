using System;
namespace Lantern.Tests.Models
{
	public class BootstrapData
	{
		public string genesis_validators_root { get; set; }
		public string trusted_block_root { get; set; }
		public string bootstrap_fork_digest { get; set; }
		public string store_fork_digest { get; set; }
    }
}

