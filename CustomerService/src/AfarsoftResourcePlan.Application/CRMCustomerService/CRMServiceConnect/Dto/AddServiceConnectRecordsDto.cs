using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CRMCustomerService.CRMServiceConnect.Dto
{
    public class AddServiceConnectRecordsDto
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceId {get;set;}
        /// <summary>
        /// 客服ID
        /// </summary>
        public Guid ServiceId { get; set; }
        /// <summary>
        /// 客服编码
        /// </summary>
        public string ServiceCode { get; set; }
        /// <summary>
        /// 客服昵称
        /// </summary>
        public string ServiceNickName { get; set; }
    }
}
