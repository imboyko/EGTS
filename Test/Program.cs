using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] rawData = File.ReadAllBytes("Probes\\sevas.bin");

            EGTS.PacketBuilder builder = new EGTS.PacketBuilder();
            builder.BuildFromBytes(rawData);

            EGTS.Packet packet = builder.Packet;

            


            Console.ReadKey();

        }
    }
}
