using System;
using System.IO;
using System.Xml.Serialization;

namespace LootsCounter
{
    public class Config
    {
        public static string ConfigName = "Config.xml";

        public static Settings ReadConfig()
        {
            Console.WriteLine("Reading Config");
            using (FileStream fs = new FileStream(ConfigName, FileMode.Open))
            {
                XmlSerializer Xml = new XmlSerializer(typeof(Settings));
                return (Settings)Xml.Deserialize(fs);
            }
        }

        public static void WriteConfig()
        {
            Console.WriteLine("Creating Congfig File!");
            Settings DefaultSettings = new Settings();
            DefaultSettings.BotUser = "Username of the bot";
            DefaultSettings.BotOauth = "Oauth of the bot";
            DefaultSettings.ChannelName = "Channel name to join";
            DefaultSettings.LootsBotUser = "Username of bot that sends loots messages";
            DefaultSettings.LootsLink = "Your own loots link";
            DefaultSettings.ResetCounter = true;
            DefaultSettings.ResetAtCount = 25;
            DefaultSettings.ResetMessage = "Loots counter has been reset!";
            DefaultSettings.ScreenText = "Loots:";

            using (FileStream fs = new FileStream(ConfigName, FileMode.CreateNew))
            {
                XmlSerializer Xml = new XmlSerializer(DefaultSettings.GetType());
                Xml.Serialize(fs, DefaultSettings);
            }
            Console.WriteLine("Config file created!");
        }

        public static bool ConfigExists()
        {
            return File.Exists(ConfigName);
        }
    }
}
