using System;
using System.Collections.Generic;
using System.Text;

namespace Telematics.EGTS
{
    /// <summary>
    /// Подзапись используется авторизующей ТП для передачи на АС данных о способе и параметрах шифрования, требуемого для дальнейшего взаимодействия
    /// </summary>
    sealed class EGTS_SR_AUTH_PARAMS : IEgtsDataSubrecord
    {
        private byte _Flags;
        private ushort _PKL;
        private byte[] _PBK;
        private ushort _ISL;
        private ushort _MSZ;
        private string _SS;
        private string _EXP;

        public EgtsSubrecordType Type => EgtsSubrecordType.EGTS_SR_AUTH_PARAMS;
    }
}
