using AfarsoftResourcePlan.OrderInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.OAuthUserService.OAuthCRMService.Dto
{
    public class AuthorizationUserInfoDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserNickName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserFaceImg { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public TerminalRefer UserType { get; set; }
    }
}
