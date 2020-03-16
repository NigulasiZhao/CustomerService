using Abp.Application.Services;
using AfarsoftResourcePlan.CRMCustomerService.CRMChatRecords.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CRMCustomerService.CRMChatRecords
{
    public interface IChatRecordsService: IApplicationService
    {
        /// <summary>
        /// 添加聊天记录
        /// </summary>
        /// <param name="addChatRecordsDto"></param>
        void AddChatRecords(AddChatRecordsDto addChatRecordsDto);
    }
}
