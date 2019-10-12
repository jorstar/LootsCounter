
using LootsCounter.Helpers;
using System;
using System.Threading;
using System.Web;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;

namespace LootsCounter.Controllers.Twitch
{
    /// <summary>  
    ///  Chatbot handler for the loots.
    ///  here is everything that happens in chat.
    /// </summary>  
    internal class ChatBot : LootsClientAccessor
    {
        public TwitchClient Client;
        private int connectionRetries = 0;
        private string channel;
        internal ChatBot( LootsClient lootsClient ) : base( lootsClient ) {
            Connect();
        }

        /// <summary>  
        ///  Connect to the chat.  
        /// </summary>  
        private void Connect() {
            try {
                ConnectionCredentials credentials = new ConnectionCredentials( LootsClient.Cache.Settings.BotUser, LootsClient.Cache.Settings.BotOauth );
                Client = new TwitchClient();
                Client.Initialize( credentials, LootsClient.Cache.Settings.ChannelName );
                Client.OnJoinedChannel += OnChannelJoined;
                Client.OnConnectionError += OnConnectionError;
                Client.OnMessageReceived += OnMessageRecieved;
                Client.OnConnected += OnConnected;
                Client.OnDisconnected += OnDisconnected;
                Client.Connect();

            }
            catch( Exception Ex ) {
                Log.Error( "Error in Connect", Ex );
                Log.CloseProgram();
            }

        }

        private void OnConnected( object sender, OnConnectedArgs e ) {
            Log.Info($"Bot connected to chat: {e.AutoJoinChannel}");
        }

        private void OnChannelJoined( object sender, OnJoinedChannelArgs e ) {
            Log.Info($"Bot joined {e.Channel}");

            channel = e.Channel;
        }
        private void OnDisconnected( object sender, OnDisconnectedEventArgs e ) {
            try {
                if( connectionRetries < 10 ) {
                    Log.Warning( "Chatbot disconnected trying to reconnect." );
                    connectionRetries++;
                    Log.Info( $"Reconnecting {connectionRetries}" );
                    Client.Reconnect();
                }
            }
            catch( Exception Ex ) {
                Log.Error( "OnDisconnected", Ex );
            }
        }


        /// <summary>  
        ///  function for error on connection.  
        /// </summary>  
        private void OnConnectionError( object sender, OnConnectionErrorArgs e ) {
            Log.Error( $"[chatbot connect error] {e.Error.Message}" );
            Log.CloseProgram();
        }

        /// <summary>  
        ///  function for error on recieved messages.  
        /// </summary>  
        private void OnMessageRecieved( object sender, OnMessageReceivedArgs e ) {
            try {
                if( e.ChatMessage.Message[0] == '!' && IsUserAllowed( e.ChatMessage ) ) {
                    LootsClient.Commands.HandleChatCommand( e.ChatMessage.Message.Remove( 0, 1 ) );
                }

                if( e.ChatMessage.Username.ToLower() != LootsClient.Cache.Settings.LootsBotUser.ToLower() ) {
                    return;
                }

                if( e.ChatMessage.Message.ToLower().Contains( "https://loots.com/" ) ) {
                    LootsClient.Counter.HandleLoots( e.ChatMessage.Message.ToLower() );
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
            if( message.IsBroadcaster && LootsClient.Cache.Settings.UseChannelOwner ) {
                return true;
            }
            else if( message.IsModerator && LootsClient.Cache.Settings.UseModerators ) {
                return true;
            }
            else if( message.IsBroadcaster && LootsClient.Cache.ChannelOwnerOnly ) {
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
            message = LootsClient.MessageHelper.ReplacePlaceholders( message );
            message = HttpUtility.HtmlEncode( message );
            Client.SendMessage( channel, message );
        }
    }
}
