using Abp.Domain.Entities;
using AfarsoftResourcePlan.OrderInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CustomerService
{
    public class ServiceConnectRecords : Entity<int>
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceId { get; set; }
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
        /// <summary>
        /// 客服头像
        /// </summary>
        public string ServiceFaceImg { get; set; }
        /// <summary>
        /// 服务人数
        /// </summary>
        public int ServiceCount { get; set; }
        /// <summary>
        /// 客服状态(在线，离线)
        /// </summary>
        public LoginState ServiceState { get; set; }

    }
}
