using System;
using System.Net.Sockets;
using System.IO;
using Serilog;

namespace Telematics.Networking
{
    public class ConnectionThread
    {
        public TcpListener ThreadListener { get; set; }
        public Egts.EgtsProcessor Context { get; set; }
        private TcpClient Client { get; set; }
        private NetworkStream Stream { get; set; }
        private int BytesReceived { get; set; }
        private static int _Connections = 0;

        public void HandleConnection()
        {
            Connect();
            Process();
            Disconnect();
        }

        private void Connect()
        {
            Client = ThreadListener.AcceptTcpClient();
            Client.ReceiveTimeout = 60000;  // 60 секунд таймаут получения
            Client.SendTimeout = 30000; // 30 секунд таймаут отправки

            Stream = Client.GetStream();
            _Connections++;

            Log.Information("Принято новое соединение от {clientip}. Активно: {ActiveConnections}", Client.Client.RemoteEndPoint.ToString(), _Connections);
        }
        private void Process()
        {
            while (Client.Connected)
            {
                byte[] recieveBuffer = new byte[0];

                try
                {
                    recieveBuffer = ReceiveMessage();

                    if (BytesReceived == 0)
                        // сокет закрыт.
                        break;

                    Log.Debug("Получено {bytesReceived} байт от {clientip}", recieveBuffer.Length, Client.Client.RemoteEndPoint.ToString());
                    Log.Verbose("{IncomingData}", recieveBuffer);

                    byte[] sendBuffer = Context.ProcessData(recieveBuffer);
                    SendMessage(sendBuffer);

                }
                catch (SocketException e)
                {
                    Log.Error(e, "Ошибка сети при получении данных от {clientip}", Client.Client.RemoteEndPoint.ToString());
                    break;  // выход из цикла, сокет закрыт
                }
                catch (IOException e)
                {
                    Log.Error(e, "Ошибка потока при получении данных от {clientip}", Client.Client.RemoteEndPoint.ToString());
                    break;  // выход из цикла, сокет закрыт
                }
                catch (Exception e)
                {
                    string dumpFileName = $"fail_{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}_{DateTime.Now.TimeOfDay.Hours}{DateTime.Now.TimeOfDay.Minutes}{DateTime.Now.TimeOfDay.Seconds}.bin";
                    Log.Error(e, "Ошибка получении данных от {clientip}. Дамп входящих данных записан в " + dumpFileName, Client.Client.RemoteEndPoint.ToString());
                    File.WriteAllBytes($"logs\\dumps\\{dumpFileName}", recieveBuffer);
                };
            }

            Log.Debug(
                "Завершение взаимодействия с {clientip}: Client.Connected={ClientConnected}, Stream.CanRead={StreamCanRead}, BytesReceived={BytesReceived}",
                Client.Client.RemoteEndPoint.ToString(), Client.Connected, Stream.CanRead, BytesReceived);

        }
        private void SendMessage(byte[] msg)
        {
            Stream.Write(msg, 0, msg.Length);

            Log.Debug("Отправлено {bytesSent} байт для {clientip}", msg.Length, Client.Client.RemoteEndPoint.ToString());
            Log.Verbose("{ResponseData}", msg);

        }
        private byte[] ReceiveMessage()
        {
            BytesReceived = 0;

            // Если сокет закрыт, то вернем пустой массив.
            if (!Stream.CanRead)
                return new byte[0];


            var mStream = new MemoryStream();

            var buffer = new byte[Client.ReceiveBufferSize];
            int numberOfBytesRead = 0;

            do
            {
                numberOfBytesRead = Stream.Read(buffer, 0, buffer.Length);
                mStream.Write(buffer, 0, numberOfBytesRead);

                BytesReceived += numberOfBytesRead;

            } while (Stream.DataAvailable);

            return mStream.GetBuffer();
        }
        private void Disconnect()
        {
            _Connections--;
            Log.Warning("Разрыв соединения c {clientip}. Активных подключений: {ActiveConnections}", Client.Client.RemoteEndPoint.ToString(), _Connections);
            Client.Close();
        }
    }
}
