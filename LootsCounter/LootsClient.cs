using LootsCounter.Controllers;
using LootsCounter.Controllers.Loots;
using LootsCounter.Controllers.Twitch;
using LootsCounter.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LootsCounter
{
    class LootsClient
    {
        public static LootsClient ActiveInstance { get; set; }
        internal Cache Cache { get; }
        internal Settings Settings { get; }
        internal ChatBot ChatBot { get; }
        internal Commands Commands { get; }
        internal Counter Counter { get; }
        internal Message MessageHelper { get; }

        public LootsClient() {
            Cache = new Cache( this );
            Thread.Sleep( 1 );
            Settings = new Settings( this );
            Thread.Sleep( 1 );
            ChatBot = new ChatBot( this );
            Thread.Sleep( 1 );
            Commands = new Commands( this );
            Thread.Sleep( 1 );
            Counter = new Counter( this );
            Thread.Sleep( 1 );
            MessageHelper = new Message( this );

            ActiveInstance = this;

            Commands.PrepareCommands();
        }
    }
}
