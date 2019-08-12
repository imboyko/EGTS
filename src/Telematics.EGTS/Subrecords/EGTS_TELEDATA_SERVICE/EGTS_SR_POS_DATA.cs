using System;
using System.Collections.Generic;
using System.Text;

namespace Telematics.EGTS
{
    /// <summary>
    /// Подзапись используется АТ при передаче основных данных определения местоположения
    /// </summary>
    sealed class EGTS_SR_POS_DATA : IEgtsDataSubrecord
    {
        private uint _NTM;
        private uint _LAT;
        private uint _LON;
        private byte _Flags;
        private ushort _SPD;
        private ushort _DIR;
        private bool _ALTS;
        private uint _ODM;
        private byte _DIN;
        private byte _SRC;
        private uint _ALT;
        private short _SRCD;

        public EgtsSubrecordType Type => EgtsSubrecordType.EGTS_SR_POS_DATA;
    }
}
