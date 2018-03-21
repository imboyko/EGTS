using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telematics.Networking;

namespace Telematics.Hosting
{
    static class ConsoleSelfHost
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage:\n\tTelematicsService.exe -port <port>");
                return;
            }

            if (!int.TryParse(args[1], out int port))
            {
                port = 6600;
                Console.WriteLine("Unable convert port value {0} to int. Using default 6600.");
            }
            Server server = new Server(port, new Telematics.BuisnesLogic.EgtsDataProcessor());
            
            Console.ReadKey();
        }
    }
}
