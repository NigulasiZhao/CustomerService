using Abp.Application.Services;
using Abp.Domain.Repositories;
using AfarsoftResourcePlan.CustomerService;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CRMCustomerService.CRMChatRecords
{
    /// <summary>
    /// 
    /// </summary>
    public class ChatRecordsService : ApplicationService, IChatRecordsService
    {
        /// <summary>
        /// 聊天记录表
        /// </summary>
        private readonly IRepository<ChatRecords, int> _chatRecords;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="chatRecords">聊天记录表</param>
        public ChatRecordsService(IRepository<ChatRecords, int> chatRecords)
        {
            _chatRecords = chatRecords;
        }

        public string GetChatRecords()
        {
            return "";
        }
    }
}
