using System;
using System.Collections.Generic;
using System.Text;

namespace Telematics.EGTS
{
    /// <summary>
    /// Подзапись используется только АС при запросе авторизации на авторизующей ТП и содержит учётные данные АС
    /// </summary>
    sealed class EGTS_SR_TERM_IDENTITY : IEgtsDataSubrecord
    {
        private uint _TID;
        private byte _Flags;
        private ushort _HDID;
        private string _IMEI;
        private string _IMSI;
        private string _LNGC;
        private ushort _NID;
        private ushort _BS;
        private string _MSISDN;

        public EgtsSubrecordType Type => EgtsSubrecordType.EGTS_SR_TERM_IDENTITY;
    }
}
