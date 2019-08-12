using System;
using System.Collections.Generic;
using System.Text;

namespace Telematics.EGTS
{
    /// <summary>
    /// Подзапись применяется АС для передачи на ТП информации о транспортном средстве.
    /// </summary>
    sealed class EGTS_SR_VEHICLE_DATA : IEgtsDataSubrecord
    {
        private string _VIN;
        private uint _VHT;
        private uint _VPST;

        public EgtsSubrecordType Type => EgtsSubrecordType.EGTS_SR_VEHICLE_DATA;
    }
}
