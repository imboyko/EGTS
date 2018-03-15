using System.Collections.Generic;

namespace EGTS.Data.ServiceLayer
{
    public abstract class ServiceFrameData : IGetByteArray
    {
        public ServiceFrameData()
        {
            ServiceDataRecords = new List<ServiceDataRecord>();
        }

        /// <summary>SDR (Service Data Record)</summary>
        public List<ServiceDataRecord> ServiceDataRecords { get; set; }

        public abstract byte[] GetBytes();
    }
}
