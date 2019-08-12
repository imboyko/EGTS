using System;
using System.Collections.Generic;
using System.Text;

namespace Telematics.EGTS
{
    /// <summary>
    /// Подзапись применяется для осуществления подтверждения процесса обработки записи Протокола Уровня Поддержки Услуг.
    /// </summary>
    public sealed class EGTS_SR_RECORD_RESPONSE : IEgtsDataSubrecord
    {
        private ushort _CRN;
        private byte _RST;  // ProcessingCode

        public EgtsSubrecordType Type => EgtsSubrecordType.EGTS_SR_RECORD_RESPONSE;
    }
}
