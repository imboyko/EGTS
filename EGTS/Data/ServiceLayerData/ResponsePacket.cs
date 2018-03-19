using System;

namespace Egts.Data.ServiceLayer
{
    public class ResponsePacket : ServiceFrameData
    {
        /// <summary>RPID (Response Packet ID)</summary>
        public ushort ResponseTo { get; set; }

        /// <summary>PR (Processing Result)</summary>
        public ProcessingCode ResultCode { get; set; }

        public override void Process(ref ProcessingResult result)
        {
            throw new System.NotImplementedException();
        }

        public override byte[] GetBytes()
        {
            byte[] result = new byte[3];

            BitConverter.GetBytes(ResponseTo).CopyTo(result, 0);
            result[2] = (byte)ResultCode;

            foreach (ServiceDataRecord record in ServiceDataRecords)
            {
                int curPos = result.Length;
                byte[] recordBytes = record.GetBytes();

                if (recordBytes.Length > 0)
                {
                    Array.Resize(ref result, result.Length + recordBytes.Length);
                    recordBytes.CopyTo(result, curPos);
                }
            }

            return result;
        }

    }
}
