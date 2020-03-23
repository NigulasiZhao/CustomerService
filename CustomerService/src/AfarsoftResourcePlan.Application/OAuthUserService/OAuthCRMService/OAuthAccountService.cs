using Abp.Application.Services;
using Abp.Domain.Repositories;
using AfarsoftResourcePlan.OauthLogin;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AfarsoftResourcePlan.OAuthUserService.OAuthCRMService
{
    public class OAuthAccountService : ApplicationService, IOAuthAccountService
    {
        private readonly IRepository<OauthSetting, int> _oauthSetting;
        private readonly IHttpClientFactory _httpClientFactory;

        public OAuthAccountService(IRepository<OauthSetting, int> oauthSetting, IHttpClientFactory httpClientFactory)
        {
            _oauthSetting = oauthSetting;
            _httpClientFactory = httpClientFactory;
        }
        public string GetOauthLoginUrl(string ThirdPlatCode)
        {
            OauthSetting OauthSettingModel = _oauthSetting.FirstOrDefault(e => e.ThirdPlatCode == ThirdPlatCode);
            if (OauthSettingModel != null)
            {

            }
            return "";
        }
    }
}
