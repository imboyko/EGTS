using System;
using Xunit;
using Telematics.EGTS;

namespace Telematics.EGTS.Tests
{
    public class PacketTests
    {
        [Theory(DisplayName = "Создание пакета с указанием PacketType")]
        [InlineData(Types.PacketType.EGTS_PT_APPDATA)]
        [InlineData(Types.PacketType.EGTS_PT_RESPONSE)]
        [InlineData(Types.PacketType.EGTS_PT_SIGNED_APPDATA)]
        public void Constructor_ByType_Passing(Types.PacketType type)
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


        [Fact(DisplayName = "Установка префикса больше 0 недопустима.")]
        public void Set_Prefix_1_Throws_ArgumentOutOfRangeException()
        {
            var packetType = Types.PacketType.EGTS_PT_APPDATA;
            var packet = new Packet(packetType);

            Assert.Throws(
                (new ArgumentOutOfRangeException()).GetType(), 
                () => packet.Prefix = 1);


        }

        [Theory(DisplayName = "Установка свойства Route")]
        [InlineData(true)]
        [InlineData(false)]
        public void SetProperty_Route_AffectPassing(bool value)
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

        [Theory(DisplayName = "Установка свойства Priority")]
        [InlineData(Types.Priority.Highest)]
        [InlineData(Types.Priority.High)]
        [InlineData(Types.Priority.Normal)]
        [InlineData(Types.Priority.Low)]
        public void SetProperty_Priority_AffectPassing(Types.Priority value)
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
    }
}
