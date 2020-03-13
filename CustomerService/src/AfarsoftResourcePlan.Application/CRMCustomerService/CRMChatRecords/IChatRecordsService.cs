using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CRMCustomerService.CRMChatRecords
{
    public interface IChatRecordsService: IApplicationService
    {
        string GetChatRecords();
    }
}
