using System.Collections.Generic;

namespace EGTS.Types
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
        /// Тип пакета
        /// </summary>
        PacketType Type { get; }

        /// <summary>
        /// Список записей уровня сервиса.
        /// </summary>
        IReadOnlyList<DataRecord> Records { get; }

        /// <summary>
        /// Добавляет запись в список записей и возвращает ее экземпляр.
        /// </summary>
        /// <returns>Экземпляр записи</returns>
        DataRecord CreateRecord();
    }

    // Абстрактный класс, представляющий содержимое пакета EGTS (сервисный уровень)
    public abstract class PacketData : IPacketData
    {
        // Коллекция записей пакета
        private readonly List<DataRecord> records = new List<DataRecord>();
        
        public Packet Packet { get; }
        public abstract PacketType Type { get; }

        IReadOnlyList<DataRecord> IPacketData.Records => records.AsReadOnly();

        public PacketData(Packet owner)
        {
            this.Packet = owner;
        }

        public DataRecord CreateRecord()
        {
            var record = new DataRecord(Packet);
            records.Add(record);

            return record;
        }
    }

    public class Appdata : PacketData
    {
        public override PacketType Type => PacketType.EGTS_PT_APPDATA;

        public Appdata(Packet owner) : base(owner)
        {
        }
    }

    public class SignedAppdata : PacketData
    {
        public override PacketType Type => PacketType.EGTS_PT_SIGNED_APPDATA;
        public short SignatureLength { get; set; }
        public byte[] SignatureData { get; set; }

        public SignedAppdata(Packet owner) : base(owner)
        {
        }
    }

    public class Response : PacketData
    {
        public override PacketType Type => PacketType.EGTS_PT_RESPONSE;
        public ushort ResponseTo { get;}
        public ProcessingCode ResultCode { get; set; }

        public Response(Packet owner, ushort responseTo) : base(owner)
        {
            ResponseTo = responseTo;
        }
    }
}
