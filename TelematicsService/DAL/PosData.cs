using System;

namespace Telematics.DAL
{
    public class PosData
    {
        public uint Id { get; set; }
        public DateTime Time { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public ushort Direction { get; set; }
        public float Speed { get; set; }
        public float Odometer { get; set; }
        public bool Valid { get; set; }
        public bool Actual { get; set; }
        public bool Moving { get; set; }
    }
}
