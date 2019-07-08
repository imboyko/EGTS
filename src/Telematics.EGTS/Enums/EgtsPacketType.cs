namespace Telematics.EGTS
{
    /// <summary>
    /// Тип пакета Транспортного Уровня.
    /// </summary>
    public enum EgtsPacketType : byte
    {
        /// <summary>
        /// Подтверждение на пакет Транспортного Уровня
        /// </summary>
        EGTS_PT_RESPONSE,

        /// <summary>
        /// Пакет, содержащий данные  Протокола Уровня Поддержки Услуг
        /// </summary>
        EGTS_PT_APPDATA,

        /// <summary>
        /// Пакет, содержащий данные  Протокола Уровня Поддержки Услуг с цифровой подписью
        /// </summary>
        EGTS_PT_SIGNED_APPDATA
    }
}