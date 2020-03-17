using AfarsoftResourcePlan.OrderInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CommonCustomerService
{
    public class CommandDisConnectionMessage : Command<CommandDisConnectionMessage>
    {
        public string servicerTerminalId { get; set; }

        public string userTerminalId { get; set; }

        public TerminalRefer fromTerminal { get; set; }
    }
}
