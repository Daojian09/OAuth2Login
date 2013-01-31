using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MultipleOauth2.Configuration
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
        [ConfigurationProperty("token", IsRequired = false)]
        public string Token { get { return base["token"].ToString(); } }
        [ConfigurationProperty("tokenSecret", IsRequired = false)]
        public string TokenSecret { get { return base["tokenSecret"].ToString(); } }
    }
}
