namespace LootsCounter.Models
{
    /// <summary>  
    ///  Model for settings.  
    /// </summary>  
    public class Settings
    {
        public string BotUser { get; set; }
        public string BotOauth { get; set; }
        public string ChannelName { get; set; }
        public string LootsBotUser { get; set; }
        public string LootsLink { get; set; }
        public bool ResetCounter { get; set; }
        public int ResetAtCount { get; set; }
        public string ResetMessage { get; set; }
        public string ScreenText { get; set; }
        public bool UseChannelOwner { get; set; }
        public bool UseModerators { get; set; }
        public string AddRemoveLootsCommand { get; set; }
        public string LootsCountCommand { get; set; }
        public string MutationResponse { get; set; }
        public string LootsCountResponse { get; set; }
        public string ResetCommandResponse { get; set; }

    }
}
