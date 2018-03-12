using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGTS.ServiceLayer;

namespace EGTS.Parsers
{
    class ServiceDataParser
    {
        public List<ServiceDataSubrecord> Parse(byte[] data, ushort offset, ushort dataLength)
        {
            List<ServiceDataSubrecord> result = new List<ServiceDataSubrecord>();

            ushort bytesRead = 0;

            while (bytesRead != dataLength)
            {
                int localOffset = 0;
                ServiceDataSubrecord subrecord = new ServiceDataSubrecord();

                subrecord.Type = (SubrecordTypes)data[offset + localOffset];
                localOffset += 1;

                subrecord.Length = BitConverter.ToUInt16(data, offset + localOffset);
                localOffset += 2;

                // TODO: subrecord data

                offset = (ushort)(offset + localOffset + subrecord.Length);
                bytesRead = (ushort)(bytesRead + localOffset + subrecord.Length);

                result.Add(subrecord);
            }

            return result;
        }
    }
}
