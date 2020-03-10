using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRDemo.CommandModel
{
    public class CommandImageMessage : Command<CommandImageMessage>
    {
        public string userTerminalId { get; set; }

        public string servicerTerminalId { get; set; }

        public TerminalRefer fromTerminal { get; set; }

        public string content { get; set; }
    }
}
