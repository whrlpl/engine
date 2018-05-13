using System;
using System.Runtime.InteropServices;
using Whirlpool.Core.Type;

namespace Whirlpool.Core.IO
{
    [NeedsRefactoring]
    public unsafe class UnsafeNativeMethods
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ReadyHandler();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DisconnectedHandler(int errorCode, string message);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ErroredHandler(int errorCode, string message);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void JoinGameHandler(string secret);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SpectateGameHandler(string secret);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void JoinRequestHandler(JoinRequest request);

        //--------------------------------------------------------------------------------

        public struct EventHandlers
        {
            public ReadyHandler ready;
            public DisconnectedHandler disconnected;
            public ErroredHandler errored;
            public JoinGameHandler joinGame;
            public SpectateGameHandler spectateGame;
            public JoinRequestHandler joinRequest;
        }

        //--------------------------------------------------------------------------------

        [Serializable]
        public struct RichPresence
        {
            public string state;
            public string details;
            public long startTimestamp;
            public long endTimestamp;
            public string largeImageKey;
            public string largeImageText;
            public string smallImageKey;
            public string smallImageText;
            public string partyId;
            public int partySize;
            public int partyMax;
            public string matchSecret;
            public string joinSecret;
            public string spectateSecret;
            public bool instance;
        }

        //--------------------------------------------------------------------------------

        [Serializable]
        public struct JoinRequest
        {
            public string userId;
            public string username;
            public string avatar;
        }

        //--------------------------------------------------------------------------------

        public enum Reply
        {
            No = 0,
            Yes = 1,
            Ignore = 2
        }

        //--------------------------------------------------------------------------------

        [DllImport("discord-rpc.dll", CharSet = CharSet.Unicode)]
        private static extern void Discord_Initialize([MarshalAs(UnmanagedType.LPWStr)]string applicationID,
            ref EventHandlers handlers,
            bool autoRegister,
            [MarshalAs(UnmanagedType.LPWStr)]string optionalSteamId);

        public static void DiscordInitialize(string appID, EventHandlers handlers)
        {
            Discord_Initialize(appID, ref handlers, true, null);
        }

        //--------------------------------------------------------------------------------

        [DllImport("discord-rpc.dll")]
        private static extern void Discord_ClearPresence();

        public static void DiscordClearPresence()
        {
            Discord_ClearPresence();
        }

        //--------------------------------------------------------------------------------

        [DllImport("discord-rpc.dll")]
        private static extern void Discord_UpdatePresence(IntPtr presence);

        public static void DiscordUpdatePresence(RichPresence presence)
        {
            IntPtr ptrPresence = Marshal.AllocHGlobal(Marshal.SizeOf(presence));
            Marshal.StructureToPtr(presence, ptrPresence, false);
            Discord_UpdatePresence(ptrPresence);
            Marshal.FreeHGlobal(ptrPresence);
        }

        //--------------------------------------------------------------------------------

        [DllImport("discord-rpc.dll")]
        private static extern void Discord_Shutdown();

        public static void DiscordShutdown()
        {
            Discord_Shutdown();
        }

        //--------------------------------------------------------------------------------

        [DllImport("discord-rpc.dll")]
        private static extern void Discord_RunCallbacks();

        public static void DiscordRunCallbacks()
        {
            Discord_RunCallbacks();
        }
        //--------------------------------------------------------------------------------

        [DllImport("discord-rpc.dll", CharSet = CharSet.Unicode)]
        private static extern void Discord_Respond(string userId, Reply reply);

        public static void DiscordRespond(string userID, Reply reply)
        {
            Discord_Respond(userID, reply);
        }

        //--------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------
        
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern bool SetSystemCursor(int hcur, int id);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetCursor();

        public static void WindowsSetCursor()
        {
            if (!SetSystemCursor(GetCursor(), 32513))
                Logging.Write("Failed to set cursor", LogStatus.Error);
        }

        //--------------------------------------------------------------------------------

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
