namespace EGTS.ServiceLayer
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
    }
}
