using Egts.Data.ServiceLayer;
using Egts.Data.TransportLayer;
using Egts.Processing;
using System.Collections.Generic;

namespace Egts.Data
{
    public class EgtsPacket : IGetByteArray, IProcessible
    {
        public TransportHeader Header { get; set; }
        public ServiceFrameData ServiceFrameData { get; set; }
        public ushort CRC { get; set; }

        #region IProcessible implementation
        public IEgtsProcessor Processor { get; private set; }

        public void SetProcessor(IEgtsProcessor processor)
        {
            Processor = processor;
            ServiceFrameData.SetProcessor(processor);
        }

        public void Process(ref ProcessingResult result)
        {
            ProcessingCode code = Processor.ProcessPacket(this);
            ServiceFrameData.Process(ref result);
        }
        #endregion

        public byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }
    }
}
