using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CRMCustomerService.CRMChatRecords.Dto
{
    public class ServicerOnDisconnectedDto
    {
        public Guid ServiceId { get; set; }

        public int[] ServiceRecordIds { get; set; }
    }
}
