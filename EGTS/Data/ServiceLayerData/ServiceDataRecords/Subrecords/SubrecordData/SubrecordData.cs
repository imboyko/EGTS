using Egts.Processing;

namespace Egts.Data.ServiceLayer
{
    public abstract class SubrecordData : IGetByteArray
    {
        public abstract byte[] GetBytes();
    }
}
