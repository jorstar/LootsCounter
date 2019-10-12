using System;
using LootsCounter.Helpers;
using LootsCounter.Controllers;
using LootsCounter.Controllers.Twitch;
using LootsCounter.Controllers.Loots;

namespace LootsCounter
{
    internal class Program
    {
        static void Main( string[] args ) {
            LootsClient p = new LootsClient();

            while( true ) {
                Console.ReadKey();

            }
        }

    }
}
