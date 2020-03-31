using AfarsoftResourcePlan.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CRMCustomerService.CRMChatRecords.Dto
{
    public class HistoryChatRecordsInput : BasePageInput
    {
        public string CustomerId { get; set; }

        public string CustomerDeviceId { get; set; }

        public string ServiceId { get; set; }
    }
}
