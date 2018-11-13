using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telematics.Networking;
using Serilog;


namespace Telematics.Hosting
{
    static class ConsoleSelfHost
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .WriteTo.RollingFile("logs\\log-{Date}.txt")
                .CreateLogger();

            if (args.Length == 0)
            {
                Log.Fatal("Неверный формат запуска.");
                Console.WriteLine("Использование:\nTelematicsService.exe -port <port>");
                return;
            }

            if (!int.TryParse(args[1], out int port))
            {
                Log.Warning("Неверный номер порта {port}. Будет использоваться 6600", args[1]);
                port = 6600;
            }
            Server server = new Server(port, new Telematics.BuisnesLogic.EgtsDataProcessor());
            
            Console.ReadKey();
        }
    }
}
