using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Navigation;
using Abp.Runtime.Session;
using AfarsoftResourcePlan.Sessions;

namespace AfarsoftResourcePlan.Web.Views.Shared.Components.SideBarNav
{
    public class SideBarNavViewComponent : AfarsoftResourcePlanViewComponent
    {
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly IAbpSession _abpSession;
        private readonly ISessionAppService _sessionAppService;

        public SideBarNavViewComponent(
            IUserNavigationManager userNavigationManager,
            IAbpSession abpSession,
            ISessionAppService sessionAppService)
        {
            _userNavigationManager = userNavigationManager;
            _abpSession = abpSession;
            _sessionAppService = sessionAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string activeMenu = "")
        {
            var model = new SideBarNavViewModel
            {
                MainMenu = _sessionAppService.GetPCNavigation().Result.Data,
                ActiveMenuItemName = activeMenu
            };

            return View(model);
        }
    }
}
