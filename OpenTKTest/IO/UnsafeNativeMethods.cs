using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Core.IO
{
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
        public delegate void JoinRequestHandler(DiscordJoinRequest request);

        //--------------------------------------------------------------------------------

        public struct DiscordEventHandlers
        {
            public ReadyHandler ready;
            public DisconnectedHandler disconnected;
            public ErroredHandler errored;
            public JoinGameHandler joinGame;
            public SpectateGameHandler spectateGame;
            public JoinRequestHandler joinRequest;
        }

        //--------------------------------------------------------------------------------

        public struct DiscordRichPresence
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

        public struct DiscordJoinRequest
        {
            public string userId;
            public string username;
            public string avatar;
        }

        //--------------------------------------------------------------------------------

        public enum DiscordReply
        {
            No = 0,
            Yes = 1,
            Ignore = 2
        }

        //--------------------------------------------------------------------------------

        [DllImport("discord-rpc.dll", CharSet = CharSet.Unicode)]
        private static extern void Discord_Initialize([MarshalAs(UnmanagedType.LPWStr)]string applicationID,
            ref DiscordEventHandlers handlers,
            bool autoRegister,
            [MarshalAs(UnmanagedType.LPWStr)]string optionalSteamId);

        public static void DiscordInitialize(string appID, DiscordEventHandlers handlers)
        {
            Discord_Initialize(appID, ref handlers, true, String.Empty);
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

        public static void DiscordUpdatePresence(DiscordRichPresence presence)
        {
            IntPtr ptrPresence = Marshal.AllocHGlobal(Marshal.SizeOf(presence));
            Marshal.StructureToPtr(presence, ptrPresence, false);
            Discord_UpdatePresence(ptrPresence);
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
        private static extern void Discord_Respond(string userId, DiscordReply reply);

        public static void DiscordRespond(string userID, DiscordReply reply)
        {
            Discord_Respond(userID, reply);
        }
    }
}
