using Serilog;
using System;
using System.IO;

namespace EgtsTest
{
    static class Program
    {

        static void Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            if (args.Length != 1)
            { 
                Log.Fatal("Неверные аргументы запуска: {args}", args);
                Console.WriteLine("Формат строки запуска\n\t EgtsTest.exe <имя_файла>");
                return;
            }
                        
            byte[] rawData = null;
            var fileName = "Probes\\33.bin";

            Log.Information("Чтение данных из файла {FileName}", args[0]);
            try
            {
                rawData = File.ReadAllBytes(args[0]);
            }
            catch (Exception e)
            {
                Log.Error(e, "Что-то пошло не так... fileName = {fileName}", fileName);
            };

            if(rawData != null)
            {
                Log.Information("Прочитано из файла {BytesRead}", rawData.Length);

                var converter = new EGTS.Helpers.PacketConverter();

                try
                {
                    var packet = converter.FromBytes(rawData);
                }
                catch ( Exception e)
                {
                    Log.Error(e, "Ошибка при разборе двоичных данных.");
                }
                
            }
            else
            {
                Log.Warning("Данные из файла не прочитаны");
            }
        }
    }
}
