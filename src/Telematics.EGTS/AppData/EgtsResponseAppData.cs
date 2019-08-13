using System.IO;

namespace Telematics.EGTS
{
    /// <summary>
    /// С помощью данного типа пакета осуществляется подтверждения пакета Транспортного Уровня. 
    /// Он содержит помимо структур Уровня Поддержки Услуг, информацию о результате обработки данных Протокола Транспортного Уровня, полученного ранее. 
    /// </summary>
    public class EgtsResponseAppData : EgtsAppData, IEgtsAppData
    {
        /// <summary>Initializes a new instance of the <see cref="EgtsResponseAppData"/> class.</summary>
        /// TODO Edit XML Comment Template for #ctor
        internal EgtsResponseAppData(EgtsPacket packet) : base(packet) { }
        internal EgtsResponseAppData(EgtsPacket packet, Stream input) : base(packet, input)
        {
            // Чтение полей ResponseAppData
            using (var reader = new BinaryReader(input, System.Text.Encoding.UTF8, true))
            {
                _RPID = reader.ReadUInt16();
                _PR = reader.ReadByte();
            }
        }

        public ushort ResponseTo
        {
            get => _RPID;
            set => _RPID = value;
        }
        public EgtsProcessingCode ProcessingCode
        {
            get => (EgtsProcessingCode)_PR;
            set => _PR = (byte)value;
        }

        private ushort _RPID;
        private byte _PR;
    }
}