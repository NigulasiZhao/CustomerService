using Abp.AutoMapper;
using AfarsoftResourcePlan.OrderInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.SerialNumber.Dto
{
    /// <summary>
    /// 获取单据编码
    /// </summary>
    //[AutoMapTo(typeof(AfarErpRecycling.Domain.Erp.SerialNumber))]
    public class GetSerialNumberInput
    {
        /// <summary>
        /// 生成日期
        /// </summary>
        public DateTime SerialNumberDate { get; set; }
        /// <summary>
        /// 单据类型
        /// </summary>

        public AllOrderType OrderType { get; set; }
        /// <summary>
        /// 机构编码
        /// </summary>

        public Guid? OrganizationId { get; set; }
    }
}
