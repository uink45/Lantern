using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lantern.Types.Basic;
using Lantern.Types.Crypto;

namespace Lantern.Types.Containers
{
    public class ForkData 
    {
        public readonly ForkVersion CurrentVersion;
        public readonly Root GenesisValidatorsRoot;

        public ForkData()
        {
            CurrentVersion = ForkVersion.Zero;
            GenesisValidatorsRoot = Root.Zero;
        }
        public ForkData(ForkVersion currentVersion, Root genesisValidatorsRoot)
        {
            CurrentVersion = currentVersion;
            GenesisValidatorsRoot = genesisValidatorsRoot;
        }
    }
}
