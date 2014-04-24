using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Oauth2Login.Client;

namespace Oauth2Login.Service
{
    public class PayPalService : IClientService
    {
        private static string _oauthUrl = "";
        private AbstractClientProvider _client;

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
                _oauthUrl = string.Format("https://api.paypal.com/webapps/auth/protocol/openidconnect/v1/authorize?" +
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
                string oauthUrl = "https://api.paypal.com/v1/identity/openidconnect/tokenservice";
                string oAuthCredentials = Convert.ToBase64String(Encoding.Default.GetBytes(_client.ClientId + ":" + _client.ClientSecret));
                var request = (HttpWebRequest)HttpWebRequest.Create(oauthUrl);
                request.Method = "POST";
                //request.Headers["Authorization"] = "Basic " + oAuthCredentials;
                request.ContentType = "application/x-www-form-urlencoded";
                string post = string.Format("grant_type=authorization_code&redirect_uri={0}&code={1}&client_id={2}&client_secret={3}",
                                    HttpUtility.HtmlEncode(_client.CallBackUrl),
                                    code,
                                    _client.ClientId,
                                    _client.ClientSecret);

                if (!string.IsNullOrEmpty(_client.Proxy))
                {
                    IWebProxy proxy = new WebProxy(_client.Proxy);
                    proxy.Credentials = new NetworkCredential();
                    request.Proxy = proxy;
                }
                using (StreamWriter swt = new StreamWriter(request.GetRequestStream()))
                {
                    swt.Write(post);
                }

                var resonseJson = "";
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        resonseJson = sr.ReadToEnd();
                    }
                }
                return JsonConvert.DeserializeAnonymousType(resonseJson, new { access_token = "" }).access_token;
            }
            return "access_denied";
        }

        public Dictionary<string, string> RequestUserProfile()
        {
            string result = "";
            string profileUrl = string.Format("https://api.paypal.com/v1/identity/openidconnect/userinfo/?schema=openid");
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(profileUrl);
            request.Headers.Add("Accept-Language", "en_US");
            request.Method = "POST";
            request.Headers["Authorization"] = "Bearer " + _client.Token;
            request.Accept = "application/json";
            //request.ContentType = "application/json";

            if (!string.IsNullOrEmpty(_client.Proxy))
            {
                IWebProxy proxy = new WebProxy(_client.Proxy);
                proxy.Credentials = new NetworkCredential();
                request.Proxy = proxy;
            }

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
            }
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
