using System;
using EGTS.TransportLayer;
using EGTS.ServiceLayer;
using System.Collections.Generic;
using EGTS.ServiceLayer.TeledataService;

namespace EGTS
{
    public class PacketBuilder
    {
        public Packet Packet { get; }
        private delegate void ServiceFrameParserDel(ref byte[] data);
        private delegate void SubrecordParserDel(ref byte[] data, int firstByte, ref ServiceDataSubrecord subrecord);
        private Dictionary<PacketType, ServiceFrameParserDel> serviceFrameParsers;
        private Dictionary<SubrecordType, SubrecordParserDel> subrecordParsers;

        public PacketBuilder()
        {
            Packet = new Packet();

            serviceFrameParsers = new Dictionary<PacketType, ServiceFrameParserDel>()
            {
                {PacketType.EGTS_PT_APPDATA, ParseAppdataPacket },
                {PacketType.EGTS_PT_SIGNED_APPDATA, ParseSignedAppdataPacket },
                {PacketType.EGTS_PT_RESPONSE, ParseResponsePacket }
            };

            subrecordParsers = new Dictionary<SubrecordType, SubrecordParserDel>()
            {
                {SubrecordType.EGTS_SR_POS_DATA, ParsePosDataSubrecord }
                // TODO: Add all parsers for the subrecords
            };
        }

        public void BuildFromBytes(byte[] data)
        {
            ParseHeader(ref data);
            ParseServiceFrameData(ref data);
            ParseCRC(ref data);
        }

