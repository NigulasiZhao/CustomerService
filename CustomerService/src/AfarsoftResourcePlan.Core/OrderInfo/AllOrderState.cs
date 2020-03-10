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
}
