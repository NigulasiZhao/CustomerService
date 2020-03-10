using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using AfarsoftResourcePlan.Controllers;

namespace AfarsoftResourcePlan.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : AfarsoftResourcePlanControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
