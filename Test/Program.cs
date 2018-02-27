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
            byte[] rawData = File.ReadAllBytes("Probes\\16.bin");

            EGTS.Packet packet = EGTS.Parser.ByteToPacket(rawData);
            ushort length = (ushort)(packet.Header.HeaderLength - 1);
            byte crc8 = EGTS.Validator.Crc8(rawData, length);

            Console.WriteLine("Packet CRC = {0}\nValidator CRC = {1}", packet.Header.CRC, crc8);
            Console.ReadKey();

        }
    }
}
