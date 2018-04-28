using System;
using System.Text.RegularExpressions;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Events.Client;

namespace LootsCounter
{
    internal class TwitchChatBot
    {
        TwitchClient client;
        Loots LootsCounter = new Loots();

        internal void Connect()
        {
            LootsCounter.FirstLaunch();
            Console.WriteLine("Connecting");
            try
            {
                ConnectionCredentials credentials = new ConnectionCredentials(Settings.CurrentSettings.BotUser, Settings.CurrentSettings.BotOauth);

                client = new TwitchClient(credentials, Settings.CurrentSettings.ChannelName, logging: false);
                client.ChatThrottler = new TwitchLib.Services.MessageThrottler(20 / 2, TimeSpan.FromSeconds(30));

                client.OnConnectionError += Client_OnConnectionError;
                client.OnMessageReceived += Client_OnMessageReceived;

                client.Connect();
                Console.WriteLine("Connected");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Waiting for loots messages");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any key to close the application...");
                Console.ReadKey();

                Environment.Exit(1);
            }
            

            
            
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Username.ToLower() == Settings.CurrentSettings.LootsBotUser.ToLower())
            {
                if (e.ChatMessage.Message.ToLower().Contains("https://loots.com/"))
                {
                    if(e.ChatMessage.Message.Contains(Settings.CurrentSettings.LootsLink))
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("loots Command used.");
                    }
                    else if (e.ChatMessage.Message.ToLower().Contains("https://loots.com/t/"))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;

                        if (LootsCounter.AddLootsCounter())
                        {
                            client.SendMessage(Settings.CurrentSettings.ResetMessage);
                        }                       
                    }
                }
            }
        }

        private void Client_OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine($"Error!! {e.Error}");
        }


        public TwitchChatBot()
        {
        }

        internal void Disconnect()
        {
            Console.WriteLine("Disconnecting");
            client.Disconnect();
        }
    }
}