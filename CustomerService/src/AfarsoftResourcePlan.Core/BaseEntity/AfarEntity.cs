using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.BaseEntity
{
    public class AfarEntity<TPrimaryKey> : Entity<TPrimaryKey>, IMustHaveTenant, IDeletionAudited, IHasDeletionTime, ISoftDelete
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 企业ID
        /// </summary>
        public Guid EnterpriseId { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public Guid OrganizationId { get; set; }
        /// <summary>
        /// 机构编码
        /// </summary>
        public Guid OrganizationIdCode { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrganizationIdName { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        public Guid MarkerId { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string MarkerName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime MarkerTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string MarkerRemark { get; set; }
        /// <summary>
        /// 最后修改人Id
        /// </summary>
        public Guid LastModifierUserId { get; set; }
        /// <summary>
        /// 最后修改人姓名
        /// </summary>
        public string LastModifierUserName { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModifierTime { get; set; }
        /// <summary>
        /// 租户ID
        /// </summary>
        public int TenantId { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 删除人ID
        /// </summary>
        public long? DeleterUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletionTime { get; set; }
    }
}
