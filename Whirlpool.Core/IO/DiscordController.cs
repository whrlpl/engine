using System;
using Whirlpool.Core.Pattern;
using Whirlpool.Core.Type;

namespace Whirlpool.Core.IO
{
    [NeedsRefactoring]
    public class DiscordController : Singleton<DiscordController>
    {

        private UnsafeNativeMethods.RichPresence currentPresence = new UnsafeNativeMethods.RichPresence()
        {
            details = "In menus",
            state = "Idle",
            partySize = 1,
            partyMax = 4,
            partyId = "partyid325892",
            joinSecret = "joinsecret23859",
            largeImageKey = "main-menu"
        };

        /// <summary>
        /// Initialize DiscordController.
        /// </summary>
        public static void Init()
        {
            UnsafeNativeMethods.EventHandlers eventHandlers = new UnsafeNativeMethods.EventHandlers()
            {
                errored = OnErrored,
                joinGame = OnJoinGame,
                joinRequest = OnJoinRequest,
                spectateGame = OnSpectateGame,
                ready = OnReady
            };
            UnsafeNativeMethods.DiscordInitialize("446739162083622913", eventHandlers);
        }

        /// <summary>
        /// Write a message when Discord is ready.
        /// </summary>
        private static void OnReady(UnsafeNativeMethods.DiscordUser user)
        {
            Logging.Write("Discord RPC ready, connected to " + user.username + "#" + user.discriminator);
        }

        /// <summary>
        /// Update the DiscordController.
        /// </summary>
        public static void Update()
        {
            var instance = GetInstance();
            UnsafeNativeMethods.DiscordRunCallbacks();
            UnsafeNativeMethods.DiscordUpdatePresence(instance.currentPresence);
        }

        /// <summary>
        /// Handler for game spectating.
        /// </summary>
        /// <param name="secret">Match secret</param>
        private static void OnSpectateGame(string secret)
        {
            throw new NotImplementedException("Spectating is not implemented yet.");
        }

        /// <summary>
        /// Handler for game joining.
        /// </summary>
        /// <param name="request">The join request</param>
        private static void OnJoinRequest(UnsafeNativeMethods.DiscordUser user)
        {
            throw new NotImplementedException("Joining is not implemented yet.");
        }

        /// <summary>
        /// Handler for game joining.
        /// </summary>
        /// <param name="secret">Match secret</param>
        private static void OnJoinGame(string secret)
        {
            throw new NotImplementedException("Joining is not implemented yet.");
        }
        
        /// <summary>
        /// Handler for errors.
        /// </summary>
        /// <param name="errorCode">The error code (as provided by Discord's API)</param>
        /// <param name="message">The error message</param>
        private static void OnErrored(int errorCode, string message)
        {
            throw new Exception("Discord RPC failed with error " + errorCode + ":\n" + message);
        }
    }
}
