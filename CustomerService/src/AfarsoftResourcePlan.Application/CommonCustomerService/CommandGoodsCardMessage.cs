using AfarsoftResourcePlan.OrderInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CommonCustomerService
{
    public class CommandGoodsCardMessage : Command<CommandGoodsCardMessage>
    {
        public string userTerminalId { get; set; }

        public string servicerTerminalId { get; set; }

        public TerminalRefer fromTerminal { get; set; }

        public GoodsCardContentDetail content { get; set; }
    }

    public class GoodsCardContentDetail
    {
        public string title { get; set; }

        public string description { get; set; }

        public string image { get; set; }
        public string price { get; set; }

    }
}
