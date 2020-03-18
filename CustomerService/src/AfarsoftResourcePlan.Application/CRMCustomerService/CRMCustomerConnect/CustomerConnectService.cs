using Abp.Application.Services;
using Abp.Domain.Repositories;
using AfarsoftResourcePlan.Common;
using AfarsoftResourcePlan.CRMCustomerService.CRMCustomerConnect.Dto;
using AfarsoftResourcePlan.CustomerService;
using AfarsoftResourcePlan.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CRMCustomerService.CRMCustomerConnect
{
    public class CustomerConnectService : ApplicationService, ICustomerConnectService
    {
        private readonly IRepository<CustomerConnectRecords, int> _CustomerConnectRecords;
        private readonly IRepository<ServiceConnectRecords, int> _ServiceConnectRecords;
        private readonly IRepository<ServiceRecords, int> _ServiceRecords;
        private readonly IRepository<ChatRecords, int> _ChatRecords;
        public CustomerConnectService(IRepository<CustomerConnectRecords, int> CustomerConnectRecords
            , IRepository<ServiceConnectRecords, int> ServiceConnectRecords
            , IRepository<ServiceRecords, int> ServiceRecords
            , IRepository<ChatRecords, int> ChatRecords)
        {
            _CustomerConnectRecords = CustomerConnectRecords;
            _ServiceConnectRecords = ServiceConnectRecords;
            _ServiceRecords = ServiceRecords;
            _ChatRecords = ChatRecords;
        }
        /// <summary>
        /// 添加客户,并建立连接
        /// </summary>
        /// <param name="addCustomerConnectRecordsDto"></param>
        public BaseDataOutput<int> AddServiceConnectRecords(AddCustomerConnectRecordsDto addCustomerConnectRecordsDto)
        {
            BaseDataOutput<int> output = new BaseDataOutput<int>();
            int CustomerConnectRecordsId = 0;
            int ServiceRecordsId = 0;
            ChatRecords ChatRecordsModel = new ChatRecords();
            //处理客户记录表
            CustomerConnectRecords CustomerConnectRecordsModel = _CustomerConnectRecords.FirstOrDefault(e => e.DeviceId == addCustomerConnectRecordsDto.DeviceId);
            if (CustomerConnectRecordsModel == null)
            {
                CustomerConnectRecords NewCustomerConnectRecordsModel = new CustomerConnectRecords();
                NewCustomerConnectRecordsModel = EntityHelper.CopyValue(addCustomerConnectRecordsDto, NewCustomerConnectRecordsModel);
                NewCustomerConnectRecordsModel.CustomerState = OrderInfo.LoginState.Online;
                CustomerConnectRecordsId = _CustomerConnectRecords.InsertAndGetId(NewCustomerConnectRecordsModel);
            }
            else
            {
                CustomerConnectRecordsModel.CustomerState = OrderInfo.LoginState.Online;
                _CustomerConnectRecords.Update(CustomerConnectRecordsModel);
                CustomerConnectRecordsId = CustomerConnectRecordsModel.Id;
            }
            //处理连接记录表
            ServiceRecords ServiceRecordsModel = new ServiceRecords();
            ServiceRecordsModel = EntityHelper.CopyValue(addCustomerConnectRecordsDto, ServiceRecordsModel);
            ServiceRecordsModel.CustomerDeviceId = addCustomerConnectRecordsDto.DeviceId;
            //连接记录表-处理客户信息
            ServiceRecordsModel.CustomerConnectRecordsId = CustomerConnectRecordsId;
            ServiceRecordsModel.CustomerContentDate = DateTime.Now;
            ServiceRecordsModel.CustomerState = OrderInfo.LoginState.Online;
            //连接记录表-如果匹配到客服，则处理客服信息
            if (addCustomerConnectRecordsDto.ServiceId != null)
            {
                ServiceConnectRecords ServiceConnectRecordsModel = _ServiceConnectRecords.FirstOrDefault(e => e.ServiceId == addCustomerConnectRecordsDto.ServiceId);
                if (ServiceConnectRecordsModel != null)
                {
                    //连接记录表-处理客服信息
                    ServiceRecordsModel = EntityHelper.CopyValue(ServiceConnectRecordsModel, ServiceRecordsModel);
                    ServiceRecordsModel.Id = 0;
                    ServiceRecordsModel.ServiceConnectRecordsId = ServiceConnectRecordsModel.Id;
                    ServiceRecordsModel.ServiceId = addCustomerConnectRecordsDto.ServiceId;
                    ServiceRecordsModel.ServiceContentDate = DateTime.Now;
                    ServiceRecordsModel.ServiceState = OrderInfo.LoginState.Online;
                    ServiceConnectRecordsModel.ServiceCount += 1;
                    _ServiceConnectRecords.Update(ServiceConnectRecordsModel);
                    //聊天记录表-处理客服信息
                    ChatRecordsModel = EntityHelper.CopyValue(ServiceConnectRecordsModel, ChatRecordsModel);
                    ChatRecordsModel.ServiceId = addCustomerConnectRecordsDto.ServiceId;
                }
            }
            ServiceRecordsId = _ServiceRecords.InsertAndGetId(ServiceRecordsModel);
            //聊天记录表-处理客户信息
            ChatRecordsModel = EntityHelper.CopyValue(addCustomerConnectRecordsDto, ChatRecordsModel);
            ChatRecordsModel.Id = 0;
            ChatRecordsModel.CustomerDeviceId = addCustomerConnectRecordsDto.DeviceId;
            //聊天记录表-处理聊天信息
            ChatRecordsModel.ServiceRecordsId = ServiceRecordsId;
            ChatRecordsModel.SendInfoType = OrderInfo.SendInfoType.TextInfo;
            ChatRecordsModel.SendSource = OrderInfo.TerminalRefer.system;
            ChatRecordsModel.SendContent = "客服["+ ChatRecordsModel.ServiceNickName+ "]为您服务";
            ChatRecordsModel.SendDateTime = DateTime.Now;
            ChatRecordsModel.ReceiveState = OrderInfo.ReceiveState.Received;
            _ChatRecords.Insert(ChatRecordsModel);
            output.Data = ServiceRecordsId;
            return output;
        }
    }
}
