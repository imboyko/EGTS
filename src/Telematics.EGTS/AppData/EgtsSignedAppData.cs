namespace Telematics.EGTS
{
    public partial class EgtsPacket
    {
        /// <summary>
        /// Пакет данного типа применяется для передачи помимо структур, содержащих информацию Уровня Поддержки Услуг, также информации о так называемой  «цифровой подписи», идентифицирующей отправителя данного пакета. 
        /// </summary>
        internal partial class EgtsSignedAppData : EgtsAppData, IEgtsAppData
        {
            private short _SIGL;
            private byte[] _SIGD;

            public EgtsSignedAppData() : base() { }
        }
    }
}
