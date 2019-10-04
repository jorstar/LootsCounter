
using LootsCounter.Helpers;
using System;
using System.Web;
using TwitchLib;
using TwitchLib.Events.Client;
using TwitchLib.Models.Client;
using TwitchLib.Services;

namespace LootsCounter.Controllers.Twitch
{
    /// <summary>  
    ///  Chatbot handler for the loots.
    ///  here is everything that happens in chat.
    /// </summary>  
    internal class ChatBot : ProgramAccessor
    {
        public TwitchClient Client;
        internal ChatBot( Program program ) : base( program ) {
            Connect();           
        }

        /// <summary>  
        ///  Connect to the chat.  
        /// </summary>  
        private void Connect() {
            try {
                ConnectionCredentials credentials = new ConnectionCredentials( Program.Cache.Settings.BotUser, Program.Cache.Settings.BotOauth );
                Client = new TwitchClient( credentials, Program.Cache.Settings.ChannelName, logging: false );
                Client.ChatThrottler = new MessageThrottler( 20 / 2, TimeSpan.FromSeconds( 30 ) );
                Client.OnConnectionError += OnConnectionError;
                Client.OnMessageReceived += OnMessageRecieved;
                Client.Connect();
                Log.Info($"Connected as {Program.Cache.Settings.BotUser} to channel {Program.Cache.Settings.ChannelName}");
            }
            catch( Exception Ex ) {
                Log.Error( "Error in Connect", Ex );
                Log.CloseProgram();
            }

        }


        /// <summary>  
        ///  function for error on connection.  
        /// </summary>  
        private void OnConnectionError( object sender, OnConnectionErrorArgs e ) {
            Log.Error( $"[chatbot connect error] {e.Error}" );
            Log.CloseProgram();
        }

        /// <summary>  
        ///  function for error on recieved messages.  
        /// </summary>  
        private void OnMessageRecieved( object sender, OnMessageReceivedArgs e ) {
            try {
                if( e.ChatMessage.Message[0] == '!' && IsUserAllowed( e.ChatMessage ) ) {
                    Program.Commands.HandleChatCommand( e.ChatMessage.Message.Remove( 0, 1 ) );
                }

                if( e.ChatMessage.Username.ToLower() != Program.Cache.Settings.LootsBotUser.ToLower() ) {
                    return;
                }

                if( e.ChatMessage.Message.ToLower().Contains( "https://loots.com/" ) ) {
                    Program.Counter.HandleLoots( e.ChatMessage.Message.ToLower() );
                }
            }
            catch( Exception Ex ) {
                Log.Error( "MessageRecieved Error", Ex );
            }
        }

        /// <summary>  
        /// Check if user is allowed.  
        /// </summary>  
        private bool IsUserAllowed( ChatMessage message ) {
            if( message.IsBroadcaster && Program.Cache.Settings.UseChannelOwner) {
                return true;
            }
            else if( message.IsModerator && Program.Cache.Settings.UseModerators ) {
                return true;
            }
            else if( message.IsBroadcaster && Program.Cache.ChannelOwnerOnly ) {
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>  
        ///  Send message to chat.  
        /// </summary>  
        public void SendMessage( string message ) {
            message = Program.MessageHelper.ReplacePlaceholders( message );
            message = HttpUtility.HtmlEncode( message );
            Client.SendMessage( message );
        }
    }
}
