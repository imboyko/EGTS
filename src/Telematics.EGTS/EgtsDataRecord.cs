using System;
using System.IO;

namespace Telematics.EGTS
{
    public sealed class EgtsDataRecord
    {
        public EgtsDataRecord(EgtsPacket packet)
        {
            Packet = packet;
        }
        internal EgtsDataRecord(EgtsPacket packet, Stream input) : this(packet)
        {
            // TODO: создание из потока
        }

        public EgtsPacket Packet { get; }
        public ushort RecordNumber
        {
            get => _RN;
            set => _RN = value;
        }
        public uint ObjectId
        {
            get => _OID;
            set => _OID = value;
        }
        public uint EventId
        {
            get => _EVID;
            set => _EVID = value;
        }
        public DateTime Time
        {
            get => new DateTime(2010, 1, 1).AddSeconds(_TM);
            set => _TM = (uint)value.Subtract(new DateTime(2010, 1, 1)).Seconds;
        }
        public bool SourceServiceOnDevice
        {
            get => (_Flags & 0b10000000) == 0b10000000;
            set => _Flags = (byte)((value ? 0b10000000 : 0) | (_Flags & 0b01111111));
        }
        public bool RecipientServiceOnDevice
        {
            get => (_Flags & 0b01000000) == 0b01000000;
            set => _Flags = (byte)((value ? 0b01000000 : 0) | (_Flags & 0b10111111));
        }
        public bool Group
        {
            get => (_Flags & 0b00100000) == 0b00100000;
            set => _Flags = (byte)((value ? 0b00100000 : 0) | (_Flags & 0b11011111));
        }
        public EgtsPriority ProcessingPriority
        {
            // TODO ProcessingPriority
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public bool TimeFieldExists
        {
            get => (_Flags & 0b00000100) == 0b00000100;
            set => _Flags = (byte)((value ? 0b00000100 : 0) | (_Flags & 0b11111011));
        }
        public bool EventIdFieldExists
        {
            get => (_Flags & 0b00000010) == 0b00000010;
            set => _Flags = (byte)((value ? 0b00000010 : 0) | (_Flags & 0b11111101));
        }
        public bool ObjectIdFieldExists
        {
            get => (_Flags & 0b00000001) == 0b00000001;
            set => _Flags = (byte)((value ? 0b00000001 : 0) | (_Flags & 0b11111110));
        }
        public EgtsServiсe SourceServiceType
        {
            get => (EgtsServiсe)_SST;
            set => _SST = (byte)value;
        }
        public EgtsServiсe RecipientServiceType
        {
            get => (EgtsServiсe)_RST;
            set => _RST = (byte)value;
        }

        private readonly EgtsPacket _EgtsPacket;
        private ushort _RL;
        private ushort _RN;
        private byte _Flags;
        private uint _OID;
        private uint _EVID;
        private uint _TM;
        private byte _SST;
        private byte _RST;

        // UNDONE private object[] RecordData;
    }
}