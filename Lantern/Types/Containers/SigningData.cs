using Lantern.Types.Basic;
using Lantern.Types.Crypto;

namespace Lantern.Types.Containers
{
    public class SigningData
    {
        public readonly Domain Domain;
        public readonly Root ObjectRoot;

        public SigningData(Root objectRoot, Domain domain)
        {
            ObjectRoot = objectRoot;
            Domain = domain;
        }
    }
}
