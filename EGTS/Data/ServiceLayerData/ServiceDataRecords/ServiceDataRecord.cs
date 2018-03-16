using Egts.Processing;
using System;
using System.Collections.Generic;

namespace Egts.Data.ServiceLayer
{
    public class ServiceDataRecord : IGetByteArray, IProcessible
    {
        /// <summary>RL (Record Length)</summary>
        public ushort RecordLength { get; set; }
        /// <summary>RN (Record Number)</summary>
        public ushort RecordNumber { get; set; }
        /// <summary>OID (Object Identifier)</summary>
        public uint ObjectID { get; set; }
        /// <summary>EVID (Event Identifier)</summary>
        public uint EventID { get; set; }
        /// <summary>TM (Time)</summary>
        public uint TM { get; set; }
        /// <summary>Represents TM field as DateTime</summary>
        public DateTime Time
        {
            get
            {
                return new DateTime(2010, 1, 1, 0, 0, 0).AddSeconds(TM);
            }

            set
            {
                TM = (uint)value.Subtract(new DateTime(2010, 1, 1, 0, 0, 0)).Seconds;
            }
        }
        /// <summary>SST (Source Service Type)</summary>
        public Service SourceService { get; set; }
        /// <summary>RST (Recipient Service Type)</summary>
        public Service RecipientService { get; set; }
        /// <summary>RD (Record Data)</summary>
        public List<ServiceDataSubrecord> RecordData { get; set; } = new List<ServiceDataSubrecord>();
        #region Record Flags
        /// <summary>SSOD (Source Service On Device)</summary>
        public bool SourceServiceOnDevice { get; set; }
        /// <summary>RSOD (Recipient Service On Device)</summary>
        public bool RecipientServiceOnDevice { get; set; }
        /// <summary>GRP (Group)</summary>
        public bool Group { get; set; }
        /// <summary>RPP (Record Processing Priority)</summary>
        public Priority ProcessingPriority { get; set; }
        /// <summary>TMFE (Time Field Exists)</summary>
        public bool TimeFieldExists { get; set; }
        /// <summary>EVFE (Event ID Field  Exists)</summary>
        public bool EventFieldExists { get; set; }
        /// <summary>OBFE (Object ID Field Exists)</summary>
        public bool ObjectFieldExists { get; set; }
        #endregion

        public IEgtsProcessor Processor { get; private set; }

        public void SetProcessor(IEgtsProcessor processor)
        {
            Processor = processor;
            foreach (ServiceDataSubrecord subrecord in RecordData)
            {
                subrecord.SetProcessor(processor);
            }
        }

        public void Process(ref ProcessingResult result)
        {
            ProcessingCode code = Processor.ProcessServiceDataRecord(this);
            foreach (ServiceDataSubrecord subrecord in RecordData)
            {
                subrecord.Process(ref result);
            }
        }

        public byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }
    }
}
