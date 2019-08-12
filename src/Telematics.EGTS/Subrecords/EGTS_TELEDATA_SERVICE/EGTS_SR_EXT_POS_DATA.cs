using System;
using System.Collections.Generic;
using System.Text;

namespace Telematics.EGTS.Subrecords.EGTS_TELEDATA_SERVICE
{
    /// <summary>
    /// Подзапись используется АТ при передаче дополнительных данных определения местоположения
    /// </summary>
    sealed class EGTS_SR_EXT_POS_DATA : IEgtsDataSubrecord
    {
        private byte _Flags;
        private ushort _VDOP;
        private ushort _HDOP;
        private ushort _PDOP;
        private byte _SAT;
        private ushort _NS;

        public EgtsSubrecordType Type => EgtsSubrecordType.EGTS_SR_EXT_POS_DATA;
    }
}
