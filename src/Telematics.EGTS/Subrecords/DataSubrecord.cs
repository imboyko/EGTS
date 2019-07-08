using System;
using System.Collections.Generic;
using System.Text;

namespace Telematics.EGTS.Types
{
    

    /// <summary>
    /// Подзапись применяется для осуществления подтверждения процесса обработки записи Протокола Уровня Поддержки Услуг.
    /// </summary>
    sealed class EGTS_SR_RECORD_RESPONSE : DataSubrecord
    {
        private ushort _CRN;
        private byte _RST;  // ProcessingCode

        internal EGTS_SR_RECORD_RESPONSE()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_RECORD_RESPONSE;
        }
    }

    #region EGTS_AUTH_SERVICE
    /// <summary>
    /// Подзапись используется только АС при запросе авторизации на авторизующей ТП и содержит учётные данные АС
    /// </summary>
    sealed class EGTS_SR_TERM_IDENTITY : DataSubrecord
    {
        private uint _TID;
        private byte _Flags;
        private ushort _HDID;
        private string _IMEI;
        private string _IMSI;
        private string _LNGC;
        private ushort _NID;
        private ushort _BS;
        private string _MSISDN;

        internal EGTS_SR_TERM_IDENTITY()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_TERM_IDENTITY;
        }
    }
    /// <summary>
    /// Подзапись предназначена для передачи на ТП информации об инфраструктуре на стороне АС, о составе, состоянии и параметрах блоков и модулей АС. 
    /// Данная подзапись является опциональной, и разработчик АС сам принимает решение о необходимости заполнения полей и отправки данной подзаписи. 
    /// Одна подзапись описывает один модуль. В одной записи может передаваться последовательно несколько таких подзаписей, что позволяет передать данные об отдельных составляющих всей аппаратной части АС и периферийного оборудования
    /// </summary>
    sealed class EGTS_SR_MODULE_DATA : DataSubrecord
    {
        private byte _MT;
        private uint _VID;
        private ushort _FWV;
        private ushort _SWV;
        private byte _MD;
        private byte _ST;
        private string _SRN;
        private string _DSCR;

        internal EGTS_SR_MODULE_DATA()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_MODULE_DATA;
        }
    }
    /// <summary>
    /// Подзапись применяется АС для передачи на ТП информации о транспортном средстве.
    /// </summary>
    sealed class EGTS_SR_VEHICLE_DATA : DataSubrecord
    {
        private string _VIN;
        private uint _VHT;
        private uint _VPST;

        internal EGTS_SR_VEHICLE_DATA()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_VEHICLE_DATA;
        }
    }
    /// <summary>
    /// Подзапись используется только авторизуемой ТП при запросе авторизации на авторизующей ТП и содержит учётные данные авторизуемой АС
    /// </summary>
    sealed class EGTS_SR_DISPATCHER_IDENTITY : DataSubrecord
    {
        private byte _DT;
        private uint _DID;
        private string _DSCR;

        internal EGTS_SR_DISPATCHER_IDENTITY()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_DISPATCHER_IDENTITY;
        }
    }
    /// <summary>
    /// Подзапись используется авторизующей ТП для передачи на АС данных о способе и параметрах шифрования, требуемого для дальнейшего взаимодействия
    /// </summary>
    sealed class EGTS_SR_AUTH_PARAMS : DataSubrecord
    {
        private byte _Flags;
        private ushort _PKL;
        private byte[] _PBK;
        private ushort _ISL;
        private ushort _MSZ;
        private string _SS;
        private string _EXP;

        internal EGTS_SR_AUTH_PARAMS()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_AUTH_PARAMS;
        }
    }
    /// <summary>
    /// Подзапись предназначена для передачи на авторизующую ТП аутентификационных данных АС (авторизуемой ТП) с использованием ранее переданных со стороны авторизующей ТП параметров для осуществления шифрования данных
    /// </summary>
    sealed class EGTS_SR_AUTH_INFO : DataSubrecord
    {
        private string _UNM;
        private string _UPSW;
        private string _SS;

        internal EGTS_SR_AUTH_INFO()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_AUTH_INFO;
        }
    }
    /// <summary>
    /// Данный тип подзаписи используется для информирования принимающей стороны, АС или ТП, в зависимости от направления отправки, о поддерживаемых Сервисах, а также для запроса определённого набора требуемых Сервисов (от АС к ТП)
    /// </summary>
    sealed class EGTS_SR_SERVICE_INFO : DataSubrecord
    {
        private byte _ST;
        private byte _SSR;
        private byte _SRVP;

        internal EGTS_SR_SERVICE_INFO()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_SERVICE_INFO;
        }
    }
    /// <summary>
    /// Подзапись применяется авторизующей ТП для информирования АС (авторизуемой ТП) о результатах процедуры аутентификации АС
    /// </summary>
    sealed class EGTS_SR_RESULT_CODE : DataSubrecord
    {
        private byte _RCD;

        internal EGTS_SR_RESULT_CODE()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_RESULT_CODE;
        }
    }
    #endregion

    #region EGTS_TELEDATA_SERVICE
    /// <summary>
    /// Подзапись используется АТ при передаче основных данных определения местоположения
    /// </summary>
    sealed class EGTS_SR_POS_DATA : DataSubrecord
    {
        private uint _NTM;
        private uint _LAT;
        private uint _LON;
        private byte _Flags;
        private ushort _SPD;
        private ushort _DIR;
        private bool _ALTS;
        private uint _ODM;
        private byte _DIN;
        private byte _SRC;
        private uint _ALT;
        private short _SRCD;

        internal EGTS_SR_POS_DATA()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_POS_DATA;
        }
    }
    /// <summary>
    /// Подзапись используется АТ при передаче дополнительных данных определения местоположения
    /// </summary>
    sealed class EGTS_SR_EXT_POS_DATA : DataSubrecord
    {
        private byte _Flags;
        private ushort _VDOP;
        private ushort _HDOP;
        private ushort _PDOP;
        private byte _SAT;
        private ushort _NS;

        internal EGTS_SR_EXT_POS_DATA()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_EXT_POS_DATA;
        }
    }
    /// <summary>
    /// Подзапись применяется АТ для передачи на ТП информации о состоянии дополнительных дискретных и аналоговых входов
    /// </summary>
    sealed class EGTS_SR_AD_SENSORS_DATA : DataSubrecord
    {
        internal EGTS_SR_AD_SENSORS_DATA()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_AD_SENSORS_DATA;
        }
    }
    /// <summary>
    /// Подзапись используется ТП для передачи на АТ данных о значении счётных входов
    /// </summary>
    sealed class EGTS_SR_COUNTERS_DATA : DataSubrecord
    {
        internal EGTS_SR_COUNTERS_DATA()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_COUNTERS_DATA;
        }
    }
    /// <summary>
    /// Подзапись предназначена для передачи на ТП данных профиля ускорения АТ
    /// </summary>
    sealed class EGTS_SR_ACCEL_DATA : DataSubrecord
    {
        internal EGTS_SR_ACCEL_DATA()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_ACCEL_DATA;
        }
    }
    /// <summary>
    /// Данный тип подзаписи используется для передачи на ТП информации о состоянии АТ (текущий режим работы, напряжение основного и резервного источников питания и т.д.)
    /// </summary>
    sealed class EGTS_SR_STATE_DATA : DataSubrecord
    {
        internal EGTS_SR_STATE_DATA()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_STATE_DATA;
        }
    }
    /// <summary>
    /// Подзапись применяется АТ для передачи на ТП данных о состоянии шлейфовых входов (используемых в охранных системах)
    /// </summary>
    sealed class EGTS_SR_LOOPIN_DATA : DataSubrecord
    {
        internal EGTS_SR_LOOPIN_DATA()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_LOOPIN_DATA;
        }
    }
    /// <summary>
    /// Подзапись применяется АТ для передачи на ТП данных о состоянии одного дискретного входа
    /// </summary>
    sealed class EGTS_SR_ABS_DIG_SENS_DATA : DataSubrecord
    {
        internal EGTS_SR_ABS_DIG_SENS_DATA()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_ABS_DIG_SENS_DATA;
        }
    }
    /// <summary>
    /// Подзапись применяется АТ для передачи на ТП данных о состоянии одного аналогового входа
    /// </summary>
    sealed class EGTS_SR_ABS_AN_SENS_DATA : DataSubrecord
    {
        internal EGTS_SR_ABS_AN_SENS_DATA()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_ABS_AN_SENS_DATA;
        }
    }
    /// <summary>
    /// Подзапись применяется АТ для передачи на ТП данных о состоянии одного счётного входа
    /// </summary>
    sealed class EGTS_SR_ABS_CNTR_DATA : DataSubrecord
    {
        internal EGTS_SR_ABS_CNTR_DATA()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_ABS_CNTR_DATA;
        }
    }
    /// <summary>
    /// Подзапись применяется АТ для передачи на ТП данных о состоянии одного шлейфового входа
    /// </summary>
    sealed class EGTS_SR_ABS_LOOPIN_DATA : DataSubrecord
    {
        internal EGTS_SR_ABS_LOOPIN_DATA()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_ABS_LOOPIN_DATA;
        }
    }
    /// <summary>
    /// Подзапись применяется АТ для передачи на ТП данных о показаниях ДУЖ
    /// </summary>
    sealed class EGTS_SR_LIQUID_LEVEL_SENSOR : DataSubrecord
    {
        internal EGTS_SR_LIQUID_LEVEL_SENSOR()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_LIQUID_LEVEL_SENSOR;
        }
    }
    /// <summary>
    /// Подзапись применяется АТ для передачи на ТП данных о показаниях счетчиков пассажиропотока
    /// </summary>
    sealed class EGTS_SR_PASSENGERS_COUNTERS : DataSubrecord
    {
        internal EGTS_SR_PASSENGERS_COUNTERS()
        {
            _SRT = (byte)EgtsSubrecordType.EGTS_SR_PASSENGERS_COUNTERS;
        }
    }

    #endregion
}
