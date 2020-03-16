using Abp.Application.Services;
using Abp.Domain.Repositories;
using AfarsoftResourcePlan.CRMCustomerService.CRMChatRecords.Dto;
using AfarsoftResourcePlan.CustomerService;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Linq;
using AfarsoftResourcePlan.Helper;

namespace AfarsoftResourcePlan.CRMCustomerService.CRMChatRecords
{
    /// <summary>
    /// 
    /// </summary>
    public class ChatRecordsService : ApplicationService, IChatRecordsService
    {
        /// <summary>
        /// 聊天记录主表
        /// </summary>
        private readonly IRepository<ServiceRecords, int> _serviceRecords;
        /// <summary>
        /// 聊天记录子表
        /// </summary>
        private readonly IRepository<ChatRecords, int> _chatRecords;

        private readonly IRepository<CustomerConnectRecords, int> _customerConnectRecords;

        private readonly IRepository<ServiceConnectRecords, int> _serviceConnectRecords;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="chatRecords">聊天记录表</param>
        public ChatRecordsService(IRepository<ChatRecords, int> chatRecords, IRepository<ServiceRecords, int> serviceRecords,
            IRepository<CustomerConnectRecords, int> customerConnectRecords, IRepository<ServiceConnectRecords, int> serviceConnectRecords)
        {
            _chatRecords = chatRecords;
            _serviceRecords = serviceRecords;
            _customerConnectRecords = customerConnectRecords;
            _serviceConnectRecords = serviceConnectRecords;
        }
        /// <summary>
        /// 添加聊天记录
        /// </summary>
        /// <param name="addChatRecordsDto"></param>
        public void AddChatRecords(AddChatRecordsDto addChatRecordsDto)
        {
            //ServiceRecords ServiceRecordsModel = _serviceRecords.GetAll()
            //    .Where(e => e.CustomerDeviceId == addChatRecordsDto.CustomerDeviceId && e.ServiceId == Guid.Parse(addChatRecordsDto.ServicerId))
            //    .OrderByDescending(e => e.CustomerContentDate).FirstOrDefault();
            ServiceRecords ServiceRecordsModel = _serviceRecords.FirstOrDefault(e => e.Id == addChatRecordsDto.ServiceRecordId);
            ChatRecords ChatRecordsModel = new ChatRecords();
            ChatRecordsModel = EntityHelper.CopyValue(ServiceRecordsModel, ChatRecordsModel);
            ChatRecordsModel.ServiceRecordsId = ServiceRecordsModel.Id;
            ChatRecordsModel.SendSource = addChatRecordsDto.SendSource;
            ChatRecordsModel.SendDateTime = DateTime.Now;
            ChatRecordsModel.SendContent = addChatRecordsDto.SendContent;
            ChatRecordsModel.ReceiveState = OrderInfo.ReceiveState.Received;
            _chatRecords.Insert(ChatRecordsModel);
        }
        /// <summary>
        /// 用户断连处理
        /// </summary>
        public void CustomerOnDisconnected() 
        { 
        
        
        }
        /// <summary>
        /// 客服断连处理
        /// </summary>
        public void ServicerOnDisconnected()
        {


        }
    }
}
