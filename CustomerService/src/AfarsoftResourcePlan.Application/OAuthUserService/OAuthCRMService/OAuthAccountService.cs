using Abp.Application.Services;
using Abp.Domain.Repositories;
using AfarsoftResourcePlan.Common;
using AfarsoftResourcePlan.OauthLogin;
using AfarsoftResourcePlan.OAuthUserService.OAuthCRMService.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AfarsoftResourcePlan.OAuthUserService.OAuthCRMService
{
    public class OAuthAccountService : ApplicationService, IOAuthAccountService
    {
        private readonly IRepository<OauthSetting, int> _oauthSetting;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OAuthAccountService(IRepository<OauthSetting, int> oauthSetting, IHttpClientFactory httpClientFactory
            , IHttpContextAccessor httpContextAccessor)
        {
            _oauthSetting = oauthSetting;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 获取授权登录地址
        /// </summary>
        /// <param name="authorizationLoginUrlInput"></param>
        /// <returns></returns>
        public async Task<BaseDataOutput<string>> AuthorizationLoginUrl(AuthorizationLoginUrlInput authorizationLoginUrlInput)
        {
            if (string.IsNullOrEmpty(authorizationLoginUrlInput.ThirdPlatCode))
            {
                authorizationLoginUrlInput.ThirdPlatCode = "CRM";
            }
            BaseDataOutput<string> Output = new BaseDataOutput<string>();
            BaseDataOutput<string> ThirdOutput = new BaseDataOutput<string>();
            var SourceRequest = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Host].ToString();
            OauthSetting OauthSettingModel = _oauthSetting.FirstOrDefault(e => e.ThirdPlatCode == authorizationLoginUrlInput.ThirdPlatCode);
            if (OauthSettingModel != null)
            {
                var GetCodeObj = new
                {
                    AppId = OauthSettingModel.AppId,
                    Secret = OauthSettingModel.Secret,
                    Domain = SourceRequest
                };
                var CodeResult = await _httpClientFactory.CreateClient().PostAsync(OauthSettingModel.GetCodeUrl, new StringContent(JsonConvert.SerializeObject(GetCodeObj), Encoding.UTF8, "application/json"));
                var ResultStr = await CodeResult.Content.ReadAsStringAsync();
                ThirdOutput = JsonConvert.DeserializeObject<BaseDataOutput<string>>(ResultStr);
            }
            Output.Data = OauthSettingModel.AuthorizationUrl + "?AuthorizationCode=" + ThirdOutput.Data;
            return Output;
        }
    }
}