        private void ParseHeader(ref byte[] data)
        {
            TransportHeader header = new TransportHeader
            {
                ProtocolVersion = data[0],
                SecurityKeyId = data[1],
                Prefix = (byte)((data[2] & (byte)HeaderFlag.PRF) >> 6),
                Route = (data[2] & (byte)HeaderFlag.RTE) == (byte)HeaderFlag.RTE,
                Compressed = (data[2] & (byte)HeaderFlag.CMP) == (byte)HeaderFlag.CMP,
                Priority = (Priority)(data[2] & (byte)HeaderFlag.PR),
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

            Packet.Header = header;
        }

        private void ParseServiceFrameData(ref byte[] data)
        {
            serviceFrameParsers.TryGetValue(Packet.Header.Type, out ServiceFrameParserDel parser);
            parser?.Invoke(ref data);
        }

        private void ParseCRC(ref byte[] data)
        {
            int packetLength = Packet.Header.HeaderLength + Packet.Header.FrameDataLength;
            if (packetLength < data.Length)
            {
                Packet.CRC = BitConverter.ToUInt16(data, packetLength - 1);
            }
        }

        private void ParseAppdataPacket(ref byte[] data)
        {
            Packet.ServiceFrameData = new AppdataPacket();

            ParseServiceDataRecords(ref data);

        }

        private void ParseSignedAppdataPacket(ref byte[] data)
        {
            throw new System.NotImplementedException();
        }

        private void ParseResponsePacket(ref byte[] data)
        {
            throw new System.NotImplementedException();
        }

        private void ParseServiceDataRecords(ref byte[] data)
        {
            int bytesRead = 0;
            int firstByte = Packet.Header.HeaderLength;

            while (bytesRead != Packet.Header.FrameDataLength)
            {
                int offset = 0;
                ServiceDataRecord record = new ServiceDataRecord();

                record.RecordLength = BitConverter.ToUInt16(data, firstByte + offset);
                offset += 2;

                record.RecordNumber = BitConverter.ToUInt16(data, firstByte + offset);
                offset += 2;

                #region Record Flags
                byte flags = data[firstByte + offset];
                offset += 1;

                record.SourceServiceOnDevice = (flags & (byte)(RecordFlag.SSOD)) == (byte)RecordFlag.SSOD;
                record.RecipientServiceOnDevice = (flags & (byte)(RecordFlag.RSOD)) == (byte)RecordFlag.RSOD;
                record.Group = (flags & (byte)(RecordFlag.GRP)) == (byte)RecordFlag.GRP;
                record.ProcessingPriority = (Priority)((flags & (byte)(RecordFlag.RPP)) >> 3);
                record.TimeFieldExists = (flags & (byte)(RecordFlag.TMFE)) == (byte)RecordFlag.TMFE;
                record.EventFieldExists = (flags & (byte)(RecordFlag.EVFE)) == (byte)RecordFlag.EVFE;
                record.ObjectFieldExists = (flags & (byte)(RecordFlag.OBFE)) == (byte)RecordFlag.OBFE;
                #endregion

                if (record.ObjectFieldExists)
                {
                    record.ObjectID = BitConverter.ToUInt32(data, firstByte + offset);
                    offset += 4;
                }

                if (record.EventFieldExists)
                {
                    record.EventID = BitConverter.ToUInt32(data, firstByte + offset);
                    offset += 4;
                }

                if (record.TimeFieldExists)
                {
                    record.TM = BitConverter.ToUInt32(data, firstByte + offset);
                    offset += 4;
                }

                record.SourceService = (Service)data[firstByte + offset];
                offset += 1;

                record.RecipientService = (Service)data[firstByte + offset];
                offset += 1;

                ParseRecord(ref data, (firstByte + offset), ref record);

                firstByte = (firstByte + offset + record.RecordLength);
                bytesRead = (bytesRead + offset + record.RecordLength);

                Packet.ServiceFrameData.ServiceDataRecords.Add(record);
            }
        }

        private void ParseRecord(ref byte[] data, int firstByte, ref ServiceDataRecord record)
        {
            int bytesRead = 0;

            while (bytesRead != record.RecordLength)
            {
                ServiceDataSubrecord subrecord = new ServiceDataSubrecord
                {
                    Type = (SubrecordType)data[firstByte + 0],
                    Length = BitConverter.ToUInt16(data, firstByte + 1)
                };

                subrecordParsers.TryGetValue(subrecord.Type, out SubrecordParserDel parser);
                parser?.Invoke(ref data, (firstByte + 3), ref subrecord);

                record.RecordData.Add(subrecord);

                bytesRead = (bytesRead + subrecord.Length + 3);
                firstByte += (subrecord.Length + 3);
            }
        }

        private void ParsePosDataSubrecord(ref byte[] data, int firstByte, ref ServiceDataSubrecord subrecord)
        {
            PosDataSubrecord posData = new PosDataSubrecord();

            byte flags = data[firstByte + 12];
            posData.NTM = BitConverter.ToUInt32(data, firstByte + 0); 
            posData.Latitude = (float)BitConverter.ToUInt32(data, firstByte + 4) * 90 / 0xFFFFFFFF * ((((PosDataFlag)flags & PosDataFlag.LAHS) == PosDataFlag.LAHS) ? -1 : 1);
            posData.Longitude = (float)BitConverter.ToUInt32(data, firstByte + 8) * 180 / 0xFFFFFFFF * ((((PosDataFlag)flags & PosDataFlag.LOHS) == PosDataFlag.LOHS) ? -1 : 1);

            
            posData.Valid = ((PosDataFlag)flags & PosDataFlag.VLD) == PosDataFlag.VLD;
            posData.Actual = ((PosDataFlag)flags & PosDataFlag.BB) != PosDataFlag.BB;
            posData.Moving = ((PosDataFlag)flags & PosDataFlag.MV) == PosDataFlag.MV;

            // TODO: speed, direction, alt sign
            posData.Speed = BitConverter.ToUInt16(new byte[] { (byte)(data[firstByte + 14] & 0x3F), data[firstByte + 13] }, 0);
            posData.Direction = data[firstByte + 15];

            posData.Odometer = BitConverter.ToUInt32(new byte[] {0, data[firstByte + 15], data[firstByte + 16], data[firstByte + 17] }, 0);
            

            posData.DigitalInputs = data[firstByte + 18];
            posData.Source = data[firstByte + 19];

            // TODO: altitude (+20)

            subrecord.Data = posData;
        }

        private enum HeaderFlag : byte
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

        private enum RecordFlag : byte
        {
            bit0 = (1 << 0),
            bit1 = (1 << 1),
            bit2 = (1 << 2),
            bit3 = (1 << 3),
            bit4 = (1 << 4),
            bit5 = (1 << 5),
            bit6 = (1 << 6),
            bit7 = (1 << 7),

            OBFE = bit0,
            EVFE = bit1,
            TMFE = bit2,
            RPP = bit3 | bit4,
            GRP = bit5,
            RSOD = bit6,
            SSOD = bit7
        }

        private enum PosDataFlag : byte
        {
            bit0 = (1 << 0),
            bit1 = (1 << 1),
            bit2 = (1 << 2),
            bit3 = (1 << 3),
            bit4 = (1 << 4),
            bit5 = (1 << 5),
            bit6 = (1 << 6),
            bit7 = (1 << 7),

            VLD = bit0,
            FIX = bit1,
            CS = bit2,
            BB = bit3,
            MV = bit4,
            LAHS = bit5,
            LOHS = bit6,
            ALTE = bit7
        }
    }
}

