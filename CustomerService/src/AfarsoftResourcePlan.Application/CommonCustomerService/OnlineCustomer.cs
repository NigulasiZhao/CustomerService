using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CommonCustomerService
{
    public class OnlineCustomer
    {
        public string nickName { get; set; }
        public string faceimg { get; set; }
        public string userId { get; set; }
        public string deviceId { get; set; }

        public string ConnectionId { get; set; }

        public string servicerTerminalId { get; set; }

        public int ServiceRecordId { get; set; }
    }
}
