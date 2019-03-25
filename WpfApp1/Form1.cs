using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WpfApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Load += (s, e) =>
            {
                var pWnd = FindWindow("Progman", null);
                pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SHELLDLL_DefVIew", null);
                pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SysListView32", null);

                //IntPtr tWnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;

                SetParent(Handle, pWnd);
            };
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow([MarshalAs(UnmanagedType.LPTStr)] string lpClassName, [MarshalAs(UnmanagedType.LPTStr)] string lpWindowName);

        [DllImport("user32")]
        private static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, string lpsz1, string lpsz2);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
    }
}