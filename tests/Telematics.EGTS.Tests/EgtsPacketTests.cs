using System;
using Xunit;
using Telematics.EGTS;

namespace Telematics.EGTS.Tests
{
    public class EgtsPacketTests
    {
        #region ctor tests
        [Theory]
        [InlineData(Types.PacketType.EGTS_PT_APPDATA)]
        [InlineData(Types.PacketType.EGTS_PT_RESPONSE)]
        [InlineData(Types.PacketType.EGTS_PT_SIGNED_APPDATA)]
        public void Should_Pass_ContructorByPacketType(Types.PacketType type)
        {
            var packet = new EgtsPacket(type);

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
            byte flags = 0b00000000;
            byte hl = 11;
            byte he = 11;
            ushort fdl = ushort.MaxValue;
            ushort pid = ushort.MaxValue;
            byte pt = (byte)packetType;
            byte hcs = 0;

            // Создаем пакет без тела, только заголовок.
            var stream = new System.IO.MemoryStream(11);
            var writer = new System.IO.BinaryWriter(stream);
            writer.Write(prv);
            writer.Write(skid);
            writer.Write(flags);
            writer.Write(hl);
            writer.Write(he);
            writer.Write(fdl);
            writer.Write(pid);
            writer.Write(pt);
            writer.Write(hcs);

            // Сброс курсора потока на начало.
            stream.Position = 0;
            var packet = new EgtsPacket(stream);

            // Используем рефлексию для проверки корректности полей.
            var type = packet.GetType();
            byte actualPRV = (byte)GetInstanceField(type, packet, "_PRV");
            byte actualSKID = (byte)GetInstanceField(type, packet, "_SKID");
            byte actualFlags = (byte)GetInstanceField(type, packet, "_Flags");
            byte actualHL = (byte)GetInstanceField(type, packet, "_HL");
            byte actualHE = (byte)GetInstanceField(type, packet, "_HE");
            ushort actualFDL = (ushort)GetInstanceField(type, packet, "_FDL");
            ushort actualPID = (ushort)GetInstanceField(type, packet, "_PID");
            byte actualPT = (byte)GetInstanceField(type, packet, "_PT");
            ushort actualPRA = (ushort)GetInstanceField(type, packet, "_PRA");
            ushort actualPCA = (ushort)GetInstanceField(type, packet, "_RCA");
            byte actualTTL = (byte)GetInstanceField(type, packet, "_TTL");
            byte actualHCS = (byte)GetInstanceField(type, packet, "_HCS");
            ushort actualSFRCS = (ushort)GetInstanceField(type, packet, "_SFRCS");


            // Проверка приватных полей
            Assert.Equal(prv, actualPRV);
            Assert.Equal(skid, actualSKID);
            Assert.Equal(flags, actualFlags);
            Assert.Equal(hl, actualHL);
            Assert.Equal(he, actualHE);
            Assert.Equal(fdl, actualFDL);
            Assert.Equal(pid, actualPID);
            Assert.Equal(pt, actualPT);
            Assert.Equal(0, actualPRA);
            Assert.Equal(0, actualPCA);
            Assert.Equal(0, actualTTL);
            Assert.Equal(hcs, actualHCS);
            Assert.Equal(0, actualSFRCS);

            // Проверка свойств
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
            var packet = new EgtsPacket(packetType)
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
            var packet = new EgtsPacket(packetType)
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

        private object GetInstanceField(Type type, object instance, string fieldName)
        {
            var bindFlags = System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Static;

            var field = type.GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }
    }
}
