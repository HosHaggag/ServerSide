// See https://aka.ms/new-console-template for more information
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

// Socket Listener acts as a server and listens to the incoming
// messages on the specified port and protocol.
public class SocketListener
{
    Socket client;
   static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            int recv;

            byte[] data = new byte[1024];

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"),9050);

            Socket newsock = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

            newsock.Bind(ipep);

            newsock.Listen(10);

            Console.WriteLine("Waiting for a client...");

            Socket client = newsock.Accept();

            IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;

            Console.WriteLine("Connected with {0} at port {1}",clientep.Address,clientep.Port);

            string welcome = "Welcome to my test server";

            data = Encoding.UTF8.GetBytes(welcome);

            client.Send(data,data.Length,SocketFlags.None);

            Thread t = new Thread(() => ReceiveMessage(client));
            t.Start();

            Thread t1 = new Thread(() => SendMessage(client));
            t1.Start();





       

            // Console.WriteLine("Disconnected from {0}", clientep.Address);

            // client.Close();

            // newsock.Close();

            // Console.ReadLine();

        }

        public static  void ReceiveMessage(Socket socket)
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int length = socket.Receive(buffer);
                string message = Encoding.UTF8.GetString(buffer, 0, length);
                 if (message == "exit")

                    break;
                Console.WriteLine("Client: "+message);
            }
           
        }

        public static void SendMessage(Socket socket)
        {
            while (true)
            {
                string message = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                if(message=="exit")
                {
                    Console.WriteLine("Disconnected from {0}", socket.RemoteEndPoint);

                    socket.Close();

                    break;
                }
                socket.Send(buffer);
            }
        }
       
}
