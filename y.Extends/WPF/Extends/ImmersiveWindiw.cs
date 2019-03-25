using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace y.Extends.WPF.Extends
{
    public static class ImmersiveWindiw
    {
        private const int GWL_HWNDPARENT = -8;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpWindowClass, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        

        [DllImport("user32.dll", EntryPoint = "GetWindow")]
        public static extern IntPtr GetWindow(IntPtr hwnd, int wCmd);

        [DllImport("user32.dll")] 
        //EnumWindows函数，EnumWindowsProc 为处理函数 
        private static extern int EnumWindows(EnumWindowsProc ewp, int lParam);
        public delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd,   StringBuilder ClassName, int nMaxCount);



        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);




        public const UInt32 SWP_NOSIZE = 0x0001;
        public const UInt32 SWP_NOMOVE = 0x0002;
        public const UInt32 SWP_NOACTIVATE = 0x0010;
        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        static StringBuilder sb=new StringBuilder((int)Math.Pow(2,16));
        public static bool Report(IntPtr hwnd, int lParam)
        {

            GetClassName(hwnd,   sb, (int)Math.Pow(2, 16));
    
            return true;
        }


        public static void Register(Window window,string windowTitle)
        {
            if (window == null)
            {
                return;
            }

            window.Owner = null;


            IntPtr pWnd = FindWindow("Progman", "Program Manager");
            SendMessage(pWnd, 0x052c, 0, 0);

            EnumWindowsProc myCallBack = new EnumWindowsProc(Report);
            EnumWindows(myCallBack, 0);

            int v = 1;

            //var handle = new WindowInteropHelper(window).Handle;

            //IntPtr pWnd = FindWindow("Progman", "Program Manager");
            //if (pWnd!=IntPtr.Zero)
            //{
            //    pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SHELLDLL_DefVIew", null);
            //    if (pWnd != IntPtr.Zero)
            //    {
            //        var pWnd2 = FindWindow(null, windowTitle);
            //        if (pWnd2 != IntPtr.Zero)
            //        {
            //            SetParent(handle, pWnd);
            //        }
            //    }
            //}



            //IntPtr hWnd = new WindowInteropHelper(window).Handle;
            //SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);

            // const int GW_CHILD = 5;                     //desktop
            //var hDeskTop = FindWindow("Progman", null);     //get system handle
            // hDeskTop = GetWindow(hDeskTop, GW_CHILD);   //get desktop handle
            // SetParent(handle, hDeskTop);           //set this form's parent as desktop


            // var maxin = FindWindow("", "Program Manager");
            // SetParent(handle, maxin);
            // var p = FindWindowEx(maxin, IntPtr.Zero, "SHELLDLL_DefView", "");
            // var hprog = FindWindowEx(p, IntPtr.Zero, "SysListView32", "FolderView");
            // SetWindowLong(handle, GWL_HWNDPARENT, p);

            //var handle = new WindowInteropHelper(window).Handle;
            //IntPtr pWnd = FindWindow("Progman", "Program Manager");
            //pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SHELLDLL_DefVIew", null);
            ////pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SysListView32", null);
            ////IntPtr tWnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;

            //SetWindowLong(handle, GWL_HWNDPARENT, pWnd);

            //var handle = new WindowInteropHelper(window).Handle;
            //var ipr1 = FindWindow("Progman", "");
            //if (ipr1 != IntPtr.Zero)
            //{
            //    var ipr2 = FindWindowEx(ipr1, IntPtr.Zero, "SHELLDLL_DefView", "");
            //    if (ipr2 != IntPtr.Zero)
            //    {
            //        var ipr3 = FindWindowEx(ipr2, IntPtr.Zero, "SysListView32", "FolderView");
            //        if (ipr3 != IntPtr.Zero)
            //        {
            //            SetWindowLong(handle, GWL_HWNDPARENT, ipr2);
            //        }
            //    }
            //}

            //var handle = new WindowInteropHelper(window).Handle;
            //IntPtr hprog = FindWindowEx(
            //                            FindWindow("SHELLDLL_DefView", ""
            //                                        ),
            //                            IntPtr.Zero, "SysListView32", "FolderView"
            //                           );
            //SetWindowLong(handle, GWL_HWNDPARENT, hprog);

            //pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SHELLDLL_DefVIew", null);
            //pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SysListView32", null);
            ////IntPtr tWnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;

            //SetParent(this.Handle, pWnd);

            //IntPtr hWnd = new WindowInteropHelper(window).Handle;
            //IntPtr hWndProgMan = FindWindow("Progman", "Program Manager");
            //SetParent(hWnd, hWndProgMan); 
            //return;
            //var handle = new WindowInteropHelper(window).Handle;
            //var ipr1 = FindWindow("Progman", "Program Manager");
            //var ipr2 = FindWindowEx(ipr1, IntPtr.Zero, "SHELLDLL_DefView", "");
            //var hprog = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "SysListView32", "FolderView");

            //SetParent(handle, hprog);
            // SetWindowLong(handle, GWL_HWNDPARENT, hprog);
        }
    }
}