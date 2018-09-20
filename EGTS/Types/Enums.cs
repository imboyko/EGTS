namespace EGTS.Types
{
    public enum Priority : byte
    {
        Highest = 0,
        High,
        Normal,
        Low
    }

    public enum PacketType : byte
    {
        /// <summary>
        /// Подтверждение на протокол транспортного уровня
        /// </summary>
        EGTS_PT_RESPONSE = 0,

        /// <summary>
        /// Пакет, содержащий данные протокола уровня поддержки услуг
        /// </summary>
        EGTS_PT_APPDATA,

        /// <summary>
        /// пакет, содержащий данные протокола уровня поддержки услуг с цифровой подписью
        /// </summary>
        EGTS_PT_SIGNED_APPDATA
    }

    public enum Service : byte
    {
        EGTS_AUTH_SERVICE = 1,
        EGTS_TELEDATA_SERVICE = 2,
        EGTS_COMMANDS_SERVICE = 4,
        EGTS_FIRMWARE_SERVICE = 9,
        EGTS_ECALL_SERVICE = 10
    }

    public enum SubrecordType : byte
    {
        EGTS_SR_RECORD_RESPONSE = 0,
        #region EGTS_AUTH_SERVICE
        EGTS_SR_TERM_IDENTITY = 1,
        EGTS_SR_MODULE_DATA = 2,
        EGTS_SR_VEHICLE_DATA = 3,
        EGTS_SR_DISPATCHER_IDENTITY = 5,
        EGTS_SR_AUTH_PARAMS = 6,
        EGTS_SR_AUTH_INFO = 7,
        EGTS_SR_SERVICE_INFO = 8,
        EGTS_SR_RESULT_CODE = 9,
        #endregion
        #region EGTS_TELEDATA_SERVICE
        EGTS_SR_POS_DATA = 16,
        EGTS_SR_EXT_POS_DATA = 17,
        EGTS_SR_AD_SENSORS_DATA = 18,
        EGTS_SR_COUNTERS_DATA = 19,
        EGTS_SR_ACCEL_DATA = 20,
        EGTS_SR_STATE_DATA = 21,
        EGTS_SR_LOOPIN_DATA = 22,
        EGTS_SR_ABS_DIG_SENS_DATA = 23,
        EGTS_SR_ABS_AN_SENS_DATA = 24,
        EGTS_SR_ABS_CNTR_DATA = 25,
        EGTS_SR_ABS_LOOPIN_DATA = 26,
        EGTS_SR_LIQUID_LEVEL_SENSOR = 27,
        EGTS_SR_PASSENGERS_COUNTERS = 28
        #endregion
    }
}
