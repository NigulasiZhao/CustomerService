using Abp.Application.Services;
using Abp.Domain.Repositories;
using AfarsoftResourcePlan.CRMCustomerService.CRMCustomerConnect.Dto;
using AfarsoftResourcePlan.CustomerService;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CRMCustomerService.CRMCustomerConnect
{
    public class CustomerConnectService : ApplicationService, ICustomerConnectService
    {
        private readonly IRepository<CustomerConnectRecords, int> _CustomerConnectRecords;
        public CustomerConnectService(IRepository<CustomerConnectRecords, int> CustomerConnectRecords)
        {
            _CustomerConnectRecords = CustomerConnectRecords;
        }
        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="addCustomerConnectRecordsDto"></param>
        public void AddServiceConnectRecords(AddCustomerConnectRecordsDto addCustomerConnectRecordsDto)
        {
            CustomerConnectRecords CustomerConnectRecordsModel = _CustomerConnectRecords.FirstOrDefault(e => e.DeviceId == addCustomerConnectRecordsDto.DeviceId);
            if (CustomerConnectRecordsModel == null)
            {
                _CustomerConnectRecords.Insert(new CustomerConnectRecords
                {
                    DeviceId = addCustomerConnectRecordsDto.DeviceId,
                    CustomerId = addCustomerConnectRecordsDto.CustomerId,
                    OpenId = addCustomerConnectRecordsDto.OpenId,
                    UnionId = addCustomerConnectRecordsDto.UnionId,
                    CustomerCode = addCustomerConnectRecordsDto.CustomerCode,
                    CustomerNickName = addCustomerConnectRecordsDto.CustomerNickName,
                    CustomerFaceImg = addCustomerConnectRecordsDto.CustomerFaceImg,
                    CustomerState = OrderInfo.LoginState.Online
                });
            }
            else
            {
                CustomerConnectRecordsModel.CustomerState = OrderInfo.LoginState.Online;
                _CustomerConnectRecords.Update(CustomerConnectRecordsModel);
            }
        }
    }
}
