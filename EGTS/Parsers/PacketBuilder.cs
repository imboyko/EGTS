using System;
using EGTS.TransportLayer;
using EGTS.ServiceLayer;
using EGTS.Parsers;

namespace EGTS
{
    public static class PacketBuilder
    {
        public static Packet ByteToPacket(byte[] data)
        {
            // Creating new Packet instance
            Packet packet = new Packet();
            packet.Header = ParseHeader(data);  // TODO: create an interface for header parser for use its instance

            int packetLength = packet.Header.HeaderLength + packet.Header.FrameDataLength;
            if (packetLength < data.Length)
            {
                packet.CRC = BitConverter.ToUInt16(data, packetLength - 1);
            }

            // Parsing service layer data
            IServiceFrameParser serviceFrameParser = null;
            switch (packet.Header.Type)
            {
                case PacketType.EGTS_PT_APPDATA:
                    serviceFrameParser = new AppdataParser();
                    break;
                case PacketType.EGTS_PT_SIGNED_APPDATA:
                    serviceFrameParser = new SignedAppdataParser();
                    break;
                case PacketType.EGTS_PT_RESPONSE:
                    serviceFrameParser = new ResponseParser();
                    break;
            }

            packet.ServiceFrameData = serviceFrameParser.Parse(data, packet.Header.HeaderLength, packet.Header.FrameDataLength);

            return packet;
        }

        public static byte[] PacketToByte(Packet packet)
        {
            throw new NotImplementedException();
        }



        private static TransportHeader ParseHeader(byte[] data)
        {
            TransportHeader header = new TransportHeader
            {
                ProtocolVersion = data[0],
                SecurityKeyId = data[1],
                Prefix = (byte)((data[2] & (byte)Bitsets.PRF) >> 6),
                Route = (data[2] & (byte)Bitsets.RTE) == (byte)Bitsets.RTE,
                Compressed = (data[2] & (byte)Bitsets.CMP) == (byte)Bitsets.CMP,
                Priority = (RoutePriority)(data[2] & (byte)Bitsets.PR),
                HeaderLength = data[3],
                HeaderEncoding = data[4],
                FrameDataLength = BitConverter.ToUInt16(data, 5), // bytes 5 to 6
                PID = BitConverter.ToUInt16(data, 7), // bytes 7 to 8
                Type = (PacketType)data[9]
            };

            if (header.Route)
            {
                header.RoutingInfo = new RoutingInfo
                {
                    PeerAddress = BitConverter.ToUInt16(data, 10), // bytes 10 to 11
                    RecipientAddress = BitConverter.ToUInt16(data, 12), // bytes 12 to 13
                    TTL = data[14]
                };


                header.CRC = data[15];
            }
            else
            {
                header.CRC = data[10];
            }

            return header;
        }

        private enum Bitsets : byte
        {
            bit0 = (1 << 0),
            bit1 = (1 << 1),
            bit2 = (1 << 2),
            bit3 = (1 << 3),
            bit4 = (1 << 4),
            bit5 = (1 << 5),
            bit6 = (1 << 6),
            bit7 = (1 << 7),

            PR = bit0 | bit1,
            CMP = bit2,
            RTE = bit5,
            PRF = bit6 | bit7
        }
    }
}
