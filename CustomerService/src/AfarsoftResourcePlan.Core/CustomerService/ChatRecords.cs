using Abp.Domain.Entities;
using AfarsoftResourcePlan.OrderInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CustomerService
{
    public class ChatRecords : Entity<int>
    {

        /// <summary>
        /// 服务记录ID
        /// </summary>
        public int ServiceRecordsId { get; set; }
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
        /// 发送时间
        /// </summary>
        public DateTime SendDateTime { get; set; }
        /// <summary>
        /// 发送消息类型
        /// </summary>
        public SendInfoType SendInfoType { get; set; }
        /// <summary>
        /// 发送内容
        /// </summary>
        public string SendContent { get; set; }
        /// <summary>
        /// 发送来源(系统、客服、客户)
        /// </summary>
        public TerminalRefer SendSource { get; set; }
        /// <summary>
        /// 接收状态(已接收、未接收)
        /// </summary>
        public ReceiveState ReceiveState { get; set; }

    }
}
