namespace EGTS.ServiceLayer
{
    public class ServiceDataSubrecord : IGetByteArray
    {
        /// <summary>SRT (Subrecord Type)</summary>
        public SubrecordType Type { get; set; }

        /// <summary>RL (Record Length)</summary>
        public ushort Length{ get; set; }

        /// <summary>SRD (Subrecord Data)</summary>
        public SubrecordData Data{ get; set; }

        public byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }
    }
}
