using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EGTS.Tests
{
    [TestClass]
    public class PacketTests
    {
        [TestMethod]
        public void Get_Header_Flags_When_Bitfield_0000000()
        {
            var packet = new Packet();

            Assert.AreEqual<byte>(0, packet.Prefix);
            Assert.IsFalse(packet.Route);
            Assert.AreEqual<byte>(0, packet.EncryptionAlgorithm);
            Assert.IsFalse(packet.Compressed);
            Assert.AreEqual(Types.Priority.Highest, packet.Priority);
        }

        [TestMethod]
        public void SetGet_Priority_Low()
        {
            var packet = new Packet() { Priority = Types.Priority.Low };

            Assert.AreEqual<byte>(0, packet.Prefix);
            Assert.IsFalse(packet.Route);
            Assert.AreEqual<byte>(0, packet.EncryptionAlgorithm);
            Assert.IsFalse(packet.Compressed);
            Assert.AreEqual(Types.Priority.Low, packet.Priority);
        }

        [TestMethod]
        public void SetGet_Compressed_True()
        {
            var packet = new Packet() { Compressed = true };

            Assert.AreEqual<byte>(0, packet.Prefix);
            Assert.IsFalse(packet.Route);
            Assert.AreEqual<byte>(0, packet.EncryptionAlgorithm);
            Assert.IsTrue(packet.Compressed);
            Assert.AreEqual(Types.Priority.Highest, packet.Priority);
        }

        [TestMethod]
        public void SetGet_EncryptionAlgorithm_3()
        {
            var packet = new Packet() { EncryptionAlgorithm = 3 };

            Assert.AreEqual<byte>(0, packet.Prefix);
            Assert.IsFalse(packet.Route);
            Assert.AreEqual<byte>(3, packet.EncryptionAlgorithm);
            Assert.IsFalse(packet.Compressed);
            Assert.AreEqual(Types.Priority.Highest, packet.Priority);
        }

        [TestMethod]
        public void SetGet_Route_True()
        {
            var packet = new Packet() { Route = true };

            Assert.AreEqual<byte>(0, packet.Prefix);
            Assert.IsTrue(packet.Route);
            Assert.AreEqual<byte>(0, packet.EncryptionAlgorithm);
            Assert.IsFalse(packet.Compressed);
            Assert.AreEqual(Types.Priority.Highest, packet.Priority);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetGet_Prefix_3()
        {
            var packet = new Packet() { Prefix = 3 };
        }

    }
}
