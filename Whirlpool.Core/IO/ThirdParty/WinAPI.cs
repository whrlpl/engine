using System;
using System.Runtime.InteropServices;

namespace Whirlpool.Core.IO.ThirdParty
{
    public unsafe class WinAPI
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        public static void ShowConsole()
        {
            ShowWindow(GetConsoleWindow(), SW_SHOW);
        }

        public static void HideConsole()
        {
            ShowWindow(GetConsoleWindow(), SW_HIDE);
        }
    }
}
