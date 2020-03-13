using AfarsoftResourcePlan.OrderInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CommonCustomerService
{
    public class CommandTextMessage : Command<CommandTextMessage>
    {
        public string userTerminalId { get; set; }

        public string servicerTerminalId { get; set; }

        public TerminalRefer fromTerminal { get; set; }

        public string content { get; set; }
    }
}
