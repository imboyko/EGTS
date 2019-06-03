using System;
using System.Collections.Generic;
using System.Text;

namespace Telematics.EGTS
{
    interface IPacket
    {
        Types.PacketType Type { get; set; }
        ushort Identifier { get; set; }
    }

    class Packet
    {
        #region Fields
        private byte _PRV;
        private byte _SKID;
        private byte _Flags;
        private byte _HL;
        private byte _HE;
        private ushort _FDL;
        private ushort _PID;
        private byte _PT;
        private ushort _PRA;
        private ushort _RCA;
        private byte _TTL;
        private byte _HCS;
        private ushort _SFRCS;

        private readonly Types.IServiceData _Data;
        #endregion


    }
}
