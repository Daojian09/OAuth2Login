using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MultipleOauth2
{
    public static class MultiOAuthFactroy
    {
        /// <summary>
        /// 使用Config組態檔產生連結實體
        /// </summary>
        /// <typeparam name="T">將建立的Client物件, GoogleClient/FacebookClient/WindowsLiveClient</typeparam>
        /// <param name="oConfigurationName">Config組態檔oauth標籤內的name名稱</param>
        /// <returns></returns>
        public static T CreateClient<T>(string oConfigurationName) where T : IClientProvider, new()
        {
            Configuration.OAuthConfigurationSection clientConfiguration =
                      ConfigurationManager.GetSection("oauth.configuration") as Configuration.OAuthConfigurationSection;

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
                                    acceptedUrl, failedUrl ,
                                    clientConfigurationElement.Proxy,
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

        /// <summary>
        /// 使用傳入參數產生連結實體
        /// </summary>
        /// <typeparam name="T">將建立的Client物件, GoogleClient/FacebookClient/WindowsLiveClient</typeparam>
        /// <param name="oClientId">應用程式ID</param>
        /// <param name="oClientSecret">應用程式密鑰</param>
        /// <param name="oCallbackUrl">驗證的網址</param>
        /// <param name="oScope">存取權限</param>
        /// <param name="oAcceptedUrl">驗證的網址</param>
        /// <param name="oFailedUrl">驗證失敗的網址</param>
        /// <returns></returns>
        public static T CreateClient<T>(string oClientId, string oClientSecret, string oCallbackUrl, string oScope,
                                        string oAcceptedUrl, string oFailedUrl) where T : IClientProvider, new()
        {
            T client = (T)Activator.CreateInstance(typeof(T), new object[] { 
                                                        oClientId,
                                                        oClientSecret,
                                                        oCallbackUrl,
                                                        oScope,
                                                        oAcceptedUrl,
                                                        oFailedUrl });
            return client;
        }
    }
}
