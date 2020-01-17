using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EchoServer
{
    class Program
    {
        static TcpListener server = null;
        public static void Main(string[] args)
        {
            int port = int.Parse(ConfigurationManager.AppSettings["port"]);
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            StartListener();
        }

        public static void StartListener()
        {
            try
            {
                Console.WriteLine("Waiting for client connections...");
                while (true)
                {
                    
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine($"Connected to client {client.Client.RemoteEndPoint}");

                    Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                    t.Start(client);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                server.Stop();
            }
        }

        public static void HandleClient(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();

            byte[] receiveBuffer = new byte[512];
            int bytesRead;

            try
            {
                // Receive and echo until the peer shuts down the connection:
                while ((bytesRead = stream.Read(receiveBuffer, 0, receiveBuffer.Length)) != 0)
                {
                    stream.Write(receiveBuffer, 0, bytesRead);
                    Console.WriteLine($"Echoed {bytesRead} bytes.");
                }
                Console.WriteLine($"Client {client.Client.RemoteEndPoint} disconnected.");
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine($"Connection to client {client.Client.RemoteEndPoint} was terminated.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
                
            }
            client.Close();
        }
    }
}
