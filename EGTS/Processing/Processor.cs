using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGTS
{
    class Processor
    {
        private byte[] data;

        public void ProcessData(byte[] data)
        {

            PacketBuilder builder = new PacketBuilder();
            builder.BuildFromBytes(data);

            Data.Packet inPacket = builder.Packet;


            Validator.CheckPacket(inPacket, data);

        }
    }
}
