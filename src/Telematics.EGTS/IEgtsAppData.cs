using System.Collections.Generic;

namespace Telematics.EGTS
{
    public partial class Packet
    {
        public interface IEgtsAppData : ICollection<EgtsDataRecord> { }
    }
}
