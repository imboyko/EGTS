using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGTS.TransportLayer;

namespace EGTS.Parsers
{
    internal interface IServiceFrameParser
    {
        ServiceFrameData Parse(byte[] data, ushort offset, ushort dataLength);
    }
}
