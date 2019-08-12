using System;
using System.Collections.Generic;
using System.Text;

namespace Telematics.EGTS
{
    /// <summary>
    /// Подзапись используется только авторизуемой ТП при запросе авторизации на авторизующей ТП и содержит учётные данные авторизуемой АС
    /// </summary>
    sealed class EGTS_SR_DISPATCHER_IDENTITY : IEgtsDataSubrecord
    {
        private byte _DT;
        private uint _DID;
        private string _DSCR;

        public EgtsSubrecordType Type => EgtsSubrecordType.EGTS_SR_DISPATCHER_IDENTITY;
    }
}
