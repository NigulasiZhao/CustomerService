using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.OAuthUserService.OAuthCRMService.Dto
{
    public class AuthorizationAccessTokenlInput
    {
        public string AccessToken { get; set; }

        public string ThirdPlatCode { get; set; }

        public string DeviceId { get; set; }
    }
}
