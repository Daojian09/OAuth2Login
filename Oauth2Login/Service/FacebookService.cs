using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Oauth2Login.Client;

namespace Oauth2Login.Service
{
    public class FacebookService : IClientService
    {
        private static string _oauthUrl = "";
        private AbstractClientProvider _client;

        public FacebookService()
        {
        }

        public FacebookService(AbstractClientProvider oCleint)
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
                _oauthUrl = string.Format("https://www.facebook.com/dialog/oauth?" +
                                "client_id={0}&redirect_uri={1}&scope={2}&state={3}&display=popup",
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
                string tokenUrl = string.Format("https://graph.facebook.com/oauth/access_token?");
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(tokenUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string post = string.Format("client_id={0}&redirect_uri={1}&client_secret={2}&code={3}",
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

                using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(post);
                }

                var resonseJson = "";
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
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

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
            }
            _client.ProfileJsonString = result;
            FacebookClient.UserProfile data = JsonConvert.DeserializeAnonymousType(result, new FacebookClient.UserProfile());

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
