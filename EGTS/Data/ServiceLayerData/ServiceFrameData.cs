using Egts.Processing;
using System.Collections.Generic;

namespace Egts.Data.ServiceLayer
{
    public abstract class ServiceFrameData : IGetByteArray, IProcessible
    {
        public List<ServiceDataRecord> ServiceDataRecords { get; set; }

        public IEgtsProcessor Processor { get; private set; }

        public ServiceFrameData()
        {
            ServiceDataRecords = new List<ServiceDataRecord>();
        }

        public void SetProcessor(IEgtsProcessor processor)
        {
            Processor = processor;
            foreach (ServiceDataRecord record in ServiceDataRecords)
            {
                record.SetProcessor(processor);
            }
        }

        public abstract void Process(ref ProcessingResult result);

        public abstract byte[] GetBytes();
    }
}
