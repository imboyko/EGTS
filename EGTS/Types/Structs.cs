using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGTS.Types
{
    /// <summary>
    /// Структура представляет Заголовок Протокола Транспортного Уровня.
    /// </summary>
    internal struct Header
    {
        public byte PRV;
        public byte SKID;
        public byte Bitfield;

        public byte PRF => (byte)((Bitfield & (3 << 6)) >> 6);
        public bool RTE => (Bitfield & (1 << 5)) == (1 << 5);
        public byte ENA => (byte)((Bitfield & (3 << 3)) >> 3);
        public bool CMP => (Bitfield & (1 << 2)) == (1 << 2);
        public byte PR => (byte)(Bitfield & (3 << 0));

        public byte HL;
        public byte HE;
        public ushort FDL;
        public ushort PID;
        public byte PT;
        public ushort PRA;
        public ushort RCA;
        public byte TTL;
        public byte HCS;

        public byte[] SFRD;
        public ushort SFRCS;

        public byte[] HeaderBytes;
    }

    /// <summary>
    /// Структура представляет Протокол Уровня Поддержки Услуг.
    /// </summary>
    internal struct SFRD
    {
        public short SIGL;
        public byte[] SIGD;

        public ushort RPID;
        public byte PR;

        public int StartIndex;
    }

}
