
using LootsCounter.Helpers;
using LootsCounter.Models.Twitch;
using System;
using System.Collections.Generic;

namespace LootsCounter.Controllers.Twitch
{
    /// <summary>  
    ///  Handle all chat commands.  
    /// </summary>  
    internal class Commands : ProgramAccessor
    {
        private Dictionary<string, Command> commands = new Dictionary<string, Command>();

        internal Commands( Program program ) : base( program ) {

        }

        public void HandleChatCommand( string message ) {
            try {
                string[] args = message.Split( ' ' );
                if( commands.TryGetValue( args[0].ToLower(), out Command com ) ) {
                    
                    string execute = args.Length > 1 ? args[1] : "";
                    bool executed = com.Execute.Invoke( execute );

                    if ( !executed ) {
                        return;
                    }

                    Log.Info( com.Log );

                    if( !string.IsNullOrEmpty( com.Message ) ) {                        
                        Program.ChatBot.SendMessage( com.Message );
                    }
                }
            }
            catch (Exception Ex) {
                Log.Error( "[HandleChatCommand]", Ex );
            }
        }

        public void PrepareCommands() {
            commands.Add( Program.Cache.Settings.AddRemoveLootsCommand.ToLower(), new Command {
                Log = "Successfully mutated the loots count",
                Message = Program.ActiveInstance.Cache.Settings.MutationResponse,
                Execute = Program.ActiveInstance.Counter.Mutate
            } );

            commands.Add( "lootscount", new Command {
                Log = "Show loots command successfull",
                Message = "",
                Execute = Program.ActiveInstance.Counter.ShowCount
            } );
        }
    }
}
