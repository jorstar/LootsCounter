using System;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Events.Client;

namespace LootsCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            Settings settings = new Settings();
            settings.LoadSettings();

            TwitchChatBot bot = new TwitchChatBot();
            bot.Connect();

            Console.ReadLine();

            bot.Disconnect();
        }
    }
}
