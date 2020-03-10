using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRDemo.CommandModel
{
    public class CommandResult
    {
        public int code { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
    }
}
