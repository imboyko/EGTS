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
            try
            {
                byte[] rawData = File.ReadAllBytes("Probes\\fail.bin");
                //byte[] rawData = File.ReadAllBytes("Probes\\13.bin");
                rawData = egtsProcessor.ProcessData(rawData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            };
        }
    }
}
