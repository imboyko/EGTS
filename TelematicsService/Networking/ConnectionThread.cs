using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;
using Serilog;

namespace Telematics.Networking
{
    public class ConnectionThread
    {
        public TcpListener ThreadListener { get; set; }
        public Egts.EgtsProcessor Context { get; set; }
        private TcpClient Client { get; set; }
        private NetworkStream Stream { get; set; }
        private BinaryWriter Writer { get; set; }
        private BinaryReader Reader { get; set; }
        private int BytesReceived { get; set; }
        private static int connections = 0;

        public void HandleConnection()
        {
            Connect();

            Writer = new BinaryWriter(Client.GetStream());
            Reader = new BinaryReader(Client.GetStream());

            Process();

            Disconnect();
        }

        public void SendMessage(byte[] msg)
        {
            Writer.Write(msg);
            Writer.Flush();

            Log.Verbose("{ResponseData}", msg);
            Log.Information("Отправлено {bytesSent} байт для {clientip}", msg.Length, Client.Client.RemoteEndPoint.ToString());
            
        }

        public byte[] ReceiveMessage()
        {
            BytesReceived = Client.Available;
            byte[] readBuffer = Reader.ReadBytes(BytesReceived);
            if (BytesReceived > 0)
            {
                Log.Information("Получено {bytesReceived} байт от {clientip}", BytesReceived, Client.Client.RemoteEndPoint.ToString());
                Log.Verbose("{IncomingData}", readBuffer);
            }
            
            return readBuffer;
        }

        private void Connect()
        {
            Client = ThreadListener.AcceptTcpClient();
            Stream = Client.GetStream();
            connections++;

            Log.Information("Принято новое соединение. Активных подключений: {ActiveConnections}", connections);
        }

        private void Process()
        {
            while (Client.Connected)
            {
                byte[] recieveBuffer = ReceiveMessage();

                if (BytesReceived != 0)
                {
                    try
                    {
                        byte[] sendBuffer = Context.ProcessData(recieveBuffer);
                        SendMessage(sendBuffer);
                    }
                    catch(Exception e)
                    {
                        Log.Error(e, "Ошибка обработки данных. Входящие данные:{recieveBuffer}, Байт принято {BytesReceived}", recieveBuffer, BytesReceived);
                        File.WriteAllBytes(
                            path: $"logs\\dumps\\fail_{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}_{DateTime.Now.TimeOfDay.Hours}{DateTime.Now.TimeOfDay.Minutes}{DateTime.Now.TimeOfDay.Seconds}.bin",
                            bytes: recieveBuffer);

                        Disconnect();

                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private void Disconnect()
        {
            Reader.Close();
            Writer.Close();
            Stream.Close();
            Client.Close();
            connections--;
            Log.Warning("Разрыв соединения {clientip}. Активных подключений: {ActiveConnections}", Client.Client.RemoteEndPoint.ToString(), connections);
        }
    }
}
