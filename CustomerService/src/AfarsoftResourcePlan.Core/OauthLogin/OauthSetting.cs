using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.OauthLogin
{
    public class OauthSetting : Entity<int>
    {
        /// <summary>
        /// 第三方平台名称
        /// </summary>
        public string ThirdPlatName { get; set; }
        /// <summary>
        /// 第三方平台编码
        /// </summary>
        public string ThirdPlatCode { get; set; }
        /// <summary>
        /// 获取第三方平台授权码
        /// </summary>
        public string GetCodeUrl { get; set; }
        /// <summary>
        /// 第三方授权登录地址
        /// </summary>
        public string AuthorizationUrl { get; set; }

        public string AppId { get; set; }

        public string Secret { get; set; }
    }
}
