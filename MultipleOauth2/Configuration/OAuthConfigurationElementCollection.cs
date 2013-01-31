using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MultipleOauth2.Configuration
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
