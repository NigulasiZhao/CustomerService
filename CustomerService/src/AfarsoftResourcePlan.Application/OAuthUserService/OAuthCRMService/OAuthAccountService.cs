﻿using Abp.Application.Services;
using Abp.Domain.Repositories;
using AfarsoftResourcePlan.Common;
using AfarsoftResourcePlan.CustomerService;
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

        private readonly IRepository<CustomerConnectRecords, int> _customerConnectRecords;

        private readonly IRepository<ServiceConnectRecords, int> _serviceConnectRecords;

        public OAuthAccountService(IRepository<OauthSetting, int> oauthSetting, IHttpClientFactory httpClientFactory
            , IHttpContextAccessor httpContextAccessor,
            IRepository<CustomerConnectRecords, int> customerConnectRecords,
            IRepository<ServiceConnectRecords, int> serviceConnectRecords)
        {
            _oauthSetting = oauthSetting;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _customerConnectRecords = customerConnectRecords;
            _serviceConnectRecords = serviceConnectRecords;
        }
        /// <summary>
        /// 获取授权登录地址
        /// </summary>
        /// <param name="authorizationLoginUrlInput"></param>
        /// <returns></returns>
        public BaseDataOutput<string> AuthorizationLoginUrl(AuthorizationLoginUrlInput authorizationLoginUrlInput)
        {
            if (string.IsNullOrEmpty(authorizationLoginUrlInput.ThirdPlatCode))
            {
                authorizationLoginUrlInput.ThirdPlatCode = "CRM";
            }
            BaseDataOutput<string> Output = new BaseDataOutput<string>();
            OauthSetting OauthSettingModel = _oauthSetting.FirstOrDefault(e => e.ThirdPlatCode == authorizationLoginUrlInput.ThirdPlatCode);
            Output.Data = OauthSettingModel.AuthorizationLoginUrl + "?AppId=" + OauthSettingModel.AppId + "&Type=OAuth2" + "&redirect_uri=" + authorizationLoginUrlInput.RedirectUri;
            return Output;
        }
        /// <summary>
        /// 根据AccessToken获取用户信息
        /// </summary>
        /// <param name="authorizationAccessTokenlInput"></param>
        /// <returns></returns>
        public async Task<BaseDataOutput<string>> AuthorizationAccessToken(AuthorizationAccessTokenlInput authorizationAccessTokenlInput)
        {
            BaseDataOutput<string> Output = new BaseDataOutput<string>();
            if (!string.IsNullOrEmpty(authorizationAccessTokenlInput.OAuthCode) && !string.IsNullOrEmpty(authorizationAccessTokenlInput.ThirdPlatCode))
            {
                BaseDataOutput<string> AccessTokenOutput = new BaseDataOutput<string>();
                BaseDataOutput<AuthorizationUserInfoDto> ThirdOutput = new BaseDataOutput<AuthorizationUserInfoDto>();
                OauthSetting OauthSettingModel = _oauthSetting.FirstOrDefault(e => e.ThirdPlatCode == authorizationAccessTokenlInput.ThirdPlatCode);
                if (OauthSettingModel != null)
                {
                    var GetAccessTokenObj = new
                    {
                        OAuthCode = authorizationAccessTokenlInput.OAuthCode
                    };
                    var CodeResult = await _httpClientFactory.CreateClient().PostAsync(OauthSettingModel.GetAccessTokenUrl, new StringContent(JsonConvert.SerializeObject(GetAccessTokenObj), Encoding.UTF8, "application/json"));
                    var ResultStr = await CodeResult.Content.ReadAsStringAsync();
                    AccessTokenOutput = JsonConvert.DeserializeObject<BaseDataOutput<string>>(ResultStr);
                    if (AccessTokenOutput.Code == 0)
                    {
                        #region 根据AccessToken获取用户信息
                        var GetUserInfoObj = new
                        {
                            AccessToken = AccessTokenOutput.Data
                        };
                        var UserInfoResult = await _httpClientFactory.CreateClient().PostAsync(OauthSettingModel.AuthorizationUrl, new StringContent(JsonConvert.SerializeObject(GetUserInfoObj), Encoding.UTF8, "application/json"));
                        var UserInfoResultStr = await UserInfoResult.Content.ReadAsStringAsync();
                        ThirdOutput = JsonConvert.DeserializeObject<BaseDataOutput<AuthorizationUserInfoDto>>(UserInfoResultStr);
                        if (ThirdOutput.Code == 0)
                        {
                            if (ThirdOutput.Data.UserType == OrderInfo.TerminalRefer.servicer)
                            {
                                ServiceConnectRecords ServiceConnectRecordsModel = _serviceConnectRecords.FirstOrDefault(e => e.ServiceId == ThirdOutput.Data.UserId);
                                if (ServiceConnectRecordsModel == null)
                                {
                                    int UserId = _serviceConnectRecords.InsertAndGetId(new ServiceConnectRecords
                                    {
                                        ServiceId = ThirdOutput.Data.UserId,
                                        ServiceCode = ThirdOutput.Data.UserCode,
                                        ServiceNickName = ThirdOutput.Data.UserNickName,
                                        ServiceCount = 0,
                                        ServiceFaceImg = ThirdOutput.Data.UserFaceImg,
                                        ServiceState = OrderInfo.LoginState.OffLine,
                                        DeviceId = authorizationAccessTokenlInput.DeviceId
                                    });
                                    Output.Data = ThirdOutput.Data.UserId;
                                }
                                else
                                {
                                    Output.Data = ServiceConnectRecordsModel.ServiceId.ToString();
                                }
                            }
                            if (ThirdOutput.Data.UserType == OrderInfo.TerminalRefer.user)
                            {
                                CustomerConnectRecords CustomerConnectRecordsModel = _customerConnectRecords.FirstOrDefault(e => e.CustomerId == ThirdOutput.Data.UserId);
                                if (CustomerConnectRecordsModel == null)
                                {
                                    int CustomerId = _customerConnectRecords.InsertAndGetId(new CustomerConnectRecords
                                    {
                                        CustomerId = ThirdOutput.Data.UserId,
                                        CustomerCode = ThirdOutput.Data.UserCode,
                                        CustomerNickName = ThirdOutput.Data.UserNickName,
                                        CustomerFaceImg = ThirdOutput.Data.UserFaceImg,
                                        CustomerState = OrderInfo.LoginState.OffLine,
                                        DeviceId = authorizationAccessTokenlInput.DeviceId
                                    });
                                    Output.Data = ThirdOutput.Data.UserId;
                                }
                                else
                                {
                                    Output.Data = CustomerConnectRecordsModel.CustomerId.ToString();
                                }
                            }
                        }
                        else
                        {
                            Output.Code = 1;
                            Output.Message = ThirdOutput.Message;
                        }
                        #endregion
                    }
                    else
                    {
                        Output.Code = 1;
                        Output.Message = "获取AccessToken失败:" + AccessTokenOutput.Message;
                    }
                }
                else
                {
                    Output.Code = 1;
                    Output.Message = "未获取到授权设置信息";
                }
            }
            else
            {
                Output.Code = 1;
                Output.Message = "未获取AccessToken或第三方平台编号";
            }
            return Output;
        }
    }
}
