using Abp.Application.Navigation;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace AfarsoftResourcePlan.Startup
{
    public class AfarsoftResourcePlanNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            string xmlName = _assembly.GetManifestResourceNames().Where(e => e.EndsWith(".Navigation.xml")).FirstOrDefault();
            if (string.IsNullOrEmpty(xmlName))
                return;
            Stream stream = _assembly.GetManifestResourceStream(xmlName);
            XmlDocument xmlDoc = null;
            xmlDoc = new XmlDocument();
            xmlDoc.Load(stream);
            foreach (XmlNode node1 in xmlDoc.SelectNodes("/items/menu"))
            {
                var level1Name = node1.Attributes["name"]?.Value;
                var level1DisplayName = node1.Attributes["displayName"]?.Value;
                var level1Icon = node1.Attributes["icon"]?.Value;
                var level1Url = node1.Attributes["url"]?.Value;
                var level1RequiredPermissionName = node1.Attributes["requiredPermissionName"]?.Value;
                var level1Target = node1.Attributes["target"]?.Value;
                var menu1 = new MenuItemDefinition(level1Name, L(level1DisplayName), level1Icon, level1Url, true, level1RequiredPermissionName, target: level1Target);
                foreach (XmlNode node2 in node1.SelectNodes("descendant::subTitle/menu"))
                {
                    var level2Name = node2.Attributes["name"]?.Value;
                    var level2DisplayName = node2.Attributes["displayName"]?.Value;
                    var level2Icon = node2.Attributes["icon"]?.Value;
                    var level2Url = node2.Attributes["url"]?.Value;
                    var level2RequiredPermissionName = node2.Attributes["requiredPermissionName"]?.Value;
                    var menu2 = new MenuItemDefinition(level2Name, L(level2DisplayName), level2Icon, level2Url, true, level2RequiredPermissionName);
                    foreach (XmlNode node3 in node2.SelectNodes("descendant::subItems/menu"))
                    {
                        var level3Name = node3.Attributes["name"]?.Value;
                        Debug.WriteLine(level3Name);
                        var level3DisplayName = node3.Attributes["displayName"]?.Value;
                        var level3Icon = node3.Attributes["icon"]?.Value;
                        var level3Url = node3.Attributes["url"]?.Value;
                        var level3RequiredPermissionName = node3.Attributes["requiredPermissionName"]?.Value;
                        var menu3 = new MenuItemDefinition(level3Name, L(level3DisplayName), level3Icon, level3Url, true, level3RequiredPermissionName);
                        List<MenuItemDefinition> menuItemDefinitions = new List<MenuItemDefinition>();
                        foreach (XmlNode node4 in node3.SelectNodes("descendant::rightItems/menu"))
                        {
                            var level4Name = node4.Attributes["name"]?.Value;
                            var level4DisplayName = node4.Attributes["displayName"]?.Value;
                            var level4Icon = node4.Attributes["icon"]?.Value;
                            var level4Url = node4.Attributes["url"]?.Value;
                            var level4RequiredPermissionName = node4.Attributes["requiredPermissionName"]?.Value;
                            menuItemDefinitions.Add(new MenuItemDefinition(level4Name, L(level4DisplayName), level4Icon, level4Url, true, level4RequiredPermissionName, 0, level4DisplayName));
                        }
                        menu3.CustomData = menuItemDefinitions;
                        menu2.AddItem(menu3);
                    }
                    menu1.AddItem(menu2);
                }
                context.Manager.MainMenu.AddItem(menu1);
            }
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AfarsoftResourcePlanConsts.LocalizationSourceName);
        }
    }
}
