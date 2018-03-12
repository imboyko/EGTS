using System;
using EGTS.ServiceLayer;
using EGTS.TransportLayer;

namespace EGTS.Parsers
{
    internal class AppdataParser : IServiceFrameParser
    {
        public ServiceFrameData Parse(byte[] data, ushort offset, ushort dataLength)
        {
            AppdataPacket result = new AppdataPacket();
            result.ServiceDataRecords = new System.Collections.Generic.List<ServiceDataRecord>();

            // TODO: extrect next code to parser class
            ushort bytesRead = 0;

            while (bytesRead != dataLength)
            {
                int localOffset = 0;
                ServiceDataRecord serviceDataRecord = new ServiceDataRecord();

                serviceDataRecord.RecordLength = BitConverter.ToUInt16(data, offset + localOffset);
                localOffset += 2;

                serviceDataRecord.RecordNumber = BitConverter.ToUInt16(data, offset + localOffset);
                localOffset += 2;

                byte flags = data[offset + localOffset];
                localOffset += 1;

                if ((flags & (byte)(Bitsets.OBFE)) == (byte)Bitsets.OBFE)
                {
                    serviceDataRecord.ObjectID = BitConverter.ToUInt32(data, offset + localOffset);
                    localOffset += 4;
                }

                if ((flags & (byte)(Bitsets.EVFE)) == (byte)Bitsets.EVFE)
                {
                    serviceDataRecord.EventID = BitConverter.ToUInt32(data, offset + localOffset);
                    localOffset += 4;
                }

                if ((flags & (byte)(Bitsets.TMFE)) == (byte)Bitsets.TMFE)
                {
                    serviceDataRecord.TM = BitConverter.ToUInt32(data, offset + localOffset);
                    localOffset += 4;
                }

                // TODO: SSOD, RSOD, RPP

                serviceDataRecord.SourceService = (Services)data[offset + localOffset];
                localOffset += 1;

                serviceDataRecord.RecipientService = (Services)data[offset + localOffset];
                localOffset += 1;

                // TODO: service data parsing
                ServiceDataParser serviceDataParser = new ServiceDataParser();
                serviceDataRecord.RecordData = serviceDataParser.Parse(data, (ushort)(offset + localOffset), serviceDataRecord.RecordLength);

                offset = (ushort)(offset + localOffset + serviceDataRecord.RecordLength);
                bytesRead = (ushort)(bytesRead + localOffset + serviceDataRecord.RecordLength);

                result.ServiceDataRecords.Add(serviceDataRecord);
            }

            return result;
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

            OBFE = bit0,
            EVFE = bit1,
            TMFE = bit2,
            RPP = bit3 | bit4,
            GRP = bit5,
            RSOD = bit6,
            SSOD = bit7
        }
    }
}