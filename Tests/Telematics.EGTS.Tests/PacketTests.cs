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

            // ѕроверка значений по умолчанию дл€ флаговых свойств.
            Assert.Equal(0, packet.Prefix);
            Assert.False(packet.Route);
            Assert.Equal(0, packet.EncryptionAlgorithm);
            Assert.False(packet.Compressed);
            Assert.Equal(Types.Priority.Highest, packet.Priority);
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

            // ”становка свойства должна вли€ть только на себ€
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

            // ”становка свойства должна вли€ть только на себ€
            Assert.Equal(0, packet.Prefix);
            Assert.False(packet.Route);
            Assert.Equal(0, packet.EncryptionAlgorithm);
            Assert.False(packet.Compressed);
            Assert.Equal(value, packet.Priority);
        }
        #endregion
    }
}
