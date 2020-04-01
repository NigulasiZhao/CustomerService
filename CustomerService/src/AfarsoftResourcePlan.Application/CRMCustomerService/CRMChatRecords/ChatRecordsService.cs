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
using Abp.Linq.Extensions;

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
        /// 获取聊天记录
        /// </summary>
        /// <param name="historyChatRecordsInput"></param>
        /// <returns></returns>
        public BaseDataOutput<List<HistoryChatRecordsOutput>> HistoryChatRecords(HistoryChatRecordsInput historyChatRecordsInput)
        {
            BaseDataOutput<List<HistoryChatRecordsOutput>> output = new BaseDataOutput<List<HistoryChatRecordsOutput>>();
            List<HistoryChatRecordsOutput> list = _chatRecords.GetAll().WhereIf(!string.IsNullOrEmpty(historyChatRecordsInput.CustomerId), e => e.CustomerId == historyChatRecordsInput.CustomerId)
                .WhereIf(!string.IsNullOrEmpty(historyChatRecordsInput.CustomerDeviceId), e => e.CustomerDeviceId == historyChatRecordsInput.CustomerDeviceId)
                .WhereIf(!string.IsNullOrEmpty(historyChatRecordsInput.ServiceId), e => e.ServiceId == historyChatRecordsInput.ServiceId).
                OrderBy(historyChatRecordsInput.sort + historyChatRecordsInput.order)
                .Skip(historyChatRecordsInput.SkipCount).Take(historyChatRecordsInput.rows)
                .Select(e => new HistoryChatRecordsOutput
                {
                    CustomerId = e.CustomerId,
                    CustomerDeviceId = e.CustomerDeviceId,
                    CustomerCode = e.CustomerCode,
                    CustomerNickName = e.CustomerNickName,
                    CustomerFaceImg = e.CustomerFaceImg,
                    ServiceId = e.ServiceId,
                    ServiceCode = e.ServiceCode,
                    ServiceNickName = e.ServiceNickName,
                    ServiceFaceImg = e.ServiceFaceImg,
                    SendDateTime = e.SendDateTime,
                    SendInfoType = e.SendInfoType,
                    SendContent = e.SendContent,
                    SendSource = e.SendSource
                }).ToList();
            output.Data = list;
            return output;
        }

        /// <summary>
        /// 获取聊天记录列表
        /// </summary>
        /// <param name="historyChatRecordsInput"></param>
        /// <returns></returns>
        public BaseDataOutput<List<HistoryChatRecordsListOutput>> HistoryChatRecordsList(HistoryChatRecordsListInput historyChatRecordsInput)
        {
            BaseDataOutput<List<HistoryChatRecordsListOutput>> output = new BaseDataOutput<List<HistoryChatRecordsListOutput>>();
            var list = _serviceRecords.GetAll()
                .WhereIf(!string.IsNullOrEmpty(historyChatRecordsInput.ServiceId), e => e.ServiceId == historyChatRecordsInput.ServiceId);

            var CustomerList = _customerConnectRecords.GetAll();
            var AllotQuery = from item in (
                             from ServiceRecordsResult in list
                             join CustomerInfo in CustomerList on ServiceRecordsResult.CustomerDeviceId equals CustomerInfo.DeviceId
                             select new
                             {
                                 ServiceRecordsResult.CustomerDeviceId,
                                 CustomerInfo.CustomerId,
                                 CustomerInfo.CustomerNickName,
                                 CustomerInfo.CustomerFaceImg,
                                 CustomerInfo.CustomerCode,
                                 ServiceRecordsResult.CustomerContentDate
                             }
                             )
                             group item by new
                             {
                                 item.CustomerDeviceId,
                                 item.CustomerId,
                                 item.CustomerNickName,
                                 item.CustomerFaceImg,
                                 item.CustomerCode,
                             } into groupChild
                             select new HistoryChatRecordsListOutput()
                             {
                                 CustomerDeviceId = groupChild.Key.CustomerDeviceId,
                                 CustomerId = groupChild.Key.CustomerId,
                                 CustomerNickName = groupChild.Key.CustomerNickName,
                                 CustomerFaceImg = groupChild.Key.CustomerFaceImg,
                                 CustomerCode = groupChild.Key.CustomerCode,
                                 CustomerContentDate = groupChild.Max(e => e.CustomerContentDate)
                             };

            var Result = AllotQuery.OrderBy(historyChatRecordsInput.sort + historyChatRecordsInput.order)
            .Skip(historyChatRecordsInput.SkipCount).Take(historyChatRecordsInput.rows)
           .ToList();
            output.Data = Result;
            return output;
        }
        /// <summary>
        /// 用户断连处理
        /// </summary>
        public void CustomerOnDisconnected(CustomerOnDisconnectedDto customerOnDisconnectedDto)
        {
            ServiceRecords ServiceRecordsModel = _serviceRecords.FirstOrDefault(e => e.Id == customerOnDisconnectedDto.ServiceRecordId);
            ServiceRecordsModel.CustomerUnContentDate = DateTime.Now;
            ServiceRecordsModel.CustomerState = OrderInfo.LoginState.OffLine;
            _serviceRecords.Update(ServiceRecordsModel);

            CustomerConnectRecords CustomerConnectRecordsModel = _customerConnectRecords.FirstOrDefault(e => e.DeviceId == customerOnDisconnectedDto.DeviceId);
            CustomerConnectRecordsModel.CustomerState = OrderInfo.LoginState.OffLine;
            _customerConnectRecords.Update(CustomerConnectRecordsModel);

            ChatRecords ChatRecordsModel = new ChatRecords();
            ChatRecordsModel = EntityHelper.CopyValue(ServiceRecordsModel, ChatRecordsModel);
            ChatRecordsModel.Id = 0;
            ChatRecordsModel.ServiceRecordsId = customerOnDisconnectedDto.ServiceRecordId;
            ChatRecordsModel.SendInfoType = OrderInfo.SendInfoType.TextInfo;
            ChatRecordsModel.SendSource = OrderInfo.TerminalRefer.system;
            ChatRecordsModel.SendContent = "用户下线";
            ChatRecordsModel.SendDateTime = DateTime.Now;
            ChatRecordsModel.ReceiveState = OrderInfo.ReceiveState.Received;
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

            List<ServiceRecords> ServiceRecordsList = _serviceRecords.GetAllList(e => e.ServiceId == servicerOnDisconnectedDto.ServiceId && e.ServiceState == OrderInfo.LoginState.Online);
            foreach (var item in ServiceRecordsList)
            {
                item.ServiceUnContentDate = DateTime.Now;
                item.ServiceState = OrderInfo.LoginState.OffLine;
                _serviceRecords.Update(item);

                ChatRecords ChatRecordsModel = new ChatRecords();
                ChatRecordsModel = EntityHelper.CopyValue(item, ChatRecordsModel);
                ChatRecordsModel.Id = 0;
                ChatRecordsModel.ServiceRecordsId = item.Id;
                ChatRecordsModel.SendInfoType = OrderInfo.SendInfoType.TextInfo;
                ChatRecordsModel.SendSource = OrderInfo.TerminalRefer.system;
                ChatRecordsModel.SendContent = "客服下线";
                ChatRecordsModel.SendDateTime = DateTime.Now;
                ChatRecordsModel.ReceiveState = OrderInfo.ReceiveState.Received;
                _chatRecords.Insert(ChatRecordsModel);
            }
        }
    }
}
