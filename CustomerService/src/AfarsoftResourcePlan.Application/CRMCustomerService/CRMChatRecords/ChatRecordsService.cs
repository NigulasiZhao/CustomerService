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
using AfarsoftResourcePlan.Common;

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
        public BaseOutput AddChatRecords(AddChatRecordsDto addChatRecordsDto)
        {
            BaseOutput output = new BaseOutput();
            ServiceRecords ServiceRecordsModel = _serviceRecords.GetAllList(e => e.ServiceId == addChatRecordsDto.ServicerId && e.CustomerDeviceId == addChatRecordsDto.CustomerDeviceId)
                .OrderByDescending(e => e.CustomerContentDate).FirstOrDefault();
            ChatRecords ChatRecordsModel = new ChatRecords();
            ChatRecordsModel = EntityHelper.CopyValue(ServiceRecordsModel, ChatRecordsModel);
            ChatRecordsModel.Id = 0;
            ChatRecordsModel.ServiceRecordsId = ServiceRecordsModel.Id;
            ChatRecordsModel.SendSource = addChatRecordsDto.SendSource;
            ChatRecordsModel.SendDateTime = DateTime.Now;
            ChatRecordsModel.SendContent = addChatRecordsDto.SendContent;
            ChatRecordsModel.ReceiveState = OrderInfo.ReceiveState.Received;
            _chatRecords.Insert(ChatRecordsModel);
            return output;
        }
        /// <summary>
        /// 用户断连处理
        /// </summary>
        public void CustomerOnDisconnected(CustomerOnDisconnectedDto customerOnDisconnectedDto)
        {
            ServiceRecords ServiceRecordsModel = _serviceRecords.FirstOrDefault(e => e.Id == customerOnDisconnectedDto.ServiceRecordId);
            ServiceRecordsModel.CustomerUnContentDate = DateTime.Now;
            _serviceRecords.Update(ServiceRecordsModel);

            CustomerConnectRecords CustomerConnectRecordsModel = _customerConnectRecords.FirstOrDefault(e => e.DeviceId == customerOnDisconnectedDto.DeviceId);
            CustomerConnectRecordsModel.CustomerState = OrderInfo.LoginState.OffLine;
            _customerConnectRecords.Update(CustomerConnectRecordsModel);

            ChatRecords ChatRecordsModel = new ChatRecords();
            ChatRecordsModel = EntityHelper.CopyValue(ServiceRecordsModel, ChatRecordsModel);
            ChatRecordsModel.ServiceRecordsId = customerOnDisconnectedDto.ServiceRecordId;
            ChatRecordsModel.SendInfoType = OrderInfo.SendInfoType.TextInfo;
            ChatRecordsModel.SendSource = OrderInfo.TerminalRefer.system;
            ChatRecordsModel.SendContent = "用户下线";
            ChatRecordsModel.SendDateTime = DateTime.Now;
            _chatRecords.Insert(ChatRecordsModel);
        }
        /// <summary>
        /// 客服断连处理
        /// </summary>
        public void ServicerOnDisconnected(ServicerOnDisconnectedDto servicerOnDisconnectedDto)
        {
            ServiceConnectRecords ServiceConnectRecordsModel = _serviceConnectRecords.FirstOrDefault(e => e.ServiceId == servicerOnDisconnectedDto.ServiceId);
            ServiceConnectRecordsModel.ServiceState = OrderInfo.LoginState.OffLine;
            _serviceConnectRecords.Update(ServiceConnectRecordsModel);
            foreach (var item in servicerOnDisconnectedDto.ServiceRecordIds)
            {
                ServiceRecords ServiceRecordsModel = _serviceRecords.FirstOrDefault(e => e.Id == item);
                ServiceRecordsModel.ServiceUnContentDate = DateTime.Now;
                _serviceRecords.Update(ServiceRecordsModel);

                ChatRecords ChatRecordsModel = new ChatRecords();
                ChatRecordsModel = EntityHelper.CopyValue(ServiceRecordsModel, ChatRecordsModel);
                ChatRecordsModel.ServiceRecordsId = item;
                ChatRecordsModel.SendInfoType = OrderInfo.SendInfoType.TextInfo;
                ChatRecordsModel.SendSource = OrderInfo.TerminalRefer.system;
                ChatRecordsModel.SendContent = "客服下线";
                ChatRecordsModel.SendDateTime = DateTime.Now;
                _chatRecords.Insert(ChatRecordsModel);
            }
        }
    }
}
