using Microsoft.AspNetCore.Antiforgery;
using AfarsoftResourcePlan.Controllers;

namespace AfarsoftResourcePlan.Web.Host.Controllers
{
    public class AntiForgeryController : AfarsoftResourcePlanControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
