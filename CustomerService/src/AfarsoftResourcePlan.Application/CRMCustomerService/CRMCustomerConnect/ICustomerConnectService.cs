using Abp.Application.Services;
using AfarsoftResourcePlan.Common;
using AfarsoftResourcePlan.CRMCustomerService.CRMCustomerConnect.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CRMCustomerService.CRMCustomerConnect
{
    public interface ICustomerConnectService :  IApplicationService
    {
        /// <summary>
        /// 添加客户,并建立连接
        /// </summary>
        /// <param name="addCustomerConnectRecordsDto"></param>
        BaseDataOutput<int> AddServiceConnectRecords(AddCustomerConnectRecordsDto addCustomerConnectRecordsDto);
    }
}
