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
    public class WindowsLiveService : OAuthServiceBase, IClientService
    {
        private static string _oauthUrl = "";
        IClientProvider _client;

        public WindowsLiveService()
        {
        }

        public WindowsLiveService(IClientProvider oCleint)
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
                _oauthUrl = string.Format("https://login.live.com/oauth20_authorize.srf?" +
                        "client_id={0}&scope={1}&display=popup&target=self&response_type=code&redirect_uri={2}",
                        HttpUtility.HtmlEncode(_client.ClientId),
                        HttpUtility.HtmlEncode(_client.Scope),
                        HttpUtility.HtmlEncode(_client.CallBackUrl)
                        );
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
                string tokenUrl = string.Format("https://login.live.com/oauth20_token.srf");
                var post = string.Format("client_id={0}&redirect_uri={1}&target=self&client_secret={2}&code={3}&grant_type=authorization_code",
                                            HttpUtility.HtmlEncode(_client.ClientId),
                                            HttpUtility.HtmlEncode(_client.CallBackUrl),
                                            _client.ClientSecret,
                                            code);
               HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(tokenUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

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

                return JsonConvert.DeserializeAnonymousType(resonseJson, new { access_token = "" }).access_token;
            }
            return "access_denied";
        }

        public Dictionary<string, string> RequestUserProfile()
        {
            string result = "";
            string profileUrl = string.Format("https://apis.live.net/v5.0/me?access_token={0}", _client.Token);
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

            var data = JsonConvert.DeserializeAnonymousType(result, new WindowsLiveClient.UserProfile());

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("source", "WindowsLive");
            dictionary.Add("id", data.Id);
            dictionary.Add("name", data.Name);
            dictionary.Add("first_name", data.First_name);
            dictionary.Add("last_name", data.Last_name);
            dictionary.Add("link", data.Link);
            dictionary.Add("gender", data.Gender);
            dictionary.Add("email", data.Emails.Preferred);

            return dictionary;
        }
    }
}
