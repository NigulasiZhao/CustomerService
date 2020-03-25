using Abp.Application.Services;
using AfarsoftResourcePlan.Common;
using AfarsoftResourcePlan.OAuthUserService.OAuthCRMService.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AfarsoftResourcePlan.OAuthUserService.OAuthCRMService
{
    public interface IOAuthAccountService : IApplicationService
    {
        /// <summary>
        /// 获取授权登录地址
        /// </summary>
        /// <param name="authorizationLoginUrlInput"></param>
        /// <returns></returns>
        Task<BaseDataOutput<string>> AuthorizationLoginUrl(AuthorizationLoginUrlInput authorizationLoginUrlInput);
        /// <summary>
        /// 根据AccessToken获取用户信息
        /// </summary>
        /// <param name="authorizationAccessTokenlInput"></param>
        /// <returns></returns>
        Task<BaseDataOutput<string>> AuthorizationAccessToken(AuthorizationAccessTokenlInput authorizationAccessTokenlInput);
    }
}
