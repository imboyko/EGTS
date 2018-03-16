namespace Egts.Data.ServiceLayer
{
    public class ResponsePacket : ServiceFrameData
    {
        /// <summary>RPID (Response Packet ID)</summary>
        public ushort ResponseTo { get; set; }

        /// <summary>PR (Processing Result)</summary>
        public ProcessingCode ResultCode { get; set; }

        public override byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }

        public override void Process(ref ProcessingResult result)
        {
            throw new System.NotImplementedException();
        }
    }
}
