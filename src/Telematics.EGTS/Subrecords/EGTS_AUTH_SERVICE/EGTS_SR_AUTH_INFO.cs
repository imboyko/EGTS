using System;
using System.Collections.Generic;
using System.Text;

namespace Telematics.EGTS
{
    /// <summary>
    /// Подзапись предназначена для передачи на авторизующую ТП аутентификационных данных АС (авторизуемой ТП) с использованием ранее переданных со стороны авторизующей ТП параметров для осуществления шифрования данных
    /// </summary>
    sealed class EGTS_SR_AUTH_INFO : IEgtsDataSubrecord
    {
        private string _UNM;
        private string _UPSW;
        private string _SS;

        public EgtsSubrecordType Type => EgtsSubrecordType.EGTS_SR_AUTH_INFO;
    }
}
