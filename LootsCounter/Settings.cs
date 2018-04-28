using System;

namespace LootsCounter
{
    public class Settings
    {
        private string ConfigName {
            get
            {
                return "Config.xml";
            }
        }

        public string BotUser { get; set; }
        public string BotOauth { get; set; }
        public string ChannelName { get; set; }
        public string LootsBotUser { get; set; }
        public string LootsLink { get; set; }
        public bool ResetCounter { get; set; }
        public int ResetAtCount { get; set; }
        public string ResetMessage { get; set; }
        public string ScreenText { get; set; }

        public static Settings CurrentSettings { get; set; }
        

        public void LoadSettings()
        {
            if (!Config.ConfigExists())
            {
                try
                {
                    Config.WriteConfig();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Make sure to Setup the configuration!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press any key to close the application...");
                    Console.ReadKey();

                    Environment.Exit(1);
                }
                catch(Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Something went wrong: {e.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press any key to close the application...");
                    Console.ReadKey();

                    Environment.Exit(1);                    
                }
                
            }
            else
            {
                try
                {
                    Console.WriteLine("Loading settings!");
                    CurrentSettings = Config.ReadConfig();
                    Console.WriteLine("Loaded Settings!");
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Something went wrong: {e.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press any key to close the application...");
                    Console.ReadKey();

                    Environment.Exit(1);
                }
            }            
        }
    }
}
