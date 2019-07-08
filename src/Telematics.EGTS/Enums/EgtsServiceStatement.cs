namespace Telematics.EGTS
{
    /// <summary>
    /// Текущее состояние сервиса
    /// </summary>
    public enum EgtsServiceStatement : byte
    {
        /// <summary>
        /// Сервис в рабочем состоянии и разрешен к использованию
        /// </summary>
        EGTS_SST_IN_SERVICE = 0,
        /// <summary>
        /// Сервис в нерабочем состоянии (выключен)
        /// </summary>
        EGTS_SST_OUT_OF_SERVICE = 128,
        /// <summary>
        /// Сервис запрещён для использования
        /// </summary>
        EGTS_SST_DENIED = 129,
        /// <summary>
        /// Сервис не настроен
        /// </summary>
        EGTS_SST_NO_CONF = 130,
        /// <summary>
        /// Сервис временно недоступен
        /// </summary>
        EGTS_SST_TEMP_UNAVAIL = 131,
    }
}