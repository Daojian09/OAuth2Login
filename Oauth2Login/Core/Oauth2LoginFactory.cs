using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Oauth2Login
{
    public class Oauth2LoginFactory
    {

        public static T CreateClient<T>(string oConfigurationName) where T : AbstractClientProvider, new()
        {
            Configuration.OAuthConfigurationSection clientConfiguration =
                      ConfigurationManager.GetSection("oauth2.login.configuration") as Configuration.OAuthConfigurationSection;

            if (clientConfiguration != null)
            {
                string acceptedUrl = clientConfiguration.WebConfiguration.AcceptedRedirectUrl;
                string failedUrl = clientConfiguration.WebConfiguration.FailedRedirectUrl;


                IEnumerator configurationReader = clientConfiguration.OAuthVClientConfigurations.GetEnumerator();

                while (configurationReader.MoveNext())
                {
                    if (configurationReader.Current is Configuration.OAuthConfigurationElement)
                    {
                        Configuration.OAuthConfigurationElement clientConfigurationElement =
                            configurationReader.Current as Configuration.OAuthConfigurationElement;

                        if (oConfigurationName != null)
                        {
                            if (clientConfigurationElement.Name == oConfigurationName)
                            {
                                T client = (T)Activator.CreateInstance(typeof(T), new object[] {
                                    clientConfigurationElement.ClientId,
                                    clientConfigurationElement.ClientSecret,
                                    clientConfigurationElement.CallbackUrl,
                                    clientConfigurationElement.Scope,
                                    acceptedUrl,
                                    failedUrl ,
                                    clientConfigurationElement.Proxy
                                });

                                return client;
                            }
                        }
                        else
                        {
                            throw new Exception("ERROR: [MultiOAuthFactroy] ConfigurationName is not found!");
                        }
                    }
                }
            }

            return default(T);
        }

        public static T CreateClient<T>(string oClientId, string oClientSecret, string oCallbackUrl, string oScope,
                                        string oAcceptedUrl, string oFailedUrl, string oProxy) where T : AbstractClientProvider, new()
        {
            T client = (T)Activator.CreateInstance(typeof(T), new object[] { 
                                                        oClientId,
                                                        oClientSecret,
                                                        oCallbackUrl,
                                                        oScope,
                                                        oAcceptedUrl,
                                                        oFailedUrl,
                                                        oProxy});
            return client;
        }
    }
}
