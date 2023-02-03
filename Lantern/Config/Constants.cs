using System;
using Lantern.Types.Basic;
using Lantern.Types.Crypto;

namespace Lantern.Config
{
	public static class Constants
	{
		public const int FINALISED_ROOT_INDEX = 105;

		public const int CURRENT_SYNC_COMMITTEE_INDEX = 54;

		public const int NEXT_SYNC_COMMITTEE_INDEX = 55;

		public const int MIN_SYNC_COMMITTEE_PARTICIPANTS = 1;

		public const int BYTES_PER_LOGS_BLOOM = 256;

		public const int MAX_EXTRA_DATA_BYTES = 32;

		public const int EXECUTION_PAYLOAD_INDEX = 25;

        public static DomainType DOMAIN_SYNC_COMMITTEE = new DomainType(Bytes.FromHexString("0x07000000"));

        public static Slot GENESIS_SlOT = (Slot)0;
    }
}

