using Abp.Domain.Entities;
using AfarsoftResourcePlan.OrderInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CustomerService
{
    public class ServiceRecords : Entity<int>
    {
        /// <summary>
        /// 客户连接记录ID
        /// </summary>
        public int CustomerConnectRecordsId { get; set; }
        /// <summary>
        /// 客服连接记录ID
        /// </summary>
        public int? ServiceConnectRecordsId { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// 客户设备ID
        /// </summary>
        public string CustomerDeviceId { get; set; }
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
        /// 客户接入日期
        /// </summary>
        public DateTime? CustomerContentDate { get; set; }
        /// <summary>
        /// 客户退出日期
        /// </summary>
        public DateTime? CustomerUnContentDate { get; set; }
        /// <summary>
        /// 客户状态(未连接、已连接)
        /// </summary>
        public LoginState CustomerState { get; set; }
        /// <summary>
        /// 客服ID
        /// </summary>
        public string ServiceId { get; set; }
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
        /// 客服接入日期
        /// </summary>
        public DateTime? ServiceContentDate { get; set; }
        /// <summary>
        /// 客服退出日期
        /// </summary>
        public DateTime? ServiceUnContentDate { get; set; }
        /// <summary>
        /// 客服状态(未连接、已连接)
        /// </summary>
        public LoginState ServiceState { get; set; }
    }
}
