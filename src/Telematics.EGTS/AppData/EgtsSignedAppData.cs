namespace Telematics.EGTS
{
    /// <summary>
    /// Пакет данного типа применяется для передачи помимо структур, содержащих информацию Уровня Поддержки Услуг, также информации о так называемой  «цифровой подписи», идентифицирующей отправителя данного пакета. 
    /// </summary>
    public class EgtsSignedAppData : EgtsAppData, IEgtsAppData
    {
        private short _SIGL;
        private byte[] _SIGD;

        internal EgtsSignedAppData() : base() { }
    }
}
