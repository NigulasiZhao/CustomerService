using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.BaseEntity
{
    public class AfarOrderEntity<TPrimaryKey> : AfarEntity<TPrimaryKey>
    {
        /// <summary>
        /// 审核人Id
        /// </summary>
        public Guid VerifierId { get; set; }
        /// <summary>
        /// 审核人姓名
        /// </summary>
        public string VerifierName { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime VerifierTime { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        public string VerifierRemark { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
