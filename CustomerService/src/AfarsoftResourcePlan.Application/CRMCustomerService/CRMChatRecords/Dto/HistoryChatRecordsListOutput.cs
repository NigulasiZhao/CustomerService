using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CRMCustomerService.CRMChatRecords.Dto
{
    public class HistoryChatRecordsListOutput
    {
        public string CustomerDeviceId { get; set; }

        public string CustomerId { get; set; }

        public string CustomerNickName { get; set; }

        public string CustomerFaceImg { get; set; }

        public string CustomerCode { get; set; }

        public DateTime? CustomerContentDate { get; set; }
    }
}
