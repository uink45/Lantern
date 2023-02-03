namespace Lantern.Types.Crypto
{
    public struct OpenRoot
    {
        public UInt256 Root { get; set; }
        public int Size { get; set; }

        public Root Close()
        {
            throw new NotImplementedException();
        }
    }
}