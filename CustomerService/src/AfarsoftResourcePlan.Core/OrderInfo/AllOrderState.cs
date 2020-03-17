using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AfarsoftResourcePlan.OrderInfo
{
    /// <summary>
    /// 采购订单状态
    /// </summary>
    public enum PurchaseOrderState
    {
        /// <summary>
        /// 审核中
        /// </summary>
        [Description("审核中")]
        Verifing = 1,
        /// <summary>
        /// 审核驳回
        /// </summary>
        [Description("审核驳回")]
        Reject = 2,
    }
    /// <summary>
    /// 采购入库单状态
    /// </summary>
    public enum PurchaseInOrderState
    {
        /// <summary>
        /// 仓库审核
        /// </summary>
        [Description("仓库审核中")]
        Verifing = 1,
        /// <summary>
        /// 审核驳回
        /// </summary>
        [Description("审核驳回")]
        Reject = 2,
        /// <summary>
        /// 财务审核
        /// </summary>
        [Description("财务审核中")]
        FinanceVerifing = 3,
        /// <summary>
        /// 交易完成
        /// </summary>
        [Description("交易完成")]
        Done = 6,
    }
    /// <summary>
    /// 在线状态
    /// </summary>
    public enum LoginState
    {
        /// <summary>
        /// 在线
        /// </summary>
        [Description("在线")]
        Online = 0,
        /// <summary>
        /// 离线
        /// </summary>
        [Description("离线")]
        OffLine = 1,

    }
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum SendInfoType
    {
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本")]
        TextInfo = 0,
        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片")]
        PictureInfo = 1,
        /// <summary>
        /// 卡片
        /// </summary>
        [Description("卡片")]
        CardInfo = 2,
    }
    /// <summary>
    /// 接收状态
    /// </summary>
    public enum ReceiveState
    {
        /// <summary>
        /// 未接收
        /// </summary>
        [Description("未接收")]
        UnReceive = 0,
        /// <summary>
        /// 已接收
        /// </summary>
        [Description("已接收")]
        Received = 1,
    }
    public enum TerminalRefer
    {
        /// <summary>
        /// 客户
        /// </summary>
        [Description("客户")]
        user,
        /// <summary>
        /// 客服
        /// </summary>
        [Description("客服")]
        servicer,
        /// <summary>
        /// 系统
        /// </summary>
        [Description("系统")]
        system
    }
}
