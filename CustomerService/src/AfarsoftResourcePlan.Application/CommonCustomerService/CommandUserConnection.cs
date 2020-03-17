using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CommonCustomerService
{
    public class CommandUserConnection : Command<CommandUserConnection>
    {
        public string nickName { get; set; }

        public string faceImg { get; set; }

        public string userId { get; set; }

        public string deviceId { get; set; }
    }
}
