﻿using Abp.Application.Services;
using AfarsoftResourcePlan.CRMCustomerService.CRMServiceConnect.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CRMCustomerService.CRMServiceConnect
{
    public interface IServiceConnectService : IApplicationService
    {
        /// <summary>
        /// 客服表添加记录
        /// </summary>
        /// <param name="addServiceConnectRecordsDto"></param>
        void AddServiceConnectRecords(AddServiceConnectRecordsDto addServiceConnectRecordsDto);
    }
}
