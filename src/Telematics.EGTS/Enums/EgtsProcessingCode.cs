﻿namespace Telematics.EGTS
{
    /// <summary>
    /// Коды результатов обработки 
    /// </summary>
    /// <remarks>
    /// Пакеты сообщений об ошибках (EGTS_PC_DECRYPT_ERROR, EGTS_PC_UNS_PROTOCOL, EGTS_PC_INC_DATAFORM , EGTS_PC_DATACRC_ERROR , EGTS_PC_INC_HEADERFORM , EGTS_PC_HEADERCRC_ERROR) 
    /// предназначены для целей тестирования оборудования и в рабочей версии ПО сервера и терминала могут быть исключены.
    /// </remarks>
    public enum EgtsProcessingCode : byte
    {
        /// <summary>
        /// Успешно обработано
        /// </summary>
        EGTS_PC_OK = 0,

        /// <summary>
        /// В процессе обработки (результат обработки ещё не известен)
        /// </summary>
        EGTS_PC_IN_PROGRESS = 1,

        /// <summary>
        /// Неподдерживаемый протокол
        /// </summary>
        /// /// <remarks>
        /// Предназначен для целей тестирования оборудования и в рабочей версии ПО сервера и терминала может быть исключен.
        /// </remarks>
        EGTS_PC_UNS_PROTOCOL = 128,

        /// <summary>
        /// Ошибка декодирования
        /// </summary>
        /// <remarks>
        /// Предназначен для целей тестирования оборудования и в рабочей версии ПО сервера и терминала может быть исключен.
        /// </remarks>
        EGTS_PC_DECRYPT_ERROR = 129,

        /// <summary>
        /// Обработка запрещена
        /// </summary>
        EGTS_PC_PROC_DENIED = 130,

        /// <summary>
        /// Неверный формат заголовка
        /// </summary>
        /// /// <remarks>
        /// Предназначен для целей тестирования оборудования и в рабочей версии ПО сервера и терминала может быть исключен.
        /// </remarks>
        EGTS_PC_INC_HEADERFORM = 131,

        /// <summary>
        /// Неверный формат данных
        /// </summary>
        /// /// <remarks>
        /// Предназначен для целей тестирования оборудования и в рабочей версии ПО сервера и терминала может быть исключен.
        /// </remarks>
        EGTS_PC_INC_DATAFORM = 132,

        /// <summary>
        /// Неподдерживаемый тип
        /// </summary>
        EGTS_PC_UNS_TYPE = 133,

        /// <summary>
        /// Неверное количество параметров
        /// </summary>
        EGTS_PC_NOTEN_PARAMS = 134,

        /// <summary>
        /// Попытка повторной  обработки
        /// </summary>
        EGTS_PC_DBL_PROC = 135,

        /// <summary>
        /// Обработка данных от источника запрещена 
        /// </summary>
        EGTS_PC_PROC_SRC_DENIED = 136,

        /// <summary>
        /// Ошибка контрольной суммы заголовка
        /// </summary>
        /// /// <remarks>
        /// Предназначен для целей тестирования оборудования и в рабочей версии ПО сервера и терминала может быть исключен.
        /// </remarks>
        EGTS_PC_HEADERCRC_ERROR = 137,

        /// <summary>
        /// Ошибка контрольной суммы данных
        /// </summary>
        /// /// <remarks>
        /// Предназначен для целей тестирования оборудования и в рабочей версии ПО сервера и терминала может быть исключен.
        /// </remarks>
        EGTS_PC_DATACRC_ERROR = 138,

        /// <summary>
        /// Некорректная длина данных
        /// </summary>
        EGTS_PC_INVDATALEN = 139,

        /// <summary>
        /// Маршрут не найден
        /// </summary>
        EGTS_PC_ROUTE_NFOUND = 140,

        /// <summary>
        /// Маршрут закрыт
        /// </summary>
        EGTS_PC_ROUTE_CLOSED = 141,

        /// <summary>
        /// Маршрутизация запрещена
        /// </summary>
        EGTS_PC_ROUTE_DENIED = 142,

        /// <summary>
        /// Неверный адрес
        /// </summary>
        EGTS_PC_INVADDR = 143,

        /// <summary>
        /// Превышено количество ретрансляции данных
        /// </summary>
        EGTS_PC_TTLEXPIRED = 144,

        /// <summary>
        /// Нет подтверждения
        /// </summary>
        EGTS_PC_NO_ACK = 145,

        /// <summary>
        /// Объект не найден
        /// </summary>
        EGTS_PC_OBJ_NFOUND = 146,

        /// <summary>
        /// Событие не найдено
        /// </summary>
        EGTS_PC_EVNT_NFOUND = 147,

        /// <summary>
        /// Сервис не найден
        /// </summary>
        EGTS_PC_SRVC_NFOUND = 148,

        /// <summary>
        /// Сервис запрещён
        /// </summary>
        EGTS_PC_SRVC_DENIED = 149,

        /// <summary>
        /// Неизвестный тип сервиса
        /// </summary>
        EGTS_PC_SRVC_UNKN = 150,

        /// <summary>
        /// Авторизация запрещена
        /// </summary>
        EGTS_PC_AUTH_DENIED = 151,

        /// <summary>
        /// Объект уже существует
        /// </summary>
        EGTS_PC_ALREADY_EXISTS = 152,

        /// <summary>
        /// Идентификатор не найден
        /// </summary>
        EGTS_PC_ID_NFOUND = 153,

        /// <summary>
        /// Неправильная дата и время
        /// </summary>
        EGTS_PC_INC_DATETIME = 154,

        /// <summary>
        /// Ошибка ввода/вывода
        /// </summary>
        EGTS_PC_IO_ERROR = 155,

        /// <summary>
        /// Недостаточно ресурсов
        /// </summary>
        EGTS_PC_NO_RES_AVAIL = 156,

        /// <summary>
        /// Внутренний сбой модуля
        /// </summary>
        EGTS_PC_MODULE_FAULT = 157,

        /// <summary>
        /// Сбой в работе цепи питания модуля
        /// </summary>
        EGTS_PC_MODULE_PWR_FLT = 158,

        /// <summary>
        /// Сбой в работе микроконтроллера модуля
        /// </summary>
        EGTS_PC_MODULE_PROC_FLT = 159,

        /// <summary>
        /// Сбой в работе программы модуля
        /// </summary>
        EGTS_PC_MODULE_SW_FLT = 160,

        /// <summary>
        /// Сбой в работе внутреннего ПО модуля
        /// </summary>
        EGTS_PC_MODULE_FW_FLT = 161,

        /// <summary>
        /// Сбой в работе блока ввода/вывода модуля
        /// </summary>
        EGTS_PC_MODULE_IO_FLT = 162,

        /// <summary>
        /// Сбой в работе внутренней памяти модуля
        /// </summary>
        EGTS_PC_MODULE_MEM_FLT = 163,

        /// <summary>
        /// Тест не пройден
        /// </summary>
        EGTS_PC_TEST_FAILED = 164
    }
}