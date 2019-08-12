using System;
using System.Collections.Generic;
using System.Text;

namespace Telematics.EGTS
{
    /// <summary>
    /// Данный тип подзаписи используется для информирования принимающей стороны, АС или ТП, в зависимости от направления отправки, о поддерживаемых Сервисах, а также для запроса определённого набора требуемых Сервисов (от АС к ТП)
    /// </summary>
    sealed class EGTS_SR_SERVICE_INFO : IEgtsDataSubrecord
    {
        private byte _ST;
        private byte _SSR;
        private byte _SRVP;

        public EgtsSubrecordType Type => EgtsSubrecordType.EGTS_SR_SERVICE_INFO;
    }
}
