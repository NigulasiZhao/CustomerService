using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CommonCustomerService
{
    public class CommandHistoryChatRecords
    {
        public string CustomerDeviceId { get; set; }
        public string CustomerId { get; set; }

        public string ServiceId { get; set; }
        public int Page { get; set; }

        public int Rows { get; set; }
    }
}
