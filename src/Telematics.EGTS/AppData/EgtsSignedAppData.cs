using System.IO;

namespace Telematics.EGTS
{
    /// <summary>
    /// Пакет данного типа применяется для передачи помимо структур, содержащих информацию Уровня Поддержки Услуг, также информации о так называемой  «цифровой подписи», идентифицирующей отправителя данного пакета. 
    /// </summary>
    public class EgtsSignedAppData : EgtsAppData, IEgtsAppData
    {
        internal EgtsSignedAppData(EgtsPacket packet) : base(packet) { }
        internal EgtsSignedAppData(EgtsPacket packet, Stream input) : base(packet, input)
        {
            // Чтение полей SignedAppData
            using (var reader = new BinaryReader(input, System.Text.Encoding.UTF8, true))
            {
                _SIGL = reader.ReadInt16();
                _SIGD = reader.ReadBytes(_SIGL);
            }
        }

        private short _SIGL;
        private byte[] _SIGD;
    }
}