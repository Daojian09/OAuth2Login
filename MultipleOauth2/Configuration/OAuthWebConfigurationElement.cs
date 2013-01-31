using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MultipleOauth2.Configuration
{
    public class OAuthWebConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("acceptedRedirectUrl", IsRequired = true)]
        public string AcceptedRedirectUrl { get { return base["acceptedRedirectUrl"].ToString(); } }
        [ConfigurationProperty("failedRedirectUrl", IsRequired = true)]
        public string FailedRedirectUrl { get { return base["failedRedirectUrl"].ToString(); } }
    }
}
