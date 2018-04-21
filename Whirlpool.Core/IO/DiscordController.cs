using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.IO
{
    public class DiscordController
    {
        protected static UnsafeDiscordMethods.RichPresence testPresence = new UnsafeDiscordMethods.RichPresence()
        {
            state = "Playing Solo",
            details = "Deathmatch",
            startTimestamp = DateTime.MinValue.Ticks,
            largeImageKey = "default",
            largeImageText = "testmap",
            smallImageKey = "s_default",
            smallImageText = "Level 1"
        };

        protected static UnsafeDiscordMethods.RichPresence defaultPresence = new UnsafeDiscordMethods.RichPresence()
        {
            state = "Idle",
            largeImageKey = "default",
            largeImageText = "Idle"
        };

        public static void Init()
        {
            UnsafeDiscordMethods.EventHandlers eventHandlers = new UnsafeDiscordMethods.EventHandlers()
            {
                errored = OnErrored,
                joinGame = OnJoinGame,
                joinRequest = OnJoinRequest,
                spectateGame = OnSpectateGame,
                ready = OnReady
            };
            UnsafeDiscordMethods.DiscordInitialize("436934908707864576", eventHandlers);
        }

        private static void OnReady()
        {
            Console.WriteLine("Discord RPC ready.");
        }

        public static void Update()
        {
            UnsafeDiscordMethods.DiscordRunCallbacks();
            UnsafeDiscordMethods.DiscordUpdatePresence(defaultPresence);
        }

        private static void OnSpectateGame(string secret)
        {
            throw new NotImplementedException("Spectating is not implemented yet.");
        }

        private static void OnJoinRequest(UnsafeDiscordMethods.JoinRequest request)
        {
            throw new NotImplementedException("Joining is not implemented yet.");
        }

        private static void OnJoinGame(string secret)
        {
            throw new NotImplementedException("Joining is not implemented yet.");
        }

        private static void OnErrored(int errorCode, string message)
        {
            throw new Exception("Discord RPC failed with error " + errorCode + ":\n" + message);
        }
    }
}
