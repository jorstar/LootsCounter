
using LootsCounter.Helpers;
using LootsCounter.Models.Twitch;
using System;
using System.Collections.Generic;

namespace LootsCounter.Controllers.Twitch
{
    /// <summary>  
    ///  Handle all chat commands.  
    /// </summary>  
    internal class Commands : LootsClientAccessor
    {
        private Dictionary<string, Command> commands = new Dictionary<string, Command>();

        internal Commands( LootsClient lootsClient ) : base( lootsClient ) {

        }

        public void HandleChatCommand( string message ) {
            try {
                string[] args = message.Split( ' ' );
                if( commands.TryGetValue( args[0].ToLower(), out Command com ) ) {
                    
                    string execute = args.Length > 1 ? args[1] : "";
                    string extra = args.Length > 2 ? args[2] : "";
                    bool executed = com.Execute.Invoke( execute, extra );

                    if ( !executed ) {
                        return;
                    }

                    Log.Info( com.Log );

                    if( !string.IsNullOrEmpty( com.Message ) ) {                        
                        LootsClient.ChatBot.SendMessage( com.Message );
                    }
                }
            }
            catch (Exception Ex) {
                Log.Error( "[HandleChatCommand]", Ex );
            }
        }

        public void PrepareCommands() {
            commands.Add( LootsClient.Cache.Settings.AddRemoveLootsCommand.ToLower(), new Command {
                Log = "Successfully mutated the loots count",
                Message = LootsClient.ActiveInstance.Cache.Settings.MutationResponse,
                Execute = LootsClient.ActiveInstance.Counter.Mutate
            } );

            commands.Add( "lootscount", new Command {
                Log = "Show loots command successfull",
                Message = "",
                Execute = LootsClient.ActiveInstance.Counter.ShowCount
            } );
        }
    }
}
