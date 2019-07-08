using System;
using System.Collections.Generic;
using System.Text;

namespace Telematics.EGTS
{
    public partial class Packet
    {
        /// <summary>
        /// С помощью данного типа пакета осуществляется подтверждения пакета Транспортного Уровня. 
        /// Он содержит помимо структур Уровня Поддержки Услуг, информацию о результате обработки данных Протокола Транспортного Уровня, полученного ранее. 
        /// </summary>
        internal partial class EgtsResponseAppData : EgtsAppData, IEgtsAppData
        {
            private ushort _RPID;
            private byte _PR;

            public EgtsResponseAppData() : base() { }

            public ushort ResponseTo
            {
                get => _RPID;
                set => _RPID = value;
            }
            public Types.ProcessingCode ProcessingCode
            {
                get => (Types.ProcessingCode)_PR;
                set => _PR = (byte)value;
            }
        }
    }
}
