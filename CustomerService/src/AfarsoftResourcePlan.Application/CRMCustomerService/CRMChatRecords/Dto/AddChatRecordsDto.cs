using AfarsoftResourcePlan.OrderInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CRMCustomerService.CRMChatRecords.Dto
{
    public class AddChatRecordsDto
    {
        public string CustomerDeviceId { get; set; }

        public string ServicerId { get; set; }
        public int ServiceRecordId { get; set; }
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
    }
}
