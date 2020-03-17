using Abp.Application.Services;
using Abp.Domain.Repositories;
using AfarsoftResourcePlan.Common;
using AfarsoftResourcePlan.CRMCustomerService.CRMServiceConnect.Dto;
using AfarsoftResourcePlan.CustomerService;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CRMCustomerService.CRMServiceConnect
{
    public class ServiceConnectService : ApplicationService, IServiceConnectService
    {
        private readonly IRepository<ServiceConnectRecords, int> _ServiceConnectRecords;
        public ServiceConnectService(IRepository<ServiceConnectRecords, int> ServiceConnectRecords)
        {
            _ServiceConnectRecords = ServiceConnectRecords;
        }
        /// <summary>
        /// 客服表添加记录
        /// </summary>
        /// <param name="addServiceConnectRecordsDto"></param>
        public BaseOutput AddServiceConnectRecords(AddServiceConnectRecordsDto addServiceConnectRecordsDto)
        {
            BaseOutput output = new BaseOutput();
            ServiceConnectRecords ServiceConnectRecordsModel = _ServiceConnectRecords.FirstOrDefault(e => e.ServiceId == addServiceConnectRecordsDto.ServiceId);
            if (ServiceConnectRecordsModel == null)
            {
                _ServiceConnectRecords.Insert(new ServiceConnectRecords
                {
                    DeviceId = addServiceConnectRecordsDto.DeviceId,
                    ServiceId = addServiceConnectRecordsDto.ServiceId,
                    ServiceCode = addServiceConnectRecordsDto.ServiceCode,
                    ServiceNickName = addServiceConnectRecordsDto.ServiceNickName,
                    ServiceFaceImg = addServiceConnectRecordsDto.ServiceFaceImg,
                    ServiceCount = 0,
                    ServiceState = OrderInfo.LoginState.Online,
                });
            }
            else
            {
                ServiceConnectRecordsModel.ServiceState = OrderInfo.LoginState.Online;
                _ServiceConnectRecords.Update(ServiceConnectRecordsModel);
            }
            return output;
        }
    }
}
