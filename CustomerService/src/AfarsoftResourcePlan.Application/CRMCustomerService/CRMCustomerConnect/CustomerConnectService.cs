using Abp.Application.Services;
using Abp.Domain.Repositories;
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
        public void AddServiceConnectRecords(AddCustomerConnectRecordsDto addCustomerConnectRecordsDto)
        {
            int CustomerConnectRecordsId = 0;
            int ServiceRecordsId = 0;
            ChatRecords ChatRecordsModel = new ChatRecords();
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
            ServiceRecords ServiceRecordsModel = new ServiceRecords();
            ServiceRecordsModel = EntityHelper.CopyValue(addCustomerConnectRecordsDto, ServiceRecordsModel);
            //处理客户信息
            ServiceRecordsModel.CustomerConnectRecordsId = CustomerConnectRecordsId;
            ServiceRecordsModel.CustomerContentDate = DateTime.Now;
            ServiceRecordsModel.CustomerState = OrderInfo.LoginState.Online;
            //如果匹配到客服，则处理客服信息
            if (addCustomerConnectRecordsDto.ServiceId != null)
            {
                ServiceConnectRecords ServiceConnectRecordsModel = _ServiceConnectRecords.FirstOrDefault(e => e.ServiceId == addCustomerConnectRecordsDto.ServiceId.Value);
                if (ServiceConnectRecordsModel != null)
                {
                    ServiceRecordsModel = EntityHelper.CopyValue(ServiceConnectRecordsModel, ServiceRecordsModel);
                    ServiceRecordsModel.ServiceConnectRecordsId = ServiceConnectRecordsModel.Id;
                    ServiceRecordsModel.ServiceId = addCustomerConnectRecordsDto.ServiceId.Value;
                    ServiceRecordsModel.ServiceContentDate = DateTime.Now;
                    ServiceRecordsModel.ServiceState = OrderInfo.LoginState.Online;

                    ChatRecordsModel.ServiceId = addCustomerConnectRecordsDto.ServiceId.Value;
                    ChatRecordsModel = EntityHelper.CopyValue(ServiceConnectRecordsModel, ChatRecordsModel);
                }
            }
            ServiceRecordsId = _ServiceRecords.InsertAndGetId(ServiceRecordsModel);
            ChatRecordsModel = EntityHelper.CopyValue(addCustomerConnectRecordsDto, ChatRecordsModel);
            ChatRecordsModel.ServiceRecordsId = ServiceRecordsId;
            ChatRecordsModel.SendInfoType = OrderInfo.SendInfoType.TextInfo;
            ChatRecordsModel.SendSource = OrderInfo.InfoSource.System;
            ChatRecordsModel.SendContent = "开始服务";
            ChatRecordsModel.SendDateTime = DateTime.Now;
            _ChatRecords.Insert(ChatRecordsModel);
        }
    }
}
