using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootsCounter.Helpers
{
    /// <summary>  
    ///  Cache everything needed.  
    /// </summary>  
    internal class Cache : LootsClientAccessor
    {
        public int LootsCount { get; set; }
        public Models.Settings Settings { get; set; }
        public bool ChannelOwnerOnly { get; set; }

        internal Cache( LootsClient lootsClient ) : base( lootsClient ) {

        }
    }
}
