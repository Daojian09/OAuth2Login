using System;
using System.Configuration;

namespace Oauth2Login.Configuration
{
    public class OAuthWebConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("acceptedRedirectUrl", IsRequired = true)]
        public string AcceptedRedirectUrl { get { return base["acceptedRedirectUrl"].ToString(); } }
        [ConfigurationProperty("failedRedirectUrl", IsRequired = true)]
        public string FailedRedirectUrl { get { return base["failedRedirectUrl"].ToString(); } }
    }
}
