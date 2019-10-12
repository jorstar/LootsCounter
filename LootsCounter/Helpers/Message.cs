using System;
using System.Collections.Generic;

namespace LootsCounter.Helpers
{
    internal class Message : LootsClientAccessor
    {
        private Dictionary<string, Func<string>> placeHolders = new Dictionary<string, Func<string>>();

        internal Message( LootsClient lootsClient ) : base( lootsClient ) {
            placeHolders.Add( "[lootscount]", lootsClient.Counter.GetCount );
        }

        internal string ReplacePlaceholders( string message ) {

            foreach( KeyValuePair<string, Func<string>> item in placeHolders ) {
                message = message.Replace( item.Key, item.Value.Invoke() );
            }

            return message;
        }
    }
}
