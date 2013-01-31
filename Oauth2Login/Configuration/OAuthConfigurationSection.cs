using System;
using System.Configuration;

namespace Oauth2Login.Configuration
{
    public class OAuthConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("web", IsRequired = false)]
        public OAuthWebConfigurationElement WebConfiguration { get { return base["web"] as OAuthWebConfigurationElement; } }

        [ConfigurationProperty("oauth", IsKey = false, IsRequired = true)]
        [ConfigurationCollection(typeof(OAuthConfigurationElementCollection), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
        public OAuthConfigurationElementCollection OAuthVClientConfigurations { get { return base["oauth"] as OAuthConfigurationElementCollection; } }

    }
}
