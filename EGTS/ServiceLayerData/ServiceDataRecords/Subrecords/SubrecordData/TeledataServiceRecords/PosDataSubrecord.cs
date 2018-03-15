using System;

namespace EGTS.ServiceLayer.TeledataService
{
    public class PosDataSubrecord : SubrecordData
    {
        public uint NTM { get; set; }

        public DateTime NavigationTime
        {
            get
            {
                return new DateTime(2010, 1, 1, 0, 0, 0).AddSeconds(NTM);
            }

            set
            {
                NTM = (uint)value.Subtract(new DateTime(2010, 1, 1, 0, 0, 0)).Seconds;
            }
        }

        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float Speed { get; set; }
        public ushort Direction { get; set; }
        public float Odometer { get; set; }
        public byte DigitalInputs { get; set; }
        public byte Source { get; set; }
        public int Altitude { get; set; }
        public bool Valid { get; set; }
        public bool Actual { get; set; }
        public bool Moving { get; set; }

        public override byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }
    }
}
