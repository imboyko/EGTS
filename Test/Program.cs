using Egts;
using System;
using System.IO;

namespace EgtsTest
{
    static class Program
    {
        static void Main(string[] args)
        {
            EgtsProcessor egtsProcessor = new EgtsProcessor(new ToConsoleProcessor());
            for (int i = 1; i <= 9; i++)
            {
                byte[] rawData = File.ReadAllBytes(String.Format("Probes\\{0}-res.bin", i));

                rawData = egtsProcessor.ProcessData(rawData);

                File.WriteAllBytes("response.bin", rawData);

                Console.ReadKey();
            }
            Console.ReadKey();
        }
    }
}
