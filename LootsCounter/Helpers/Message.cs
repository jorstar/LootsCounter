using System;
using System.Collections.Generic;

namespace LootsCounter.Helpers
{
    internal class Message : ProgramAccessor
    {
        private Dictionary<string, Func<string>> placeHolders = new Dictionary<string, Func<string>>();

        internal Message( Program program ) : base( program ) {
            placeHolders.Add( "[lootscount]", program.Counter.GetCount );
        }

        internal string ReplacePlaceholders( string message ) {

            foreach( KeyValuePair<string, Func<string>> item in placeHolders ) {
                message = message.Replace( item.Key, item.Value.Invoke() );
            }

            return message;
        }
    }
}
