using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRDemo.CommandModel
{
    public class CommandDisConnectionMessage : Command<CommandDisConnectionMessage>
    {
        public string servicerTerminalId { get; set; }

        public string userTerminalId { get; set; }

        public TerminalRefer fromTerminal { get; set; }
    }
}
