using System;
using Whirlpool.Core.Pattern;

namespace Whirlpool.Core.IO.ThirdParty
{
    public class DiscordController : Singleton<DiscordController>
    {

        public DiscordWrapper.RichPresence m_currentPresence = new DiscordWrapper.RichPresence()
        {
            details = "Free Roam",
            state = "Playing Alone",
            partySize = 1,
            partyMax = 65535,
            partyId = "sgdfhdfgh",
            joinSecret = "adsfr",
            largeImageKey = "test",
            smallImageKey = "180sx",
            largeImageText = "Yokohama City",
            smallImageText = "Nissan 180SX"
        };

        public static DiscordWrapper.RichPresence currentPresence
        {
            get
            {
                return GetInstance().m_currentPresence;
            }
            set
            {
                GetInstance().m_currentPresence = value;
            }
        }

        public static void Init()
        {
            Logging.Write("Initializing Discord");
            DiscordWrapper.EventHandlers eventHandlers = new DiscordWrapper.EventHandlers()
            {
                errored = OnErrored,
                joinGame = OnJoinGame,
                joinRequest = OnJoinRequest,
                spectateGame = OnSpectateGame,
                ready = OnReady
            };
            Logging.Write("Discord client ID: " + BaseGame.gameProperties.discordClientID);
            DiscordWrapper.DiscordInitialize(BaseGame.gameProperties.discordClientID, ref eventHandlers);
        }

        private static void OnReady(ref DiscordWrapper.DiscordUser user)
        {
            Logging.Write("Discord RPC ready, connected to " + user.username + "#" + user.discriminator);
        }

        public static void Update()
        {
            var instance = GetInstance();
            DiscordWrapper.DiscordRunCallbacks();
            DiscordWrapper.DiscordUpdatePresence(currentPresence);
        }

        public static void Shutdown()
        {
            Logging.Write("Shutting down Discord RPC");
            System.Threading.Thread.Sleep(500);
            DiscordWrapper.DiscordShutdown();
        }

        private static void OnSpectateGame(string secret)
        {
            throw new NotImplementedException("Spectating is not implemented yet.");
        }

        private static void OnJoinRequest(ref DiscordWrapper.DiscordUser user)
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
