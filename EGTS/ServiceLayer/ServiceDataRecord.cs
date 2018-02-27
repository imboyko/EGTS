using System.Collections.Generic;

namespace EGTS.ServiceLayer
{
    public class ServiceDataRecord : IGetByteArray
    {
        /// <summary>RL (Record Length)</summary>
        public ushort RecordLength { get ; set; }

        /// <summary>RN (Record Number)</summary>
        public ushort RecordNumber { get; set; }

        // TODO: Flags

        /// <summary>OID (Object Identifier)</summary>
        public uint ObjectID { get; set; }

        /// <summary>EVID (Event Identifier)</summary>
        public uint EventID { get; set; }

        /// <summary>TM (Time)</summary>
        public uint Time { get; set; }

        /// <summary>SST (Source Service Type)</summary>
        public Services SourceService { get; set; }

        /// <summary>RST (Recipient Service Type)</summary>
        public Services RecipientService { get; set; }

        /// <summary>RD (Record Data)</summary>
        public List<ServiceDataSubrecord> RecordData { get; set; }


        public byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }
    }
}
