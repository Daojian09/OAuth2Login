using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Newtonsoft.Json;
using Oauth2Login.Client;
using Oauth2Login.Core;

namespace Oauth2Login.Service
{
    public class PayPalService : IClientService
    {
        private static string _oauthUrl = "";
        private AbstractClientProvider _client;

#if DEBUG
        private const string OAUTH_API_URL = "https://api.sandbox.paypal.com";
#else
        private const string OAUTH_API_URL = "https://api.paypal.com"; 
#endif

#if DEBUG
        private const string OAUTH_API_LOGIN_URL = "https://www.sandbox.paypal.com";
#else
        private const string OAUTH_API_LOGIN_URL = "https://www.paypal.com"; 
#endif

        public PayPalService()
        {
        }

        public PayPalService(AbstractClientProvider oCleint)
        {
            _client = oCleint;
        }

        public void CreateOAuthClient(IOAuthContext oContext)
        {
            _client = oContext.Client;
        }

        public void CreateOAuthClient(AbstractClientProvider oClient)
        {
            _client = oClient;
        }

        public string BeginAuthentication()
        {
            if (_client != null)
            {
                _oauthUrl = string.Format(OAUTH_API_LOGIN_URL + "/webapps/auth/protocol/openidconnect/v1/authorize?" +
                                "client_id={0}&response_type=code&redirect_uri={1}&scope={2}",
                                _client.ClientId,
                                HttpUtility.HtmlEncode(_client.CallBackUrl),
                                _client.Scope,
                                "");
                return _oauthUrl;
            }
            throw new Exception("ERROR: BeginAuth the cleint not found!");
        }

        public string RequestToken()
        {
            string code = HttpContext.Current.Request.Params["code"];
            if (code != null)
            {
                string oauthUrl = OAUTH_API_URL + "/v1/identity/openidconnect/tokenservice";
                //string oAuthCredentials = Convert.ToBase64String(Encoding.Default.GetBytes(_client.ClientId + ":" + _client.ClientSecret));
                string post = string.Format("grant_type=authorization_code&redirect_uri={0}&code={1}&client_id={2}&client_secret={3}",
                                    HttpUtility.HtmlEncode(_client.CallBackUrl),
                                    code,
                                    _client.ClientId,
                                    _client.ClientSecret);
                string resonseJson = RestfullRequest.Request(oauthUrl, 
                                                                                        "POST", 
                                                                                        "application/x-www-form-urlencoded", 
                                                                                        null, 
                                                                                        post,
                                                                                        _client.Proxy);
              return JsonConvert.DeserializeAnonymousType(resonseJson, new { access_token = "" }).access_token;
            }
            return "access_denied";
        }

        public Dictionary<string, string> RequestUserProfile()
        {           
            string profileUrl = string.Format(OAUTH_API_URL + "/v1/identity/openidconnect/userinfo/?schema=openid");
            NameValueCollection header = new NameValueCollection();
            header.Add("Accept-Language", "en_US");
            header.Add("Authorization", "Bearer " + _client.Token);
            string result = RestfullRequest.Request(profileUrl, "POST", "application/json", header, null, _client.Proxy);
            _client.ProfileJsonString = result;
            PayPalClient.UserProfile data = JsonConvert.DeserializeAnonymousType(result, new PayPalClient.UserProfile());

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("source", "PayPal");
            dictionary.Add("email", data.Email);
            dictionary.Add("verified_email", data.Verified_email);
            dictionary.Add("name", data.Name);
            dictionary.Add("given_name", data.Given_name);
            dictionary.Add("family_name", data.Family_name);
            dictionary.Add("link", data.User_id);
            dictionary.Add("picture", data.Picture);
            dictionary.Add("gender", data.Gender);
            return dictionary;
        }
    }
}
