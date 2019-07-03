using System;
using Xunit;
using Telematics.EGTS;

namespace Telematics.EGTS.Tests
{
    public class PacketTests
    {
        #region ctor tests
        [Theory]
        [InlineData(Types.PacketType.EGTS_PT_APPDATA)]
        [InlineData(Types.PacketType.EGTS_PT_RESPONSE)]
        [InlineData(Types.PacketType.EGTS_PT_SIGNED_APPDATA)]
        public void Should_Pass_ContructorByPacketType(Types.PacketType type)
        {
            var packet = new Packet(type);

            Assert.Equal(type, packet.PacketType);

            // Проверка значений по умолчанию для флаговых свойств.
            Assert.Equal(0, packet.Prefix);
            Assert.False(packet.Route);
            Assert.Equal(0, packet.EncryptionAlgorithm);
            Assert.False(packet.Compressed);
            Assert.Equal(Types.Priority.Highest, packet.Priority);
        }

        [Fact]
        public void ConstructorFromStream_Pass__AppDataPacketWithoutRoute()
        {
            var packetType = Types.PacketType.EGTS_PT_APPDATA;
            byte prv = 1;
            byte skid = 0;
            byte flag = 0b00000000;
            byte hl = 11;
            ushort fdl = 0;
            ushort pid = ushort.MaxValue;
            byte pt = (byte)packetType;
            byte hcs = 0;

            // Создаем пакет без тела, только заголовок.
            var stream = new System.IO.MemoryStream(11);
            using(var writer = new System.IO.BinaryWriter(stream))
            {
                writer.Write(prv);
                writer.Write(skid);
                writer.Write(flag);
                writer.Write(hl);
                writer.Write(fdl);
                writer.Write(pid);
                writer.Write(pt);
                writer.Write(hcs);

                writer.Flush();
            }

            var packet = new Packet(stream);

            // Проверка созданного пакета
            Assert.Equal(prv, packet.ProtocolVersion);
            Assert.Equal(skid, packet.SecurityKeyId);
            Assert.Equal(pid, packet.PacketIdentifier);
            Assert.Equal(packetType, packet.PacketType);

            // Флаговые свойства
            Assert.Equal(0, packet.Prefix);
            Assert.False(packet.Route);
            Assert.Equal(0, packet.EncryptionAlgorithm);
            Assert.False(packet.Compressed);
            Assert.Equal(Types.Priority.Highest, packet.Priority);

            // Свойства маршрутизации
            Assert.Equal(0, packet.PeerAddress);
            Assert.Equal(0, packet.RecipientAddress);
            Assert.Equal(0, packet.TTL);

        }
        #endregion

        #region Properties tests
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Pass_SetRouteValue(bool value)
        {
            var packetType = Types.PacketType.EGTS_PT_APPDATA;
            var packet = new Packet(packetType)
            {
                Route = value
            };

            // Установка свойства должна влиять только на себя
            Assert.Equal(0, packet.Prefix);
            Assert.Equal(value, packet.Route);
            Assert.Equal(0, packet.EncryptionAlgorithm);
            Assert.False(packet.Compressed);
            Assert.Equal(Types.Priority.Highest, packet.Priority);
        }

        [Theory]
        [InlineData(Types.Priority.Highest)]
        [InlineData(Types.Priority.High)]
        [InlineData(Types.Priority.Normal)]
        [InlineData(Types.Priority.Low)]
        public void Should_Pass_SetPriorityValue(Types.Priority value)
        {
            var packetType = Types.PacketType.EGTS_PT_APPDATA;
            var packet = new Packet(packetType)
            {
                Priority = value
            };

            // Установка свойства должна влиять только на себя
            Assert.Equal(0, packet.Prefix);
            Assert.False(packet.Route);
            Assert.Equal(0, packet.EncryptionAlgorithm);
            Assert.False(packet.Compressed);
            Assert.Equal(value, packet.Priority);
        }
        #endregion
    }
}
