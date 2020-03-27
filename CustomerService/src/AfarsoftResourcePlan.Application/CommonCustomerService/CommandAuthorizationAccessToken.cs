using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CommonCustomerService
{
    public class CommandAuthorizationAccessToken
    {
        public string OAuthCode { get; set; }

        public string ThirdPlatCode { get; set; }

        public string DeviceId { get; set; }
    }
}
