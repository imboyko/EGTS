using System.Collections.Generic;

namespace EGTS.Types
{
    // Абстрактный класс, представляющий содержимое пакета EGTS (сервисный уровень)
    public abstract class PacketData : IPacketData
    {
        // Коллекция записей пакета
        private readonly List<ServiceDataRecord> records = new List<ServiceDataRecord>();

        public Packet Packet { get; }
        IReadOnlyList<ServiceDataRecord> IPacketData.DataRecords => records.AsReadOnly();
        
        public PacketData(Packet owner)
        {
            this.Packet = owner;
        }

        public ServiceDataRecord CreateRecord()
        {
            var record = new ServiceDataRecord(Packet);
            records.Add(record);

            return record;
        }
    }

    public class Appdata : PacketData
    {
        public Appdata(Packet owner) : base(owner)
        {
        }
    }

    public class SignedAppdata : PacketData
    {
        public short SignatureLength { get; set; }
        public byte[] SignatureData { get; set; }

        public SignedAppdata(Packet owner) : base(owner)
        {
        }
    }

    public class Response : PacketData
    {
        public ushort ResponseTo { get;}
        public ProcessingCode ResultCode { get; set; }

        public Response(Packet owner, ushort responseTo) : base(owner)
        {
            ResponseTo = responseTo;
        }
    }
}
