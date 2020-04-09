using AfarsoftResourcePlan.OrderInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CommonCustomerService
{
    public class CommandHistoryChatRecordsList
    {
        public string ServiceId { get; set; }

        public string SearchText { get; set; }
        public LoginState? LoginState { get; set; }
        public int Page { get; set; }

        public int Rows { get; set; }
    }
}
