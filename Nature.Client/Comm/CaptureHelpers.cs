﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace NatureConsole
{
    public class CaptureHelpers
    {
        /// <summary>  
        /// 该函数返回桌面窗口的句柄。桌面窗口覆盖整个屏幕。桌面窗口是一个要在其上绘制所有的图标和其他窗口的区域。  
        /// 【说明】获得代表整个屏幕的一个窗口（桌面窗口）句柄.  
        /// </summary>  
        /// <returns>返回值：函数返回桌面窗口的句柄。</returns>  
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetDesktopWindow();

        /// <summary>  
        /// 该函数返回与指定窗口有特定关系（如Z序或所有者）的窗口句柄。  
        /// 函数原型：HWND GetWindow（HWND hWnd，UNIT nCmd）；  
        /// </summary>  
        /// <param name="hWnd">窗口句柄。要获得的窗口句柄是依据nCmd参数值相对于这个窗口的句柄。</param>  
        /// <param name="uCmd">说明指定窗口与要获得句柄的窗口之间的关系。该参数值参考GetWindowCmd枚举。</param>  
        /// <returns>返回值：如果函数成功，返回值为窗口句柄；如果与指定窗口有特定关系的窗口不存在，则返回值为NULL。  
        /// 若想获得更多错误信息，请调用GetLastError函数。  
        /// 备注：在循环体中调用函数EnumChildWindow比调用GetWindow函数可靠。调用GetWindow函数实现该任务的应用程序可能会陷入死循环或退回一个已被销毁的窗口句柄。  
        /// 速查：Windows NT：3.1以上版本；Windows：95以上版本；Windows CE：1.0以上版本；头文件：winuser.h；库文件：user32.lib。  
        /// </returns>  
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCmd uCmd);

        /// <summary>  
        /// 窗口与要获得句柄的窗口之间的关系。  
        /// </summary>  
        enum GetWindowCmd : uint
        {
            /// <summary>  
            /// 返回的句柄标识了在Z序最高端的相同类型的窗口。  
            /// 如果指定窗口是最高端窗口，则该句柄标识了在Z序最高端的最高端窗口；  
            /// 如果指定窗口是顶层窗口，则该句柄标识了在z序最高端的顶层窗口：  
            /// 如果指定窗口是子窗口，则句柄标识了在Z序最高端的同属窗口。  
            /// </summary>  
            GW_HWNDFIRST = 0,
            /// <summary>  
            /// 返回的句柄标识了在z序最低端的相同类型的窗口。  
            /// 如果指定窗口是最高端窗口，则该柄标识了在z序最低端的最高端窗口：  
            /// 如果指定窗口是顶层窗口，则该句柄标识了在z序最低端的顶层窗口；  
            /// 如果指定窗口是子窗口，则句柄标识了在Z序最低端的同属窗口。  
            /// </summary>  
            GW_HWNDLAST = 1,
            /// <summary>  
            /// 返回的句柄标识了在Z序中指定窗口下的相同类型的窗口。  
            /// 如果指定窗口是最高端窗口，则该句柄标识了在指定窗口下的最高端窗口：  
            /// 如果指定窗口是顶层窗口，则该句柄标识了在指定窗口下的顶层窗口；  
            /// 如果指定窗口是子窗口，则句柄标识了在指定窗口下的同属窗口。  
            /// </summary>  
            GW_HWNDNEXT = 2,
            /// <summary>  
            /// 返回的句柄标识了在Z序中指定窗口上的相同类型的窗口。  
            /// 如果指定窗口是最高端窗口，则该句柄标识了在指定窗口上的最高端窗口；  
            /// 如果指定窗口是顶层窗口，则该句柄标识了在指定窗口上的顶层窗口；  
            /// 如果指定窗口是子窗口，则句柄标识了在指定窗口上的同属窗口。  
            /// </summary>  
            GW_HWNDPREV = 3,
            /// <summary>  
            /// 返回的句柄标识了指定窗口的所有者窗口（如果存在）。  
            /// GW_OWNER与GW_CHILD不是相对的参数，没有父窗口的含义，如果想得到父窗口请使用GetParent()。  
            /// 例如：例如有时对话框的控件的GW_OWNER，是不存在的。  
            /// </summary>  
            GW_OWNER = 4,
            /// <summary>  
            /// 如果指定窗口是父窗口，则获得的是在Tab序顶端的子窗口的句柄，否则为NULL。  
            /// 函数仅检查指定父窗口的子窗口，不检查继承窗口。  
            /// </summary>  
            GW_CHILD = 5,
            /// <summary>  
            /// （WindowsNT 5.0）返回的句柄标识了属于指定窗口的处于使能状态弹出式窗口（检索使用第一个由GW_HWNDNEXT 查找到的满足前述条件的窗口）；  
            /// 如果无使能窗口，则获得的句柄与指定窗口相同。  
            /// </summary>  
            GW_ENABLEDPOPUP = 6
        }

        /*GetWindowCmd指定结果窗口与源窗口的关系，它们建立在下述常数基础上： 
              GW_CHILD 
              寻找源窗口的第一个子窗口 
              GW_HWNDFIRST 
              为一个源子窗口寻找第一个兄弟（同级）窗口，或寻找第一个顶级窗口 
              GW_HWNDLAST 
              为一个源子窗口寻找最后一个兄弟（同级）窗口，或寻找最后一个顶级窗口 
              GW_HWNDNEXT 
              为源窗口寻找下一个兄弟窗口 
              GW_HWNDPREV 
              为源窗口寻找前一个兄弟窗口 
              GW_OWNER 
              寻找窗口的所有者 
         */

        [DllImport("user32.dll", EntryPoint = "GetWindowText")]
        public static extern int GetWindowText(int hWnd,StringBuilder lpString,int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "GetClassName")]
        public static extern int GetClassName(int hWnd,StringBuilder lpString,int nMaxCont);

        [StructLayout(LayoutKind.Sequential)]//定义与API相兼容结构体，实际上是一种内存转换
        public struct POINTAPI
        {
            public int X;
            public int Y;
        }
        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]//获取鼠标坐标
        public static extern int GetCursorPos(ref POINTAPI lpPoint);


        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);


        public static void Test()
        {
            //1、获取桌面窗口的句柄  
            IntPtr desktopPtr = GetDesktopWindow();
            //2、获得一个子窗口（这通常是一个顶层窗口，当前活动的窗口）  
            IntPtr winPtr = GetWindow(desktopPtr, GetWindowCmd.GW_CHILD);

            //3、循环取得桌面下的所有子窗口  
            while (winPtr != IntPtr.Zero)
            {
                //4、继续获取下一个子窗口  
                winPtr = GetWindow(winPtr, GetWindowCmd.GW_HWNDNEXT);
                StringBuilder name = new StringBuilder(256);
                //Console.WriteLine(winPtr.ToString() + "——" + GetWindowText(winPtr.ToInt32(), name, 256) + "——" + GetClassName(winPtr.ToInt32(), name, 256));
            }
        }

        //获取当前顶置窗口句柄的标题
        public static void GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                Console.WriteLine(Buff.ToString());
            }
        }


        //通过进程名称获取当前进程中主线程的截图
        public static void GetActiveWindowImage(string procName)
        {
            var proc = Process.GetProcessesByName(procName)[0];
            var rect = new User32.Rect();
            User32.GetWindowRect(proc.MainWindowHandle, ref rect);
            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.CopyFromScreen(rect.left, rect.top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);

            string guid = Guid.NewGuid().ToString();

            bmp.Save(@"C:\Users\Enz\Desktop\" + guid + ".png", ImageFormat.Png);
        }

        //获取顶置窗口句柄的截图
        public void GetActiveWindowImage()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            var rect = new User32.Rect();
            User32.GetWindowRect(handle, ref rect);

            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.CopyFromScreen(rect.left, rect.top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);

            string guid = Guid.NewGuid().ToString();

            bmp.Save(@"C:\Users\Enz\Desktop\" + guid + ".png", ImageFormat.Png);
        }
        //获取顶置窗口句柄的截图的路径
        public string GetActiveWindowImageUrl()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            var rect = new User32.Rect();
            User32.GetWindowRect(handle, ref rect);

            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.CopyFromScreen(rect.left, rect.top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);

            string guid = Guid.NewGuid().ToString();

            bmp.Save(@"C:\Users\Enz\Desktop\" + guid + ".png", ImageFormat.Png);
            return @"C:\Users\Enz\Desktop\" + guid + ".png";
        }
    }

    //通过窗口句柄获取窗口矩形
    public static class User32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
    }
}
