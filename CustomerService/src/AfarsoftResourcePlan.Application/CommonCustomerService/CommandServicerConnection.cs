using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CommonCustomerService
{
    public class CommandServicerConnection : Command<CommandServicerConnection>
    {
        public string nickName { get; set; }

        public string faceimg { get; set; }

        public string servicerId { get; set; }

        public string deviceId { get; set; }
    }
}
