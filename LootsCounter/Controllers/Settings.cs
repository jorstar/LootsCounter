
using LootsCounter.Helpers;
using System;
using System.IO;
using System.Xml.Serialization;

namespace LootsCounter.Controllers
{
    /// <summary>  
    ///  Controller for settings.  
    /// </summary>  
    internal class Settings : LootsClientAccessor
    {
        private string SettingsFile { get; } = "Config.xml";

        internal Settings( LootsClient lootsClient ) : base( lootsClient ) {
            if( !File.Exists( SettingsFile ) ) {
                LoadDefault();
            }

            Models.Settings settings = ReadConfig();
            lootsClient.Cache.Settings = settings;
            lootsClient.Cache.ChannelOwnerOnly = (settings.UseChannelOwner == true && settings.UseModerators == false || settings.UseChannelOwner == false && settings.UseModerators == false);
        }

        /// <summary>  
        ///  Load the default setting values.  
        /// </summary>  
        private void LoadDefault() {
            try {
                Models.Settings settings = new Models.Settings {
                    BotUser = "Username of the bot",
                    BotOauth = "Oauth of the bot",
                    ChannelName = "Channel name to join",
                    LootsBotUser = "Username of bot that sends loots messages",
                    LootsLink = "Your own loots link",
                    ResetCounter = true,
                    ResetAtCount = 25,
                    ResetMessage = "Loots counter has been reset!",
                    ScreenText = "Loots:",
                    UseChannelOwner = true,
                    UseModerators = false,
                    AddRemoveLootsCommand = "LootsCounter",
                    LootsCountCommand = "LootsCount",
                    MutationResponse = "Mutated the loots count",
                    LootsCountResponse = "Current Loots count is [lootscount]",
                    ResetCommandResponse = "Loots counter has been reset"
                };

                using( FileStream fs = new FileStream( SettingsFile, FileMode.CreateNew ) ) {
                    XmlSerializer Xml = new XmlSerializer( settings.GetType() );
                    Xml.Serialize( fs, settings );
                }

                Log.Info( $"Settings file {SettingsFile} Created!" );
                Log.Info( $"Change the settings to your likings and reboot the program!" );
                Log.CloseProgram();
            }
            catch( Exception Ex ) {
                Log.Error( $"Could not write file {SettingsFile} Something went wrong!", Ex );
                Log.CloseProgram();
            }
        }

        /// <summary>  
        ///  Read the config file output a setting model.  
        /// </summary>  
        private Models.Settings ReadConfig() {
            try {
                using( FileStream fs = new FileStream( SettingsFile, FileMode.Open ) ) {
                    XmlSerializer Xml = new XmlSerializer( typeof( Models.Settings ) );
                    return (Models.Settings)Xml.Deserialize( fs );
                }
            }
            catch( Exception Ex ) {
                Log.Error( $"Could not read file {SettingsFile} Something went wrong!", Ex );
                Log.CloseProgram();
                return null;
            }
        }
    }
}

