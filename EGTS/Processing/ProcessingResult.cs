using Egts.Data.ServiceLayer;
using System.Collections.Generic;

namespace Egts
{
    public class ProcessingResult
    {
        public ProcessingCode Result { get; set; }
        public ushort PacketId { get; set; }

        public List<RecordResult> RecResults { get; set; } = new List<RecordResult>();

        public struct RecordResult
        {
            public ServiceDataRecord Record;
            public ProcessingCode Result;
        }
    }
}