namespace EGTS.TransportLayer
{
    public class ResponsePacket : ServiceFrameData
    {
        /// <summary>RPID (Response Packet ID)</summary>
        public ushort ResponseTo { get; set; }

        /// <summary>PR (Processing Result)</summary>
        public ProcessingCodes ResultCode { get; set; }

        public override byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }
    }
}
