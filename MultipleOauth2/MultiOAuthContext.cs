using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace MultipleOauth2
{
    public class MultiOAuthContext
    {
        private const string SessionKey = "MultiOAuthContext";
        private const string CookieKey = "MultiOAuthCookie";
        private IClientProvider _client ;
        private IClientService _service;

        public IClientProvider client
        {
            get { return _client; }
        }

        public IClientService service
        {
            get { return _service; }
        }

        /// <summary>
        /// 取得通行令牌
        /// </summary>
        public string Token
        {
            get
            {
                if (_client.Token == "")
                    RequestToken();
                return _client.Token;
            }
            set { _client.Token = value; }
        }

        /// <summary>
        /// 取得使用者資訊(基本: id,name,email)
        /// </summary>
        public Dictionary<string, string> Profile
        {
            get
            {
                if (client.Profile == null)
                    RequestProfile();
                return _client.Profile;
            }
        }

        public static MultiOAuthContext Current
        {
            get
            {
                MultiOAuthContext context = null;
                context = HttpContext.Current.Session[SessionKey] as MultiOAuthContext;

                if (context == null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieKey];
                    if (cookie != null)
                    {
                        Configuration.OAuthConfigurationSection clientConfiguration =
                        ConfigurationManager.GetSection("oauth.configuration") as Configuration.OAuthConfigurationSection;
                        IEnumerator configurationReader = clientConfiguration.OAuthVClientConfigurations.GetEnumerator();

                        while (configurationReader.MoveNext())
                        {
                            if (configurationReader.Current is Configuration.OAuthConfigurationElement)
                            {
                                Configuration.OAuthConfigurationElement clientConfigurationElement =
                                    configurationReader.Current as Configuration.OAuthConfigurationElement;

                                if (cookie["configuration"] != null)
                                {
                                    if (clientConfigurationElement.Name == cookie["configuration"])
                                    {
                                        Type cType = Type.GetType(clientConfigurationElement.Name + "Client");
                                        IClientProvider client = (IClientProvider)Activator.CreateInstance(cType, new object[] { 
                                                        clientConfigurationElement.ClientId,
                                                        clientConfigurationElement.ClientSecret,
                                                        clientConfigurationElement.CallbackUrl,
                                                        clientConfigurationElement.Scope });

                                        //context = MultiOAuthContext.Create(MultiOAuthFactroy.CreateClient<IClientProvider>(clientConfigurationElement.Name));


                                        //
                                        //IClientProvider clientProvider = (IClientProvider)Activator.CreateInstance(type);
                                        //context = new MultiOAuthContext(clientProvider);

                                        //T 

                                        //this._client = oClient;
                                        //this._service = (IClientService)Activator.CreateInstance(_client.serviceType, new object[] { this._client });
                                    }
                                }
                                else
                                {
                                    throw new Exception("ERROR: Cookie[configuration] is not found!");
                                }
                            }
                        }
                    }
                }
                return context;
            }
        }

        public MultiOAuthContext()
        {
        }

        public MultiOAuthContext(IClientProvider oClient)
        {
            if (oClient != null)
            {
                // 設定物件
                _client = oClient;
                _service = (IClientService)Activator.CreateInstance(_client.ServiceType, new object[] { _client });
                // 設定Session
                HttpContext.Current.Session.Add(SessionKey, this);
                // 設定Cooike
                var oauthCookie = new HttpCookie(CookieKey);
                oauthCookie["configuration"] = _client.GetType().Name.Replace("Client", "");
                oauthCookie.Expires = DateTime.Now.AddHours(1);
            }
            else
                throw new Exception("ERROR: [MultiOAuthContext] Client is not found!");
        }

        /// <summary>
        /// 產生MultiOAuthContext實體
        /// </summary>
        /// <param name="oClient">MultiOAuthFactory</param>
        /// <returns></returns>
        public static MultiOAuthContext Create(IClientProvider oClient)
        {
            return new MultiOAuthContext(oClient);
        }

        /// <summary>
        /// 開始驗證
        /// </summary>
        public string BeginAuth()
        {
            return _service.BeginAuthentication();
        }

        /// <summary>
        /// 取得通行令牌
        /// </summary>
        private void RequestToken()
        {
            string result = _service.RequestToken();
            if (result != "access_denied")
                _client.Token = result;
            else
                HttpContext.Current.Response.Redirect(_client.FailedRedirectUrl);

        }

        /// <summary>
        /// 取得使用者資訊
        /// </summary>
        private void RequestProfile()
        {
            Dictionary<string, string> result = _service.RequestUserProfile();
            if (result != null)
                _client.Profile = result;
            else
                throw new Exception("ERROR: [MultiOAuthContext] Profile is not found!");
        }
    }
}
