namespace Egts.Data.ServiceLayer
{
    public class SignedAppdataPacket : ServiceFrameData
    {
        /// <summary>SIGL(Signature Length)</summary>
        public short SignatureLength { get; set; }

        /// <summary>SIGD (Signature Data)</summary>
        public byte[] SignatureData { get; set; }

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
