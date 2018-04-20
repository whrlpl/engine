using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Core.IO
{
    public class DiscordController
    {
        protected static UnsafeNativeMethods.DiscordRichPresence testPresence = new UnsafeNativeMethods.DiscordRichPresence()
        {
            state = "Playing Solo",
            details = "Deathmatch",
            startTimestamp = DateTime.MinValue.Ticks,
            largeImageKey = "default",
            largeImageText = "testmap",
            smallImageKey = "s_default",
            smallImageText = "Level 1",
            instance = true
        };

        protected static UnsafeNativeMethods.DiscordRichPresence defaultPresence = new UnsafeNativeMethods.DiscordRichPresence()
        {
            state = "Idle",
            largeImageKey = "default",
            largeImageText = "Idle"
        };

        public static void Init()
        {
            UnsafeNativeMethods.DiscordEventHandlers eventHandlers = new UnsafeNativeMethods.DiscordEventHandlers()
            {
                errored = OnErrored,
                joinGame = OnJoinGame,
                joinRequest = OnJoinRequest,
                spectateGame = OnSpectateGame
            };
            UnsafeNativeMethods.DiscordInitialize("436934908707864576", eventHandlers);
        }

        public static void Update()
        {
            UnsafeNativeMethods.DiscordRunCallbacks();
            UnsafeNativeMethods.DiscordUpdatePresence(defaultPresence);
        }

        private static void OnSpectateGame(string secret)
        {
            throw new NotImplementedException("Spectating is not implemented yet.");
        }

        private static void OnJoinRequest(UnsafeNativeMethods.DiscordJoinRequest request)
        {
            throw new NotImplementedException("Joining is not implemented yet.");
        }

        private static void OnJoinGame(string secret)
        {
            throw new NotImplementedException("Joining is not implemented yet.");
        }

        private static void OnErrored(int errorCode, string message)
        {
            throw new Exception("UnsafeNativeMethods failed with error " + errorCode + ":\n" + message);
        }
    }
}
