using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootsCounter.Models.Twitch
{
    /// <summary>  
    ///  Model for commands.  
    /// </summary>
    public class Command
    {
        public string Log { get; set; }
        public string Message { get; set; }
        public Func<string, string, bool> Execute { get; set; }
    }
}
