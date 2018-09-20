using System;
using System.Collections.Generic;

namespace EGTS.Types
{
    public partial class ServiceDataRecord
    {
        internal ServiceDataRecord(Packet owner)
        {
            this.Packet = owner;
        }
        
        #region Свойства
        public Packet Packet { get; }

        public ushort Number { get; set; }
        public uint ObjectID { get; set; }
        public uint EventID { get; set; }
        public bool Group { get; set; }
        public DateTime Time;
        
        public List<ServiceDataSubrecord> ServiceDataSubrecord { get; } = new List<ServiceDataSubrecord>();
        public bool SourceServiceOnDevice { get; set; }
        public bool RecipientServiceOnDevice { get; set; }

        public Service SourceService { get; set; }
        public Service RecipientService { get; set; }

        public Priority ProcessingPriority { get; set; }
   
        #endregion
    }

}
