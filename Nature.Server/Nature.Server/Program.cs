using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Nature.Server
{
    public class Program
    {

        static void Main(string[] args)
        {
            ////SocketServer调用
            //SocketServer socket = new SocketServer();
            //socket.CreateSocket();
            ////socket.SendMsg();
            //socket.SendMsg2All();
            ////string ip = Console.ReadLine();
            ////socket.SendMsg2Alone(ip);
            //Console.ReadKey();

            ////TcpHelper调用
            //TcpHelper.StartListener();
            //TcpHelper.ConnectAsTcpClient();
            //Console.ReadLine();

            TcpListeners tcpListeners = new TcpListeners();
            Console.ReadKey();
        }
        #region 20190916
        //static void Main(string[] args)
        //{
        //SocketServer socketServer = new SocketServer();
        //socketServer.StartServer();
        //}

        ////20190916 code by ckjbug
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Server is running ... ");
        //    IPAddress ip = new IPAddress(new byte[] { 127, 0, 0, 1 });
        //    TcpListener listener = new TcpListener(ip, 8500);
        //    listener.Start();           // 开始侦听
        //    Console.WriteLine("Start Listening ...");
        //    while (true)
        //    {
        //        TcpClient remoteClient = listener.AcceptTcpClient();//接受挂起的连接请求
        //        Console.WriteLine("Client Connected！{0} <-- {1}",
        //            remoteClient.Client.LocalEndPoint.ToString(), remoteClient.Client.RemoteEndPoint.ToString());
        //    }
        //}




        //private static byte[] result = new byte[1024];
        //private const int port = 8888;
        //private static string IpStr = "127.0.0.1";
        //private static TcpListener listener;
        //public static List<TcpClient> clients = new List<TcpClient>();

        //static void Main(string[] args)
        //{

        //    //获取本机IPv4地址
        //    String localHostName = Dns.GetHostName();
        //    string localIP = string.Empty;
        //    IPHostEntry host = Dns.GetHostEntry(localHostName);
        //    foreach (IPAddress iPAddress in host.AddressList)
        //    {
        //        //判断是否为IPV4
        //        if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
        //        {
        //            localIP = iPAddress.ToString();
        //        }
        //    }
        //    IPAddress ip = IPAddress.Parse(localIP);


        //    //IPAddress ip = IPAddress.Parse(IpStr);
        //    IPEndPoint ip_end_point = new IPEndPoint(ip, port);
        //    listener = new TcpListener(ip_end_point);
        //    listener.Start();
        //    Console.WriteLine("启动监听成功");

        //    //异步接收 递归循环接收多个客户端
        //    listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpclient), listener);
        //    Console.ReadKey();

        //}
        //private static void DoAcceptTcpclient(IAsyncResult State)
        //{
        //    /* 处理多个客户端接入*/
        //    TcpListener listener = (TcpListener)State.AsyncState;

        //    TcpClient client = listener.EndAcceptTcpClient(State);

        //    clients.Add(client);

        //    Console.WriteLine("\n收到新客户端:{0}", client.Client.RemoteEndPoint.ToString());
        //    //开启线程用来持续收来自客户端的数据
        //    Thread myThread = new Thread(new ParameterizedThreadStart(PrintReceiveMsg));
        //    myThread.Start(client);

        //    //开启线程用来向客户端发送数据命令
        //    foreach (var item in clients)
        //    {
        //        Thread sendThread = new Thread(new ParameterizedThreadStart(SendMsg2Client));
        //        sendThread.Start(item);
        //    }

        //    listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpclient), listener);
        //}

        //private static void PrintReceiveMsg(object reciveClient)
        //{
        //    /* 用来打印接收的消息*/
        //    TcpClient client = reciveClient as TcpClient;
        //    if (client == null)
        //    {
        //        Console.WriteLine("client error");
        //        return;
        //    }
        //    while (true)
        //    {
        //        try
        //        {
        //            NetworkStream stream = client.GetStream();
        //            int num = stream.Read(result, 0, result.Length); //将数据读到result中，并返回字符长度                  
        //            if (num != 0)
        //            {
        //                string str = Encoding.UTF8.GetString(result, 0, num);//把字节数组中流存储的数据以字符串方式赋值给str
        //                //在服务器显示收到的数据
        //                Console.WriteLine("From: " + client.Client.RemoteEndPoint.ToString() + " : " + str);


        //                //服务器收到消息后并会给客户端一个消息。
        //                string msg = "服务器已收到您的消息[" + str + "]";
        //                result = Encoding.UTF8.GetBytes(msg);
        //                stream = client.GetStream();
        //                stream.Write(result, 0, result.Length);
        //                stream.Flush();
        //            }
        //            else
        //            {   //这里需要注意 当num=0时表明客户端已经断开连接，需要结束循环，不然会死循环一直卡住
        //                Console.WriteLine("客户端关闭");
        //                break;
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            clients.Remove(client);
        //            Console.WriteLine("error:" + e.ToString());
        //            break;
        //        }
        //    }
        //}

        //private static void SendMsg2Client(object reciveClient)
        //{
        //    /* 向客户端发送命令消息*/
        //    TcpClient client = reciveClient as TcpClient;
        //    if (client == null)
        //    {
        //        Console.WriteLine("client error");
        //        return;
        //    }
        //    NetworkStream stream = client.GetStream();
        //    Console.Write("请输入您要发送的数据: ");
        //    string msg = Console.ReadLine();

        //    byte[] data = Encoding.UTF8.GetBytes(msg);
        //    stream.Write(data, 0, data.Length);
        //}

        #endregion
    }
}
