using Egts.Data;
using Egts.Data.ServiceLayer;
using Egts.Data.TransportLayer;
using System;
using System.Collections.Generic;

namespace Egts
{
    public class EgtsPacketBuilder
    {
        private EgtsPacket Packet;
        private delegate void ServiceFrameParserDel(ref byte[] data);
        private delegate void SubrecordParserDel(ref byte[] data, int firstByte, ref ServiceDataSubrecord subrecord);
        private Dictionary<PacketType, ServiceFrameParserDel> serviceFrameParsers;
        private Dictionary<SubrecordType, SubrecordParserDel> subrecordParsers;

        public EgtsPacketBuilder()
        {
            Packet = new EgtsPacket();

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

        public EgtsPacket GetPacket()
        {
            return Packet;
        }

        public void BuildFromProcessingResult(ProcessingResult result)
        {

            ResponsePacket response = new ResponsePacket
            {
                ResponseTo = result.PacketId,
                ResultCode = result.Result
            };

            foreach (ProcessingResult.RecordResult recResult in result.RecResults)
            {
                // subrecord data
                SubrecordResponse subrecord = new SubrecordResponse
                {
                    ConfirmedRecord = recResult.Record.RecordNumber,
                    Result = (byte)recResult.Result
                };

                // record data
                ServiceDataSubrecord recordData = new ServiceDataSubrecord
                {
                    Data = subrecord,
                    Length = (ushort)subrecord.GetBytes().Length,
                    Type = SubrecordType.EGTS_SR_RECORD_RESPONSE
                };

                // Record
                ServiceDataRecord record = new ServiceDataRecord
                {
                    EventFieldExists = false,
                    ObjectFieldExists = recResult.Record.ObjectFieldExists,
                    ObjectID = recResult.Record.ObjectID,
                    ProcessingPriority = recResult.Record.ProcessingPriority,
                    RecipientService = recResult.Record.SourceService,
                    RecipientServiceOnDevice = recResult.Record.SourceServiceOnDevice,
                    RecordNumber = recResult.Record.RecordNumber,
                    SourceService = recResult.Record.RecipientService,
                    SourceServiceOnDevice = recResult.Record.RecipientServiceOnDevice,
                    TimeFieldExists = false,
                    RecordLength = (ushort)recordData.GetBytes().Length, // only one subrecord ib RecordData
                };
                record.RecordData.Add(recordData);

                response.ServiceDataRecords.Add(record);
            }

            TransportHeader header = new TransportHeader
            {
                Compressed = false,
                HeaderEncoding = 0,
                PID = result.PacketId,
                Prefix = 0,
                Priority = Priority.Highest,
                ProtocolVersion = 1,
                Route = false,
                SecurityKeyId = 0,
                Type = PacketType.EGTS_PT_RESPONSE,
                FrameDataLength = (ushort)response.GetBytes().Length,
                HeaderLength = 11   // TODO: calculate HeaderLength
            };

            header.CRC = Validator.GetCrc8(header.GetBytes(), (ushort)(header.HeaderLength - 1));

            Packet = new EgtsPacket
            {
                Header = header,
                ServiceFrameData = response,
                CRC = Validator.GetCrc16(response.GetBytes(), 0, header.FrameDataLength)
            };
        }

        #region Parsing
        private void ParseHeader(ref byte[] data)
        {
            TransportHeader header = new TransportHeader
            {
                ProtocolVersion = data[0],
                SecurityKeyId = data[1],
                Prefix = (byte)((data[2] & (byte)HeaderFlags.PRF) >> 6),
                Route = (data[2] & (byte)HeaderFlags.RTE) == (byte)HeaderFlags.RTE,
                Compressed = (data[2] & (byte)HeaderFlags.CMP) == (byte)HeaderFlags.CMP,
                Priority = (Priority)(data[2] & (byte)HeaderFlags.PR),
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
            if (Packet.Header.FrameDataLength == 0)
            {
                return;
            }

            serviceFrameParsers.TryGetValue(Packet.Header.Type, out ServiceFrameParserDel parser);
            parser?.Invoke(ref data);
        }

        private void ParseCRC(ref byte[] data)
        {
            if (Packet.Header.FrameDataLength == 0)
            {
                return;
            }

            int packetLength = Packet.Header.HeaderLength + Packet.Header.FrameDataLength;
            if (packetLength < data.Length)
            {
                Packet.CRC = BitConverter.ToUInt16(data, packetLength);
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

                record.SourceServiceOnDevice = (flags & (byte)(RecordFlags.SSOD)) == (byte)RecordFlags.SSOD;
                record.RecipientServiceOnDevice = (flags & (byte)(RecordFlags.RSOD)) == (byte)RecordFlags.RSOD;
                record.Group = (flags & (byte)(RecordFlags.GRP)) == (byte)RecordFlags.GRP;
                record.ProcessingPriority = (Priority)((flags & (byte)(RecordFlags.RPP)) >> 3);
                record.TimeFieldExists = (flags & (byte)(RecordFlags.TMFE)) == (byte)RecordFlags.TMFE;
                record.EventFieldExists = (flags & (byte)(RecordFlags.EVFE)) == (byte)RecordFlags.EVFE;
                record.ObjectFieldExists = (flags & (byte)(RecordFlags.OBFE)) == (byte)RecordFlags.OBFE;
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
            Data.ServiceLayer.TeledataService.PosDataSubrecord posData = new Data.ServiceLayer.TeledataService.PosDataSubrecord();

            byte flags = data[firstByte + 12];  // 12
            posData.NTM = BitConverter.ToUInt32(data, firstByte + 0);   // 0-3
            posData.Latitude = (float)BitConverter.ToUInt32(data, firstByte + 4) * 90 / 0xFFFFFFFF * ((((PosDataFlags)flags & PosDataFlags.LAHS) == PosDataFlags.LAHS) ? -1 : 1);   // 4-7
            posData.Longitude = (float)BitConverter.ToUInt32(data, firstByte + 8) * 180 / 0xFFFFFFFF * ((((PosDataFlags)flags & PosDataFlags.LOHS) == PosDataFlags.LOHS) ? -1 : 1); // 8-11

            posData.Valid = ((PosDataFlags)flags & PosDataFlags.VLD) == PosDataFlags.VLD;
            posData.Actual = ((PosDataFlags)flags & PosDataFlags.BB) != PosDataFlags.BB;
            posData.Moving = ((PosDataFlags)flags & PosDataFlags.MV) == PosDataFlags.MV;

            posData.Speed = BitConverter.ToUInt16(new byte[] { data[firstByte + 13], (byte)(data[firstByte + 14] & 0x3F) }, 0) / 10;// * 1.60934F; // 13-14
            posData.Direction = BitConverter.ToUInt16(new byte[] { data[firstByte + 15], (byte)((data[firstByte + 14] & 0x80)>> 7)}, 0); // 15

            posData.Odometer = (float)BitConverter.ToUInt32(new byte[] { data[firstByte + 16], data[firstByte + 17], data[firstByte + 18], 0 }, 0) / 10;    // 16-18

            posData.DigitalInputs = data[firstByte + 19];   // 19
            posData.Source = data[firstByte + 20];  // 20

            if (((PosDataFlags)flags & PosDataFlags.ALTE) == PosDataFlags.ALTE)
            {
                posData.Altitude = BitConverter.ToInt32(new byte[] { data[firstByte + 21], data[firstByte + 22], data[firstByte + 23], 0 }, 0) * (((data[firstByte + 14] & 0x40) == 0x40) ? -1 : 1);    //21-23
            }

            subrecord.Data = posData;
        }
        #endregion

    }
}

