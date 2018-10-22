namespace EGTS.Types
{
    public interface ISubrecord
    {
        DataRecord Record { get; }
        SubrecordType Type { get; }
    }

    public class ResponseSubrecord : ISubrecord
    {
        public DataRecord Record { get; }
        public SubrecordType Type => SubrecordType.EGTS_SR_RECORD_RESPONSE;

        public ushort ConfirmedRecordNumber { get; set; }
        public byte RecordStatus { get; set; }

        public ResponseSubrecord(DataRecord owner)
        {
            this.Record = owner;
        }
    }

    public class PosDataSubrecord : ISubrecord
    {
        public DataRecord Record { get; }
        public SubrecordType Type => SubrecordType.EGTS_SR_POS_DATA;

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
        public System.DateTime Time { get; set; }

        public PosDataSubrecord(DataRecord owner)
        {
            this.Record = owner;
        }
    }
}
