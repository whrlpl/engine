using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.IO
{
    public class DiscordController
    {
        protected static UnsafeNativeMethods.RichPresence testPresence = new UnsafeNativeMethods.RichPresence()
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

        protected static UnsafeNativeMethods.RichPresence defaultPresence = new UnsafeNativeMethods.RichPresence()
        {
            state = "Idle",
            largeImageKey = "default",
            largeImageText = "Idle"
        };

        public static void Init()
        {
            UnsafeNativeMethods.EventHandlers eventHandlers = new UnsafeNativeMethods.EventHandlers()
            {
                errored = OnErrored,
                joinGame = OnJoinGame,
                joinRequest = OnJoinRequest,
                spectateGame = OnSpectateGame
            };
            UnsafeNativeMethods.Initialize("408670906404175883", eventHandlers);
        }

        public static void Update()
        {
            UnsafeNativeMethods.RunCallbacks();
            UnsafeNativeMethods.UpdatePresence(defaultPresence);
        }

        private static void OnSpectateGame(string secret)
        {
            throw new NotImplementedException("Spectating is not implemented yet.");
        }

        private static void OnJoinRequest(UnsafeNativeMethods.JoinRequest request)
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
