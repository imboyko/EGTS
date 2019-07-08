using System.Collections.Generic;

namespace Telematics.EGTS
{
    public partial class EgtsPacket
    {
        public interface IEgtsAppData : ICollection<EgtsDataRecord> { }
    }
}
