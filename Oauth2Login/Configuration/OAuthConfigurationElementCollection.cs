using System;
using System.Configuration;

namespace Oauth2Login.Configuration
{
    public class OAuthConfigurationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new OAuthConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var authConfigurationElement = element as OAuthConfigurationElement;
            if (authConfigurationElement != null) return authConfigurationElement.Name;
            return "";
        }
    }
}
