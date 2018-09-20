//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.IO;
//using System.Threading;
//using System.Text;

//namespace Telematics.Networking
//{
//    public class Server
//    {
//        TcpListener listener;
//        int Port;
//        Egts.Processing.IEgtsProcessor ContextProcessor;

//        public Server(int port, Egts.Processing.IEgtsProcessor contextProcessor)
//        {
//            ContextProcessor = contextProcessor;

//            Port = port;

//            Start();
//            HandleRequests();
//            Stop();
//        }

//        public void Start()
//        {
//            listener = new TcpListener(IPAddress.Any, Port);
//            listener.Start();
//            Console.WriteLine(String.Format("Listening to {0}:{1}", IPAddress.Any.ToString(), Port));
//        }
//        public void Stop()
//        {
//            listener.Stop();
//        }

//        public void HandleRequests()
//        {
//            while (true)
//            {
//                while (!listener.Pending())
//                {
//                    Thread.Sleep(1000);
//                }

//                ConnectionThread newConnection = new ConnectionThread();
//                newConnection.ThreadListener = this.listener;
//                newConnection.Context = new Egts.EgtsProcessor(ContextProcessor);
//                Thread newThread = new Thread(new ThreadStart(newConnection.HandleConnection));
//                newThread.Start();
//            }
//        }

//    }
//}
