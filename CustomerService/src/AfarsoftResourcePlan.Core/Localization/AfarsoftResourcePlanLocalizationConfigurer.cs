using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace AfarsoftResourcePlan.Localization
{
    public static class AfarsoftResourcePlanLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(AfarsoftResourcePlanConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(AfarsoftResourcePlanLocalizationConfigurer).GetAssembly(),
                        "AfarsoftResourcePlan.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
