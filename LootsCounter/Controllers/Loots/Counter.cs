using LootsCounter.Helpers;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace LootsCounter.Controllers.Loots
{
    /// <summary>  
    ///  Loots counter
    ///  all loots mutations happen here.  
    /// </summary>  
    class Counter : LootsClientAccessor
    {
        private string LootsFile { get; } = "LootsCounter.txt";

        internal Counter( LootsClient lootsClient ) : base( lootsClient ) {
            if ( !File.Exists( LootsFile ) ) {
                ResetCount();
            }
            else {
                Log.Info( "Loots file found, Do you want to continue the current count ? [Y / N]" );
                if ( Log.AcceptOrDeny() ) {
                    LoadFile();
                }
                else {
                    Log.Info( "Reset the loots count to 0" );
                    ResetCount();
                    Log.Info( $"The current loots count is: {LootsClient.Cache.LootsCount}" );
                }
            }
        }

        /// <summary>  
        ///  Handle loots message that came from chatbot.  
        /// </summary>  
        public void HandleLoots( string message ) {
            if ( message.Contains( LootsClient.Cache.Settings.LootsLink ) ) {
                Log.Info( "Loots link shown in chat!" );
            }
            else {
                Add();
                Log.Info( $"The current loots count is: {LootsClient.Cache.LootsCount}" );
            }
        }

        public bool Mutate( string addremove, string extra ) {
            try {
                if ( string.IsNullOrEmpty( addremove ) ) {
                    return false;
                }

                int count;
                bool tryparse = int.TryParse( extra, out count );
                if (!tryparse) {
                    count = 1;
                }

                bool ret;
                switch ( addremove.ToLower() ) {
                    case "add":
                    case "+1":
                    case "+":
                        Add( count );
                        ret = true;
                        break;
                    case "remove":
                    case "-1":
                    case "-":
                    case "del":
                    case "delete":
                        Remove( count );
                        ret = true;
                        break;
                    case "set":
                        if ( tryparse ) {
                            SetCount( count );
                            ret = true;
                        }
                        else {
                            ret = false;
                        }

                        break;
                    case "reset":
                        ResetCount();
                        LootsClient.ChatBot.SendMessage( LootsClient.Cache.Settings.ResetCommandResponse );
                        ret = true;
                        break;
                    default:
                        ret = false;
                        break;
                }

                Log.Info( $"Loots count is now at {LootsClient.Cache.LootsCount}" );
                return ret;
            }
            catch ( Exception Ex ) {
                Log.Error( "Mutate", Ex );
                return false;
            }
        }

        private void SetCount( int count ) {
            Log.Info( $"Setting count from {LootsClient.Cache.LootsCount} to {count}" );
            LootsClient.Cache.LootsCount = count;
            WriteFile();

        }

        public string GetCount() {
            return LootsClient.Cache.Settings.ResetCounter ? $"{ LootsClient.Cache.LootsCount} / { LootsClient.Cache.Settings.ResetAtCount}" : LootsClient.Cache.LootsCount.ToString();
        }
        public bool ShowCount( string s = "", string extra = "" ) {

            Log.Info( $"The current loots count is {GetCount()}" );
            LootsClient.ChatBot.SendMessage( LootsClient.Cache.Settings.LootsCountResponse );

            return true;
        }

        /// <summary>  
        ///  Add 1 to loots count.
        ///  if resetting the count is enabled and the count is
        ///  the same as the reset count then reset the count
        ///  and send a chat message.
        /// </summary>  
        public void Add(int count = 1) {
            LootsClient.Cache.LootsCount+= count;
            WriteFile();

            if ( LootsClient.Cache.Settings.ResetCounter && LootsClient.Cache.LootsCount == LootsClient.Cache.Settings.ResetAtCount ) {
                ResetCount();
                LootsClient.ChatBot.SendMessage( LootsClient.Cache.Settings.ResetMessage );
            }

        }

        /// <summary>  
        ///  remove 1 from loots count.
        /// </summary>  
        public void Remove( int count = 1 ) {
            if ( (LootsClient.Cache.LootsCount == 0 || ( LootsClient.Cache.LootsCount - count ) < 0) && LootsClient.Cache.Settings.ResetCounter ) {
                LootsClient.Cache.LootsCount = LootsClient.Cache.LootsCount - count + LootsClient.Cache.Settings.ResetAtCount ;
                WriteFile();
                Log.Info( "Reverted back and removed 1" );
            }
            else if ( LootsClient.Cache.LootsCount != 0 && (LootsClient.Cache.LootsCount - count) >= 0 ) {
                LootsClient.Cache.LootsCount -= count;
                WriteFile();
                Log.Info( $"Removed {count} from Loots Count" );
            }
            else {
                Log.Info( "Cant remove when counter is at 0" );
            }
        }

        /// <summary>  
        ///  Reset the loots count.  
        /// </summary>  
        private void ResetCount() {
            LootsClient.Cache.LootsCount = 0;
            WriteFile();
            Log.Info( "Loots count has been reset!" );
        }

        /// <summary>  
        ///  Load the loots file.  
        /// </summary>  
        private void LoadFile() {
            if ( !File.Exists( LootsFile ) ) {
                Log.Info( $"{LootsFile} does not exist, Creating file." );
                ResetCount();
            }
            else {
                try {
                    string LootsCountText = "";
                    using ( StreamReader readtext = new StreamReader( LootsFile ) ) {
                        LootsCountText = readtext.ReadLine();
                    }

                    LootsClient.Cache.LootsCount = Convert.ToInt16( Regex.Match( LootsCountText.Split( '/' )[0].Trim(), @"\d+" ).Value );

                    Log.Info( $"Original Loots Loaded. Loots is now set to {LootsClient.Cache.LootsCount}" );
                }
                catch ( Exception Ex ) {
                    Log.Error( "Loots Counter LoadFile", Ex );
                    Log.CloseProgram();
                }
            }
        }

        /// <summary>  
        ///  write the count to the loots file.  
        /// </summary>  
        private void WriteFile() {
            try {
                using ( StreamWriter sw = new StreamWriter( LootsFile ) ) {
                    sw.WriteLine( $"{LootsClient.Cache.Settings.ScreenText} {LootsClient.Cache.LootsCount} / {LootsClient.Cache.Settings.ResetAtCount}" );
                }
                Log.Info( "Wrote loots to file." );
            }
            catch ( Exception Ex ) {
                Log.Error( "Error Writing loots file", Ex );
                Log.CloseProgram();
            }
        }
    }
}
