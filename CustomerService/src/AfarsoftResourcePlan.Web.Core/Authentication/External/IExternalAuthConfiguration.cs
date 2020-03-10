using System.Collections.Generic;

namespace AfarsoftResourcePlan.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
