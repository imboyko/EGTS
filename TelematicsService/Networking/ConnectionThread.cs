//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.IO;
//using System.Text;
//using System.Threading;

//namespace Telematics.Networking
//{
//    public class ConnectionThread
//    {
//        public TcpListener ThreadListener { get; set; }
//        public Egts.EgtsProcessor Context { get; set; }
//        private TcpClient Client { get; set; }
//        private NetworkStream Stream { get; set; }
//        private BinaryWriter Writer { get; set; }
//        private BinaryReader Reader { get; set; }
//        private int BytesReceived { get; set; }
//        private static int connections = 0;

//        public void HandleConnection()
//        {
//            Connect();

//            Writer = new BinaryWriter(Client.GetStream());
//            Reader = new BinaryReader(Client.GetStream());

//            Process();

//            Disconnect();
//        }

//        public void SendMessage(byte[] msg)
//        {
//            Writer.Write(msg);
//            Writer.Flush();
//            Console.WriteLine("Sent to {0}: {1} bytes", Client.Client.RemoteEndPoint.ToString(), msg.Length);
//            Console.WriteLine();
//        }

//        public byte[] ReceiveMessage()
//        {
//            BytesReceived = Client.Available;
//            byte[] readBuffer = Reader.ReadBytes(BytesReceived);
//            if (BytesReceived > 0)
//            {
//                Console.WriteLine("Recieved from {0}: {1} bytes", Client.Client.RemoteEndPoint.ToString(), BytesReceived);
//            }
//            return readBuffer;
//        }

//        private void Connect()
//        {
//            Client = ThreadListener.AcceptTcpClient();
//            Stream = Client.GetStream();
//            connections++;
//            Console.WriteLine("New connection accepted: {0} active connections", connections.ToString());
//            Console.WriteLine();
//        }

//        private void Process()
//        {
//            while (Client.Connected)
//            {
//                byte[] recieveBuffer = ReceiveMessage();

//                if (BytesReceived != 0)
//                {
//                    try
//                    {
//                        byte[] sendBuffer = Context.ProcessData(recieveBuffer);
//                        SendMessage(sendBuffer);
//                    }
//                    catch(Exception e)
//                    {
//                        Console.ForegroundColor = ConsoleColor.Red;
//                        Console.WriteLine("\t[ FAIL ]");
//                        Console.WriteLine(e);
//                        Console.ForegroundColor = ConsoleColor.White;

//                        File.WriteAllBytes(
//                            path: $"dumps\\fail_{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.TimeOfDay.Hours}_{DateTime.Now.TimeOfDay.Minutes}_{DateTime.Now.TimeOfDay.Seconds}.bin",
//                            bytes: recieveBuffer);

//                        Disconnect();

//                    }
//                }
//                else
//                {
//                    Thread.Sleep(1000);
//                }
//            }
//        }

//        private void Disconnect()
//        {
//            Reader.Close();
//            Writer.Close();
//            Stream.Close();
//            Client.Close();
//            connections--;
//            Console.WriteLine("Client disconnected: {0} active connections", connections);
//            Console.WriteLine();
//        }
//    }
//}
