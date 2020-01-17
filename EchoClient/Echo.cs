using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EchoClient
{
    public static class Echo
    {
        /// <summary>
        /// Opens a new connection to an echo server, sends
        /// "Hello world!", and then closes the connection.
        /// </summary>
        /// <param name="hostname">IP (or DNS name) of the echo server</param>
        /// <param name="port">Port used by the echo server.</param>
        public static void Test(string hostname, int port)
        {
            string hello = "Hello world!";
            using (var tcpClient = new TcpClient { NoDelay = true })
            {
                tcpClient.Connect(hostname, port);

                var tcpStream = tcpClient.GetStream();

                // Send some bytes:
                byte[] payload = Encoding.UTF8.GetBytes(hello);
                tcpStream.Write(payload, offset: 0, size: payload.Length);

                // Read response:
                byte[] responseBuffer = new byte[payload.Length];
                int bytesRemaining = responseBuffer.Length;

                do
                {
                    int bufferOffset = responseBuffer.Length - bytesRemaining;
                    int bytesRead = tcpStream.Read(responseBuffer, bufferOffset, bytesRemaining);
                    bytesRemaining -= bytesRead;
                    if (bytesRead == 0 && bytesRemaining > 0)
                    {
                        // Server was probably killed
                        throw new IOException("No response from echo server.");
                    }
                } while (bytesRemaining > 0);


                // Check response:
                string echoedString = Encoding.UTF8.GetString(responseBuffer);
                if (echoedString != hello)
                    throw new Exception("Unexpected response from echo server.");
            }

        }
    }
}
