using Abp.Application.Navigation;
using System.Collections.Generic;

namespace AfarsoftResourcePlan.Web.Views.Shared.Components.SideBarNav
{
    public class SideBarNavViewModel
    {
        public IList<UserMenuItem> MainMenu { get; set; }

        public string ActiveMenuItemName { get; set; }
    }
}
