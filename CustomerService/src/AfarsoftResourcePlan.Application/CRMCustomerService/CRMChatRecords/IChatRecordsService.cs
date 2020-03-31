using Abp.Application.Services;
using AfarsoftResourcePlan.Common;
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
        BaseOutput AddChatRecords(AddChatRecordsDto addChatRecordsDto);
        /// <summary>
        /// 获取聊天记录
        /// </summary>
        /// <param name="historyChatRecordsInput"></param>
        /// <returns></returns>
        BaseDataOutput<List<HistoryChatRecordsOutput>> HistoryChatRecords(HistoryChatRecordsInput historyChatRecordsInput);
        /// <summary>
        /// 用户断连处理
        /// </summary>
        void CustomerOnDisconnected(CustomerOnDisconnectedDto customerOnDisconnectedDto);
        /// <summary>
        /// 客服断连处理
        /// </summary>
        void ServicerOnDisconnected(ServicerOnDisconnectedDto servicerOnDisconnectedDto);
    }
}
