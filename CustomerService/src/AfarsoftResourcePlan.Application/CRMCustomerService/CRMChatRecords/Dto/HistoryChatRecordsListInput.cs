using AfarsoftResourcePlan.Common;
using AfarsoftResourcePlan.OrderInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CRMCustomerService.CRMChatRecords.Dto
{
    public class HistoryChatRecordsListInput : BasePageInput
    {
        public string ServiceId { get; set; }
        public string SearchText { get; set; }
        public LoginState? LoginState { get; set; }
    }
}
