using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace Nature.Server
{
    public class TcpListeners
    {
        public TcpListeners()
        {
            RunServerAsync();
        }
        public async void RunServerAsync()
        {
            var listener = new TcpListener(IPAddress.Any,10001);
            listener.Start();
            try
            {
                while (true)
                {
                    await Accept(await listener.AcceptTcpClientAsync());
                }
            }
            finally
            {
                listener.Stop();
            }
        }
        const int packet_length = 2;
        public async Task Accept(TcpClient client)
        {
            Console.WriteLine("[Server] Client {0} was Connected", client.Client.RemoteEndPoint.ToString());

            await Task.Yield();
            try
            {
                using (client)
                {
                    using (NetworkStream n = client.GetStream())
                    {
                        byte[] data = new byte[packet_length];
                        int bytesRead = 0;
                        int chunkSize = 1;
                        while (bytesRead < data.Length && chunkSize > 0)
                        {
                            chunkSize = await n.ReadAsync(data,bytesRead,data.Length - bytesRead);
                            bytesRead += chunkSize;

                            string str = Encoding.Default.GetString(data);
                            Console.WriteLine("[Server] received : {0}",str);

                            while (true)
                            {
                                string send_str = Console.ReadLine();
                                byte[] send_data = Encoding.UTF8.GetBytes(send_str);
                                await n.WriteAsync(send_data, 0, send_data.Length);
                            }
                            //string send_str = "server_send_msg";
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }
}
