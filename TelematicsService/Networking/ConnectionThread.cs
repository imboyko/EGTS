using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;

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
            Console.WriteLine("Sent to {0}: {1} bytes", Client.Client.RemoteEndPoint.ToString(), msg.Length);
            Console.WriteLine();
        }

        public byte[] ReceiveMessage()
        {
            BytesReceived = Client.Available;
            byte[] readBuffer = Reader.ReadBytes(BytesReceived);
            if (BytesReceived > 0)
            {
                Console.WriteLine("Recieved from {0}: {1} bytes", Client.Client.RemoteEndPoint.ToString(), BytesReceived);
            }
            return readBuffer;
        }

        private void Connect()
        {
            Client = ThreadListener.AcceptTcpClient();
            Stream = Client.GetStream();
            connections++;
            Console.WriteLine("New connection accepted: {0} active connections", connections.ToString());
            Console.WriteLine();
        }

        private void Process()
        {
            while (Client.Connected)
            {
                byte[] recieveBuffer = ReceiveMessage();

                if (BytesReceived != 0)
                {
                    DateTime now = DateTime.Now;
                    File.WriteAllBytes(String.Format("dumps\\{0}-in.bin", now.TimeOfDay.Ticks), recieveBuffer);
                    byte[] sendBuffer = Context.ProcessData(recieveBuffer);
                    File.WriteAllBytes(String.Format("dumps\\{0}-out.bin", now.TimeOfDay.Ticks), sendBuffer);
                    SendMessage(sendBuffer);
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
            Console.WriteLine("Client disconnected: {0} active connections", connections);
            Console.WriteLine();
        }
    }
}
