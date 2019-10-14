using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Nature.Client.Comm
{
    public class TcpClients
    {
        public TcpClients()
        {
            RunAsTcpClient();
        }
        
        public async void RunAsTcpClient()
        {
            using (var tcpClient = new TcpClient())
            {
                Console.WriteLine("[Client] Connecting to server");
                await tcpClient.ConnectAsync("127.0.0.1",10001);
                Console.WriteLine("[Client] Connected to server");
                using (var networkStream = tcpClient.GetStream())
                {
                    string ClientRequestString = Console.ReadLine();
                    byte[] ClientRequestBytes = Encoding.ASCII.GetBytes(ClientRequestString);
                    Console.WriteLine("[Client] Writeing request {0}",ClientRequestString);
                    await networkStream.WriteAsync(ClientRequestBytes,0,ClientRequestBytes.Length);

                    var buffer = new byte[4096];
                    var byteCount = await networkStream.ReadAsync(buffer,0,buffer.Length);
                    var response = Encoding.ASCII.GetString(buffer,0,byteCount);
                    Console.WriteLine("[Client] Server response wa {0}",response);
                }
            }
        }
    }
}
