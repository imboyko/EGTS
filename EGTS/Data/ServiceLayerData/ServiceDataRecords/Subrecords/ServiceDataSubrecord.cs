using Egts.Processing;
using System;

namespace Egts.Data.ServiceLayer
{
    public class ServiceDataSubrecord : IGetByteArray, IProcessible
    {
        /// <summary>SRT (Subrecord Type)</summary>
        public SubrecordType Type { get; set; }
        /// <summary>RL (Record Length)</summary>
        public ushort Length { get; set; }
        /// <summary>SRD (Subrecord Data)</summary>
        public SubrecordData Data { get; set; }

        public IEgtsProcessor Processor { get; private set; }

        public void SetProcessor(IEgtsProcessor processor)
        {
            Processor = processor;
        }

        public void Process(ref ProcessingResult result)
        {
            Processor.ProcessServiceDataSubrecord(this);
        }

        public byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }
    }
}
