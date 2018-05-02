using System;
using Whirlpool.Core.Pattern;
using Whirlpool.Core.Type;

namespace Whirlpool.Core.IO
{
    [NeedsRefactoring]
    public class DiscordController : Singleton<DiscordController>
    {

        private UnsafeDiscordMethods.RichPresence currentPresence;

        /// <summary>
        /// Initialize DiscordController.
        /// </summary>
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

        /// <summary>
        /// Write a message when Discord is ready.
        /// </summary>
        private static void OnReady()
        {
            Logging.Write("Discord RPC ready.");
        }

        /// <summary>
        /// Update the DiscordController.
        /// </summary>
        public static void Update()
        {
            var instance = GetInstance();
            UnsafeDiscordMethods.DiscordRunCallbacks();
            UnsafeDiscordMethods.DiscordUpdatePresence(instance.currentPresence);
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
        private static void OnJoinRequest(UnsafeDiscordMethods.JoinRequest request)
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
