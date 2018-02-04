using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.IO
{
    public class DiscordController
    {
        protected static DiscordRPC.RichPresence testPresence = new DiscordRPC.RichPresence()
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

        protected static DiscordRPC.RichPresence defaultPresence = new DiscordRPC.RichPresence()
        {
            state = "Idle",
            largeImageKey = "default",
            largeImageText = "Idle"
        };

        public static void Init()
        {
            DiscordRPC.EventHandlers eventHandlers = new DiscordRPC.EventHandlers()
            {
                errored = onErrored,
                joinGame = onJoinGame,
                joinRequest = onJoinRequest,
                spectateGame = onSpectateGame
            };
            DiscordRPC.Initialize("408670906404175883", eventHandlers);
        }

        public static void Update()
        {
            DiscordRPC.RunCallbacks();
            DiscordRPC.UpdatePresence(defaultPresence);
        }

        private static void onSpectateGame(string secret)
        {
            throw new NotImplementedException("Spectating is not implemented yet.");
        }

        private static void onJoinRequest(DiscordRPC.JoinRequest request)
        {
            throw new NotImplementedException("Joining is not implemented yet.");
        }

        private static void onJoinGame(string secret)
        {
            throw new NotImplementedException("Joining is not implemented yet.");
        }

        private static void onErrored(int errorCode, string message)
        {
            throw new Exception("DiscordRPC failed with error " + errorCode + ":\n" + message);
        }
    }
}
