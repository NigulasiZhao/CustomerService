using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.BaseEntity
{
    public class AfarOrderDetailEntity<TPrimaryKey>: AfarEntity<TPrimaryKey>
    {
        /// <summary>
        /// 排序号
        /// </summary>
        public int Sort { get; set; }
    }
}
