using Egts.Data.ServiceLayer;
using Egts.Data.TransportLayer;
using Egts.Processing;
using System;
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
            int packetLength = Header.HeaderLength + Header.FrameDataLength + ((Header.FrameDataLength > 0) ? 2 : 0);
            byte[] result = new byte[packetLength];

            Header.GetBytes().CopyTo(result, 0);
            ServiceFrameData.GetBytes().CopyTo(result, Header.HeaderLength);

            BitConverter.GetBytes(CRC).CopyTo(result, Header.HeaderLength + Header.FrameDataLength);

            return result;
        }
    }
}
