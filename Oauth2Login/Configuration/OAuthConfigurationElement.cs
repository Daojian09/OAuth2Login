using System;
using System.Configuration;

namespace Oauth2Login.Configuration
{
    public class OAuthConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name { get { return base["name"].ToString(); } }
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type { get { return base["type"].ToString(); } }
        [ConfigurationProperty("clientid", IsRequired = true)]
        public string ClientId { get { return base["clientid"].ToString(); } }
        [ConfigurationProperty("clientsecret", IsRequired = true)]
        public string ClientSecret { get { return base["clientsecret"].ToString(); } }
        [ConfigurationProperty("callbackUrl", IsRequired = false, DefaultValue = "oob")]
        public string CallbackUrl { get { return base["callbackUrl"].ToString(); } }
        [ConfigurationProperty("scope", IsRequired = true)]
        public string Scope { get { return base["scope"].ToString(); } }
        [ConfigurationProperty("proxy", IsRequired = false)]
        public string Proxy { get { return base["proxy"].ToString(); } }
    }
}
