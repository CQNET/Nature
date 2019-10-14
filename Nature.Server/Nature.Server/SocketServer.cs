using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nature.Server
{
    public class SocketServer
    {
        public void CreateSocket()
        {
            try
            {
                //创建负责监听客户端的Socket
                Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Any;
                //创建端口号对象
                IPEndPoint point = new IPEndPoint(ip, 50000);
                //绑定端口号对象，开始监听
                socketWatch.Bind(point);
                ShowMsg("监听成功！");
                socketWatch.Listen(100);
                //Task<Socket> socket = socketWatch.AcceptAsync();
                Thread thread = new Thread(Listen);
                thread.IsBackground = true;
                thread.Start(socketWatch);

                Console.ReadKey();
            }
            catch
            { }
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Exception Log:" + ex.ToString());
            //}
        }

        void ShowMsg(string str)
        {
            Console.WriteLine(str + "\r\n");
            Console.Title = "服务器";
        }

        Socket socketSend;
        //将远程连接的客户端IP地址和Socket存入集合中
        Dictionary<string, Socket> dicSocket = new Dictionary<string, Socket>();

        void Listen(object o)
        {
            while (true)
            {
                try
                {
                    Socket socketWatch = o as Socket;
                    //等待客户端的连接，并创建一个负责通信的Socket
                    socketSend = socketWatch.Accept();
                    //socketWatch.AcceptAsync();
                    dicSocket.Add(socketSend.RemoteEndPoint.ToString(),socketSend);
                    ShowMsg(socketSend.RemoteEndPoint.ToString() + ":" + "连接成功");
                    //开启一个新线程，不停地接收客户端发来的消息
                    Thread thread = new Thread(ReciveMsg);
                    thread.IsBackground = true;
                    thread.Start(socketSend);
                }
                catch
                { }
                //catch ( Exception ex)
                //{
                //    Console.WriteLine("Exception Log:" + ex.ToString());
                //}
            }
        }
        //服务器端不停地接收客户端发过来的消息
        void ReciveMsg(object o)
        {
            Socket socketSend = o as Socket;
            while (true)
            {
                try
                {
                    //客户端连接成功后，服务器应该接收客户端发来的消息
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    //实际接收的有效字节数
                    int r = socketSend.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }

                    //如果第一个字节为0则表示为字符（心跳包）
                    if (buffer[0] == 0)
                    {
                        string str = Encoding.UTF8.GetString(buffer, 1, r-1);
                        ShowMsg(socketSend.RemoteEndPoint + ":" + str);
                    }
                    //如果第一个字节为1则表示为截屏图片
                    if (buffer[0] == 1)
                    {
                        string url = @"C:\Users\Enz\Desktop\1.png";
                        using (FileStream fsWrite = new FileStream(url, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            fsWrite.Write(buffer, 1, r - 1);
                        }
                        break;
                    }
                }
                catch
                { }
                //catch (Exception ex)
                //{
                //    Console.WriteLine("Exception Log:" + ex.ToString());
                //}
            }
        }

        //服务器给客户端发消息
        public void SendMsg()
        {
            while (true)
            {
                string msg = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(msg);
                socketSend.Send(buffer);
            }
        }
        //给指定IP的客户端发送消息
        public void SendMsg2Alone(string ip)
        {
            try
            {
                string msg = "00";
                byte[] buffer = Encoding.UTF8.GetBytes(msg);
                dicSocket[ip].Send(buffer);
            }
            catch
            {
            }
        }
        //给所有的客户端发送消息
        public void SendMsg2All()
        {
            try
            {
                foreach (var item in dicSocket)
                {
                    string msg = "\r\n" + "shutdown -s -t 00" +"\r\n";
                    byte[] buffer = Encoding.UTF8.GetBytes(msg);
                    dicSocket[item.Key].Send(buffer);
                }
            }
            catch
            {
            }
        }
    }
    #region SocketServer
    /*
    public class SocketServer
    {
        // 创建一个和客户端通信的套接字
        static Socket socketwatch = null;
        //定义一个集合，存储客户端信息
        static Dictionary<string, Socket> clientConnectionItems = new Dictionary<string, Socket> { };

        public void StartServer()
        {
            //定义一个套接字用于监听客户端发来的消息，包含三个参数（IP4寻址协议，流式连接，Tcp协议）  
            socketwatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //服务端发送信息需要一个IP地址和端口号  
            IPAddress address = IPAddress.Parse("127.0.0.1");
            //将IP地址和端口号绑定到网络节点point上  
            IPEndPoint point = new IPEndPoint(address, 8088);
            //此端口专门用来监听的  

            //监听绑定的网络节点  
            socketwatch.Bind(point);

            //将套接字的监听队列长度限制为20  
            socketwatch.Listen(20);

            //负责监听客户端的线程:创建一个监听线程  
            Thread threadwatch = new Thread(watchconnecting);

            //将主线程设置为与后台同步，随着主线程结束而结束  
            threadwatch.IsBackground = true;

            //启动线程     
            threadwatch.Start();

            Console.WriteLine("开启监听。。。");
            Console.WriteLine("点击输入任意数据回车退出程序。。。");
            Console.ReadKey();
            Console.WriteLine("退出监听，并关闭程序。");
        }

        //监听客户端发来的请求  
        public static void watchconnecting()
        {
            Socket connection = null;

            //持续不断监听客户端发来的请求     
            while (true)
            {
                try
                {
                    connection = socketwatch.Accept();
                }
                catch (Exception ex)
                {
                    //提示套接字监听异常     
                    Console.WriteLine(ex.Message);
                    break;
                }

                //获取客户端的IP和端口号  
                IPAddress clientIP = (connection.RemoteEndPoint as IPEndPoint).Address;
                int clientPort = (connection.RemoteEndPoint as IPEndPoint).Port;

                //让客户显示"连接成功的"的信息  
                string sendmsg = "连接服务端成功！\r\n" + "本地IP:" + clientIP + "，本地端口" + clientPort.ToString();
                byte[] arrSendMsg = Encoding.UTF8.GetBytes(sendmsg);
                connection.Send(arrSendMsg);

                //客户端网络结点号  
                string remoteEndPoint = connection.RemoteEndPoint.ToString();
                //显示与客户端连接情况
                Console.WriteLine("成功与" + remoteEndPoint + "客户端建立连接！\t\n");
                //添加客户端信息  
                clientConnectionItems.Add(remoteEndPoint, connection);

                //IPEndPoint netpoint = new IPEndPoint(clientIP,clientPort); 
                IPEndPoint netpoint = connection.RemoteEndPoint as IPEndPoint;

                //创建一个通信线程      
                ParameterizedThreadStart pts = new ParameterizedThreadStart(recv);
                Thread thread = new Thread(pts);
                //设置为后台线程，随着主线程退出而退出 
                thread.IsBackground = true;
                //启动线程     
                thread.Start(connection);
                
            }
        }

        /// <summary>
        /// 接收客户端发来的信息，客户端套接字对象
        /// </summary>
        /// <param name="socketclientpara"></param>    
        public static void recv(object socketclientpara)
        {
            Socket socketServer = socketclientpara as Socket;

            while (true)
            {
                //创建一个内存缓冲区，其大小为1024*1024字节  即1M     
                byte[] arrServerRecMsg = new byte[1024 * 1024];
                //将接收到的信息存入到内存缓冲区，并返回其字节数组的长度    
                try
                {
                    int length = socketServer.Receive(arrServerRecMsg);

                    //将机器接受到的字节数组转换为人可以读懂的字符串     
                    string strSRecMsg = Encoding.UTF8.GetString(arrServerRecMsg, 0, length);

                    //将发送的字符串信息附加到文本框txtMsg上     
                    Console.WriteLine("客户端:" + socketServer.RemoteEndPoint + ",time:" + GetCurrentTime() + "\r\n" + strSRecMsg + "\r\n\n");

                    socketServer.Send(Encoding.UTF8.GetBytes("测试server 是否可以发送数据给client "));
                }
                catch (Exception ex)
                {
                    clientConnectionItems.Remove(socketServer.RemoteEndPoint.ToString());

                    Console.WriteLine("Client Count:" + clientConnectionItems.Count);

                    //提示套接字监听异常  
                    Console.WriteLine("客户端" + socketServer.RemoteEndPoint + "已经中断连接" + "\r\n" + ex.Message + "\r\n" + ex.StackTrace + "\r\n");
                    //关闭之前accept出来的和客户端进行通信的套接字 
                    socketServer.Close();
                    break;
                }
            }
        }

        ///      
        /// 获取当前系统时间的方法    
        /// 当前时间     
        public static DateTime GetCurrentTime()
        {
            DateTime currentTime = new DateTime();
            currentTime = DateTime.Now;
            return currentTime;
        }
    }
    */
    #endregion
}

