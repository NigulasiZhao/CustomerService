using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.OAuthUserService.OAuthCRMService.Dto
{
    public class AuthorizationLoginUrlInput
    {
        /// <summary>
        /// 第三方平台编码
        /// </summary>
        public string ThirdPlatCode { get; set; }
        /// <summary>
        /// 回调地址
        /// </summary>
        public string RedirectUri { get; set; }
    }
}
