using Abp.Application.Services;
using Abp.Domain.Repositories;
using AfarsoftResourcePlan.Common;
using AfarsoftResourcePlan.OauthLogin;
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

        public OAuthAccountService(IRepository<OauthSetting, int> oauthSetting, IHttpClientFactory httpClientFactory)
        {
            _oauthSetting = oauthSetting;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<BaseDataOutput<string>> GetOauthLoginUrl(string ThirdPlatCode)
        {
            BaseDataOutput<string> Output = new BaseDataOutput<string>();
            BaseDataOutput<string> ThirdOutput = new BaseDataOutput<string>();
            OauthSetting OauthSettingModel = _oauthSetting.FirstOrDefault(e => e.ThirdPlatCode == ThirdPlatCode);
            if (OauthSettingModel != null)
            {
                var GetCodeObj = new
                {
                    AppId = OauthSettingModel.AppId,
                    Secret = OauthSettingModel.Secret
                };
                var CodeResult = await _httpClientFactory.CreateClient().PostAsync(OauthSettingModel.GetCodeUrl, new StringContent(JsonConvert.SerializeObject(GetCodeObj), Encoding.UTF8, "application/json"));
                var ResultStr = await CodeResult.Content.ReadAsStringAsync();
                ThirdOutput = JsonConvert.DeserializeObject<BaseDataOutput<string>>(ResultStr);
            }
            Output.Data = OauthSettingModel.AuthorizationUrl + "?Code=" + ThirdOutput.Data;
            return Output;
        }
    }
}
