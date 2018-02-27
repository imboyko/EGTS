using EGTS.ServiceLayer;
using System.Collections.Generic;

namespace EGTS.TransportLayer
{
    public abstract class ServiceFrameData : IGetByteArray
    {
        /// <summary>SDR (Service Data Record)</summary>
        public List<ServiceDataRecord> ServiceDataRecords { get; set; }

        public abstract byte[] GetBytes();
    }
}
