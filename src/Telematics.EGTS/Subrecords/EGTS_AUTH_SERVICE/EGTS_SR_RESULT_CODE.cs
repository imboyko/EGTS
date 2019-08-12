using System;
using System.Collections.Generic;
using System.Text;

namespace Telematics.EGTS
{
    /// <summary>
    /// Подзапись применяется авторизующей ТП для информирования АС (авторизуемой ТП) о результатах процедуры аутентификации АС
    /// </summary>
    sealed class EGTS_SR_RESULT_CODE : IEgtsDataSubrecord
    {
        private byte _RCD;

        public EgtsSubrecordType Type => EgtsSubrecordType.EGTS_SR_RESULT_CODE;
    }
}
