using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.Common
{
    /// <summary>
    /// 添加、删除、修改返回基类
    /// </summary>
    public class BaseOutput
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 返回码备注
        /// </summary>
        public string Message { get; set; }
        public BaseOutput()
        {
        }
        public BaseOutput(int code, string message = "")
        {
            this.Code = code;
            this.Message = message;
        }
    }
    /// <summary>
    /// 返回单独实体基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseDataOutput<T> : BaseOutput
    {
        /// <summary>
        /// 返回的业务实体列表
        /// </summary>
        public T Data { get; set; }
        public BaseDataOutput()
        {

        }
        public BaseDataOutput(T data, int code = 0, string message = "")
        {
            this.Code = code;
            this.Message = message;
            this.Data = data;
        }
    }
    /// <summary>
    /// 返回列表实体基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseListDataOutput<T> : BaseOutput
    {
        /// <summary>
        /// 返回的业务实体列表
        /// </summary>
        public List<T> Data { get; set; }
        public BaseListDataOutput()
        {

        }
        public BaseListDataOutput(List<T> data, int code = 0, string message = "")
        {
            this.Code = code;
            this.Message = message;
            this.Data = data;
        }
    }
    /// <summary>
    /// 返回分页列表基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasePageDataOutput<T> : BaseOutput//, IListResult<T>, IHasTotalCount
    {
        /// <summary>
        /// 分页的业务实体列表
        /// </summary>
        public IReadOnlyList<T> Rows { get; set; }
        /// <summary>
        /// 合计字段
        /// </summary>
        public object Footer { get; set; }
        /// <summary>
        /// 总数据条数
        /// </summary>
        public int Total { get; set; }
        public BasePageDataOutput()
        {
        }
        public BasePageDataOutput(List<T> data, int totalCount, object footer = null, int code = 0, string message = "")
        {
            this.Rows = data;
            this.Footer = footer;
            this.Code = code;
            this.Message = message;
            this.Total = totalCount;
        }
    }

}
