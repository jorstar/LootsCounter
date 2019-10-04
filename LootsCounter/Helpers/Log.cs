using System;
using System.IO;

namespace LootsCounter.Helpers
{
    /// <summary>  
    ///  Log class  
    /// </summary>  
    internal class Log
    {
        internal static bool IsActive { get; set; } = true;
        private static string LogFile { get; } = "LootsLog.txt";

        /// <summary>  
        ///  log errors to console and file.  
        /// </summary>  
        internal static void Error( string message ) {
            WriteLog( $"[ERROR]: {message}", ConsoleColor.DarkRed );
            WriteToFile( $"[ERROR]: {message}" );
        }

        /// <summary>  
        ///  log errors to console and file with exception.  
        /// </summary>
        internal static void Error( string message, Exception ex ) {
            WriteLog( $"[ERROR]: {message}", ex, ConsoleColor.DarkRed );
            WriteToFile( $"[ERROR]: {message}", ex );
        }

        /// <summary>  
        ///  log warnings to console and file.  
        /// </summary>
        internal static void Warning( string message ) {
            WriteLog( $"[WARNING]: {message}", ConsoleColor.Yellow );
            WriteToFile( $"[WARNING]: {message}" );
        }

        /// <summary>  
        ///  log information to console.  
        /// </summary>
        internal static void Info( string message ) {
            WriteLog( $"[Info]: {message}", ConsoleColor.Cyan );
        }

        /// <summary>  
        ///  write message to console.  
        /// </summary>
        private static void WriteLog( string message, ConsoleColor color ) {
            string text = $"[{DateTime.UtcNow}] {message}";

            Console.ForegroundColor = color;
            Console.WriteLine( text );
            Console.ResetColor();

        }

        /// <summary>  
        ///  write message with exception to console.  
        /// </summary>
        private static void WriteLog( string message, Exception ex, ConsoleColor color ) {
            string text = $"[{DateTime.UtcNow}] {message}";
            string exception = $"[{DateTime.UtcNow}] [ErrorMessage] {ex.Message}";

            Console.ForegroundColor = color;
            Console.WriteLine( text );
            Console.WriteLine( exception );
            Console.ResetColor();
        }

        /// <summary>  
        ///  write message to file.  
        /// </summary>
        private static void WriteToFile( string message ) {
            try {
                string text = $"[{DateTime.UtcNow}] {message}";

                using( StreamWriter streamWriter = new StreamWriter( LogFile ) ) {
                    streamWriter.WriteLine( text );
                    streamWriter.Close();
                }
            }
            catch( Exception Ex ) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine( $"[{DateTime.UtcNow}] [Internal Error] Can not write to logfile" );
                Console.WriteLine( $"[{DateTime.UtcNow}] [ErrorMessage] {Ex.Message}" );
                Console.ResetColor();
            }
        }

        /// <summary>  
        ///  write message with exception to file.  
        /// </summary>
        internal static void WriteToFile( string message, Exception ex ) {
            try {
                string text = $"[{DateTime.UtcNow}] {message}";
                string exception = $"[{DateTime.UtcNow}] [ErrorMessage] {ex.Message}";

                using( StreamWriter streamWriter = new StreamWriter( LogFile ) ) {
                    streamWriter.WriteLine( text );
                    streamWriter.WriteLine( exception );
                    streamWriter.Close();
                }
            }
            catch( Exception Ex ) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine( $"[{DateTime.UtcNow}] [Internal Error] Can not write to logfile" );
                Console.WriteLine( $"[{DateTime.UtcNow}] [ErrorMessage] {Ex.Message}" );
                Console.ResetColor();
            }
        }

        /// <summary>  
        ///  stop the program.  
        /// </summary>
        internal static void CloseProgram() {
            Console.WriteLine( "Press any key to close the application..." );
            Console.ReadKey();

            Environment.Exit( 1 );
        }

        /// <summary>  
        ///  Send accept or deny keypress.
        ///  return as bool
        /// </summary>
        internal static bool AcceptOrDeny() {
            ConsoleKeyInfo KeyRead = Console.ReadKey();
            bool val = false;
            if( KeyRead.Key == ConsoleKey.Y ) {
                val = true;
            }
            else if( KeyRead.Key == ConsoleKey.N ) {
                val = false;
            }
            else {
                AcceptOrDeny();
            }

            return val;
        }
    }
}
