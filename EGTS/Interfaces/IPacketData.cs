using System.Collections.Generic;

namespace EGTS
{
    /// <summary>
    /// Интерфейс данных пакета уровня сервиса.
    /// </summary>
    public interface IPacketData
    {
        /// <summary>
        /// Пакет-владелец данных.
        /// </summary>
        Packet Packet { get; }

        /// <summary>
        /// Список записей уровня сервиса.
        /// </summary>
        IReadOnlyList<Types.ServiceDataRecord> DataRecords { get; }


        /// <summary>
        /// Добавляет запись в список записей и возвращает ее экземпляр.
        /// </summary>
        /// <returns>Экземпляр записи</returns>
        Types.ServiceDataRecord CreateRecord();
    }
}
