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
    class Counter : ProgramAccessor
    {
        private string LootsFile { get; } = "LootsCounter.txt";

        internal Counter( Program program ) : base( program ) {
            if( !File.Exists( LootsFile ) ) {
                ResetCount();
            }
            else {
                Log.Info( "Loots file found, Do you want to continue the current count ? [Y / N]" );
                if( Log.AcceptOrDeny() ) {
                    LoadFile();
                }
                else {
                    Log.Info( "Reset the loots count to 0" );
                    ResetCount();
                    Log.Info( $"The current loots count is: {Program.Cache.LootsCount}" );
                }
            }
        }

        /// <summary>  
        ///  Handle loots message that came from chatbot.  
        /// </summary>  
        public void HandleLoots( string message ) {
            if( message.Contains( Program.Cache.Settings.LootsLink ) ) {
                Log.Info( "Loots link shown in chat!" );
            }
            else {
                Add();
                Log.Info( $"The current loots count is: {Program.Cache.LootsCount}" );
            }
        }

        public bool Mutate( string addremove ) {
            if( string.IsNullOrEmpty( addremove ) ) {
                return false;
            }

            bool ret;
            switch( addremove.ToLower() ) {
            case "add":
            case "+1":
            case "+":
                Add();
                ret = true;
                break;
            case "remove":
            case "-1":
            case "-":
            case "del":
            case "delete":
                Remove();
                ret = true;
                break;
            case "reset":
                ResetCount();
                Program.ChatBot.SendMessage( Program.Cache.Settings.ResetCommandResponse );
                ret = true;
                break;
            default:
                ret = false;
                break;
            }

            Log.Info( $"Loots count is now at {Program.Cache.LootsCount}" );
            return ret;
        }

        public string GetCount() {
            return Program.Cache.Settings.ResetCounter ? $"{ Program.Cache.LootsCount} / { Program.Cache.Settings.ResetAtCount}" : Program.Cache.LootsCount.ToString();
        }
        public bool ShowCount( string s = "" ) {

            Log.Info( $"The current loots count is {GetCount()}" );
            Program.ChatBot.SendMessage(Program.Cache.Settings.LootsCountResponse);

            return true;
        }

        /// <summary>  
        ///  Add 1 to loots count.
        ///  if resetting the count is enabled and the count is
        ///  the same as the reset count then reset the count
        ///  and send a chat message.
        /// </summary>  
        public void Add() {
            Program.Cache.LootsCount++;
            WriteFile();

            if( Program.Cache.Settings.ResetCounter && Program.Cache.LootsCount == Program.Cache.Settings.ResetAtCount ) {
                ResetCount();
                Program.ChatBot.SendMessage( Program.Cache.Settings.ResetMessage );
            }

        }

        /// <summary>  
        ///  remove 1 from loots count.
        /// </summary>  
        public void Remove() {
            if( Program.Cache.LootsCount == 0 && Program.Cache.Settings.ResetCounter ) {
                Program.Cache.LootsCount = Program.Cache.Settings.ResetAtCount - 1;
                WriteFile();
                Log.Info( "Reverted back and removed 1" );
            }
            else if( Program.Cache.LootsCount != 0 ) {
                Program.Cache.LootsCount--;
                WriteFile();
                Log.Info( "Removed 1 from Loots Count" );
            }
            else {
                Log.Info( "Cant remove when counter is at 0" );
            }
        }

        /// <summary>  
        ///  Reset the loots count.  
        /// </summary>  
        private void ResetCount() {
            Program.Cache.LootsCount = 0;
            WriteFile();
            Log.Info( "Loots count has been reset!" );
        }

        /// <summary>  
        ///  Load the loots file.  
        /// </summary>  
        private void LoadFile() {
            if( !File.Exists( LootsFile ) ) {
                Log.Info( $"{LootsFile} does not exist, Creating file." );
                ResetCount();
            }
            else {
                try {
                    string LootsCountText = "";
                    using( StreamReader readtext = new StreamReader( LootsFile ) ) {
                        LootsCountText = readtext.ReadLine();
                    }

                    Program.Cache.LootsCount = Convert.ToInt16( Regex.Match( LootsCountText.Split( '/' )[0].Trim(), @"\d+" ).Value );

                    Log.Info( $"Original Loots Loaded. Loots is now set to {Program.Cache.LootsCount}" );
                }
                catch( Exception Ex ) {
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
                using( StreamWriter sw = new StreamWriter( LootsFile ) ) {
                    sw.WriteLine( $"{Program.Cache.Settings.ScreenText} {Program.Cache.LootsCount} / {Program.Cache.Settings.ResetAtCount}" );
                }
                Log.Info( "Wrote loots to file." );
            }
            catch( Exception Ex ) {
                Log.Error( "Error Writing loots file", Ex );
                Log.CloseProgram();
            }
        }
    }
}
