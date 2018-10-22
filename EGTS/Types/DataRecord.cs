using System;
using System.Collections.Generic;

namespace EGTS.Types
{
    public partial class DataRecord
    {
        private readonly List<ISubrecord> subrecords = new List<ISubrecord>();
        
        public Packet Packet { get; }
        public ushort Number { get; set; }
        public uint ObjectID { get; set; }
        public uint EventID { get; set; }
        public bool Group { get; set; }
        public DateTime Time;

        public IReadOnlyList<ISubrecord> Subrecords { get => subrecords; }

        public bool SourceServiceOnDevice { get; set; }
        public bool RecipientServiceOnDevice { get; set; }

        public Service SourceService { get; set; }
        public Service RecipientService { get; set; }

        public Priority ProcessingPriority { get; set; }


        public DataRecord(Packet owner)
        {

            this.Packet = owner;
        }

        public override string ToString()
        {
            return $"{this.RecipientService} record #{this.Number}";
        }
    }

}
