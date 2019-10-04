using System;
using LootsCounter.Helpers;
using LootsCounter.Controllers;
using LootsCounter.Controllers.Twitch;
using LootsCounter.Controllers.Loots;

namespace LootsCounter
{
    internal class Program
    {
        public static Program ActiveInstance { get; set; }
        internal Cache Cache { get; }
        internal Settings Settings { get; }
        internal ChatBot ChatBot { get; }
        internal Commands Commands { get; }
        internal Counter Counter { get; }
        internal Message MessageHelper { get; }

        static void Main( string[] args ) {
            Program p = new Program();

            while ( true )
            {
                Console.ReadKey();
            };
        }

        public Program() {
            Cache = new Cache( this );
            Settings = new Settings( this );
            ChatBot = new ChatBot( this );
            Commands = new Commands( this );
            Counter = new Counter( this );
            MessageHelper = new Message( this );

            ActiveInstance = this;

            Commands.PrepareCommands();
        }
    }
}
