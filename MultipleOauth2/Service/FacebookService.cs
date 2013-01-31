using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using MultipleOauth2.Client;
using Newtonsoft.Json;

namespace MultipleOauth2.Service
{
    public class FacebookService : OAuthServiceBase, IClientService
    {
        private static string _oauthUrl = "";
        IClientProvider _client;

        public FacebookService()
        {
        }

        public FacebookService(IClientProvider oCleint)
        {
            _client = oCleint;
        }

        internal override void CreateOAuthClient(IClientProvider oClient)
        {
            _client = oClient;
        }

        internal override void CreateOAuthClient(MultiOAuthContext oContext)
        {
            _client = oContext.client;
        }

        public string BeginAuthentication()
        {
            if (_client != null)
            {
                _oauthUrl = string.Format("https://www.facebook.com/dialog/oauth?" +
                                "client_id={0}&redirect_uri={1}&scope={2}&state={3}&display=popup",
                                _client.ClientId,
                                HttpUtility.HtmlEncode(_client.CallBackUrl),
                                _client.Scope,
                                "");
                //HttpContext.Current.Response.Redirect(_oauthUrl);
                return _oauthUrl;
            }
            else
            {
                throw new Exception("ERROR: BeginAuth the cleint not found!");
            }
        }

        public string RequestToken()
        {
            string code = HttpContext.Current.Request.Params["code"];
            if (code != null)
            {
                string tokenUrl = string.Format("https://graph.facebook.com/oauth/access_token?");
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(tokenUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                var post = string.Format("client_id={0}&redirect_uri={1}&client_secret={2}&code={3}",
                                          _client.ClientId,
                                          HttpUtility.HtmlEncode(_client.CallBackUrl),
                                          _client.ClientSecret,
                                          code);
                if (!string.IsNullOrEmpty(_client.Proxy))
                {
                    IWebProxy proxy = new WebProxy(_client.Proxy);
                    proxy.Credentials = new NetworkCredential();
                    request.Proxy = proxy;
                }

                using (var sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(post);
                }
              
                var resonseJson = "";
                using (var response = request.GetResponse())
                {
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        resonseJson = sr.ReadToEnd();
                    }
                }

                resonseJson = "{\"" + resonseJson.Replace("=", "\":\"").Replace("&", "\",\"") + "\"}";

                return JsonConvert.DeserializeAnonymousType(resonseJson, new { access_token = "" }).access_token;
            }
            return "access_denied";
        }

        public Dictionary<string, string> RequestUserProfile()
        {
            string result = "";
            string profileUrl = string.Format("https://graph.facebook.com/me?access_token={0}", _client.Token);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(profileUrl);
            request.Headers.Add("Accept-Language", "zh-cn");

            if (!string.IsNullOrEmpty(_client.Proxy))
            {
                IWebProxy proxy = new WebProxy(_client.Proxy);
                proxy.Credentials = new NetworkCredential();
                request.Proxy = proxy;
            }

            using (var response = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
            }

            var data = JsonConvert.DeserializeAnonymousType(result, new FacebookClient.UserProfile());

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("source", "Facebook");
            dictionary.Add("id", data.Id);
            dictionary.Add("name", data.Name);
            dictionary.Add("first_name", data.First_name);
            dictionary.Add("last_name", data.Last_name);
            dictionary.Add("link", data.Link);
            dictionary.Add("gender", data.Gender);
            dictionary.Add("email", data.Email);
            dictionary.Add("picture", data.Picture);

            return dictionary;
        }
    }
}
