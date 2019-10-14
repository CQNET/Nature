using Nature.Client.Comm;
using System;
using System.Net.Sockets;

namespace Nature.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ////SocketClient调用
            //SocketClient socketClient = new SocketClient();
            //socketClient.ConnectServer();
            //socketClient.SendImage();
            //socketClient.SendMsg();

            //TcpClients调用
            TcpClients tcpClients = new TcpClients();

            #region 20190916
            //SocketClient socketClient = new SocketClient();

            //string msg = Console.ReadLine();
            //socketClient.StartServer(msg);

            ////20190916 code by ckjbug
            //Console.WriteLine("Client is running......");
            //TcpClient tcpClient;
            //for (int i = 0; i < 10000; i++)
            //{
            //    try
            //    {
            //        tcpClient = new TcpClient();
            //        tcpClient.Connect("localhost", 8500);
            //        Console.WriteLine("Server Connected！{0} --> {1}",
            //            tcpClient.Client.LocalEndPoint.ToString(), tcpClient.Client.RemoteEndPoint.ToString());
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.ToString());
            //        Console.Read();
            //        return;
            //    }
            //}
            //Console.Read();
            #endregion
        }
    }
}
