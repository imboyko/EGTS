namespace EGTS.ServiceLayer.TeledataService
{
    public class PosData : SubrecordData
    {
        public uint NavigationTime { get; set; }
        public uint Latitude { get; set; }
        public uint Longitude { get; set; }
        public ushort Speed { get; set; }
        public ushort Direction { get; set; }
        public uint Odometer { get; set; }
        public byte DigitalInputs { get; set; }
        public byte Source { get; set; }
        public uint Altitude { get; set; }

        public override byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }
    }
}
