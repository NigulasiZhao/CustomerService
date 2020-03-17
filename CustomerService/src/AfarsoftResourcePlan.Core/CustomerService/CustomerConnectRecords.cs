using Abp.Domain.Entities;
using AfarsoftResourcePlan.OrderInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CustomerService
{
    public class CustomerConnectRecords : Entity<int>
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// 客户OpenId
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 客户UnionId
        /// </summary>
        public string UnionId { get; set; }
        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// 客户昵称
        /// </summary>
        public string CustomerNickName { get; set; }
        /// <summary>
        /// 客户头像
        /// </summary>
        public string CustomerFaceImg { get; set; }
        /// <summary>
        /// 客户状态(在线，离线)
        /// </summary>
        public LoginState CustomerState { get; set; }
        /// <summary>
        /// 上次接待客服Id
        /// </summary>
        public Guid? ServiceId { get; set; }
    }
}
