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

            result.RecResults.Add(new ProcessingResult.RecordResult() { Record = this, Result = code });
        }

        public byte[] GetBytes()
        {
            byte[] result = new byte[7];

            // 0-1 bytes will be setted later

            BitConverter.GetBytes(RecordNumber).CopyTo(result, 2);  // 2-3

            int resize = 0;
            // Set flags
            byte flags = 0;
            if (SourceServiceOnDevice)
            {
                flags = (byte)(flags | (byte)RecordFlags.SSOD);
            };
            if (RecipientServiceOnDevice)
            {
                flags = (byte)(flags | (byte)RecordFlags.RSOD);
            };
            if (Group)
            {
                flags = (byte)(flags | (byte)RecordFlags.GRP);
            };
            if (TimeFieldExists)
            {
                flags = (byte)(flags | (byte)RecordFlags.TMFE);
                resize += 4;
            };
            if (EventFieldExists)
            {
                flags = (byte)(flags | (byte)RecordFlags.EVFE);
                resize += 4;
            };
            if (ObjectFieldExists)
            {
                flags = (byte)(flags | (byte)RecordFlags.OBFE);
                resize += 4;
            };
            flags = (byte)(flags | ((byte)ProcessingPriority << 3));

            result[4] = flags;  // 4

            if (resize > 0)
            {
                Array.Resize(ref result, result.Length + resize);
            }

            // Set mandatory fields
            int curPos = 5;
            if (ObjectFieldExists)
            {
                BitConverter.GetBytes(ObjectID).CopyTo(result, curPos);
                curPos += 4;
            };
            if (EventFieldExists)
            {
                BitConverter.GetBytes(EventID).CopyTo(result, curPos);
                curPos += 4;
            };
            if (TimeFieldExists)
            {
                BitConverter.GetBytes(TM).CopyTo(result, curPos);
                curPos += 4;
            };

            result[curPos] = (byte)SourceService;
            curPos += 1;
            result[curPos] = (byte)RecipientService;
            curPos += 1;

            ushort recLength = 0;
            foreach (ServiceDataSubrecord subrecod in RecordData)
            {
                byte[] subrecBytes = subrecod.GetBytes();

                recLength += (ushort)subrecBytes.Length;

                Array.Resize(ref result, result.Length + subrecBytes.Length);

                subrecBytes.CopyTo(result, curPos);
                curPos += subrecBytes.Length;
            }

            BitConverter.GetBytes(recLength).CopyTo(result, 0);     // 0-1

            return result;
        }
    }
}
