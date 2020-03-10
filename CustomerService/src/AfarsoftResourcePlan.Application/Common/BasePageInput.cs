using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AfarsoftResourcePlan.Common
{
    /// <summary>
    /// 分页请求的基类，接收分页请求
    /// </summary>
    public class BasePageInput : PagedResultRequestDto, IShouldNormalize
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public int page { get; set; }
        /// <summary>
        /// 读取行数
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public int rows { get; set; }
        /// <summary>
        /// 忽略此参数
        /// </summary>
        [Editable(false)]
        public override int MaxResultCount { get { return rows; } }
        /// <summary>
        /// 忽略此参数
        /// </summary>
        [Editable(false)]
        public override int SkipCount { get => (page - 1) * rows; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string sort { get; set; }
        /// <summary>
        /// 排序方式(asc:升序;desc:降序 )
        /// </summary>
        public string order { get; set; }
        /// <summary>
        /// 默认排序
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(sort))
            {
                sort = "Id";
            }
            if (StartDate != null)
            {
                StartDate = new DateTime(StartDate.Value.Year, StartDate.Value.Month, StartDate.Value.Day, 0, 0, 0);
            }
            if (EndDate != null)
            {
                EndDate = new DateTime(EndDate.Value.Year, EndDate.Value.Month, EndDate.Value.Day, 23, 59, 59);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                order = " " + order;
            }
        }
    }
}
