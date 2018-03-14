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
            ushort length = (ushort)(packet.Header.HeaderLength - 1);
            byte crc8 = EGTS.Validator.Crc8(rawData, length);

            Console.WriteLine("Packet CRC = {0}\nValidator CRC = {1}", packet.Header.CRC, crc8);

            Console.WriteLine(0xFFFFFFFF);
            Console.ReadKey();

        }
    }
}
