using System;
using System.IO;

namespace EgtsTest
{
    static class Program
    {

        static void Main(string[] args)
        {

            var converter = new EGTS.Helpers.PacketConverter();

            try
            {
                byte[] rawData = File.ReadAllBytes("Probes\\sevas.bin");
                //byte[] rawData = File.ReadAllBytes("Probes\\13.bin");
                var packet = converter.FromBytes(rawData);


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            };
        }
    }
}
