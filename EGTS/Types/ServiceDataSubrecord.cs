namespace EGTS.Types
{
    public abstract class ServiceDataSubrecord
    {
        public ServiceDataSubrecord(ServiceDataRecord owner)
        {
            Owner = owner;
        }

        public SubrecordType Type { get; set; }
        public ServiceDataRecord Owner { get; private set; }
    }

    public class ResponseSubrecord : ServiceDataSubrecord
    {
        public ResponseSubrecord(ServiceDataRecord owner) : base(owner)
        {
        }

        public ushort ConfirmedRecordNumber { get; set; }
        public byte RecordStatus { get; set; }
    }

    public class PosDataSubrecord : ServiceDataSubrecord
    {
        public PosDataSubrecord(ServiceDataRecord owner) : base(owner)
        {
        }

        #region Свойства
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public ushort Speed { get; set; }
        public ushort Direction { get; set; }
        public float Odometer { get; set; }
        public byte DigitalInputs { get; set; }
        public byte Source { get; set; }
        public int Altitude { get; set; }
        public bool Valid { get; set; }
        public bool Actual { get; set; }
        public bool Moving { get; set; }
        public System.DateTime NavigationTime
        {
            get
            {
                return new System.DateTime(2010, 1, 1, 0, 0, 0).AddSeconds(NTM);
            }

            set
            {
                NTM = (uint)value.Subtract(new System.DateTime(2010, 1, 1, 0, 0, 0)).Seconds;
            }
        }
        #endregion

        internal uint NTM;
    }
}
