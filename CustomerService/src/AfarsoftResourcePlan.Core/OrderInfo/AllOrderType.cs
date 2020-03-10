using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AfarsoftResourcePlan.OrderInfo
{
    /// <summary>
    /// 全部单据类型枚举
    /// </summary>
    public enum AllOrderType
    {
        [Description("采购订单")]
        CGDD = 1,
        [Description("采购入库单")]
        CGRKD = 2,
    }
}
