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
    public class GoogleService : OAuthServiceBase, IClientService
    {
        private static string _oauthUrl = "";
        IClientProvider _client ;

        public GoogleService()
        {
        }

        public GoogleService(IClientProvider oCleint)
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
                _oauthUrl = string.Format("https://accounts.google.com/o/oauth2/auth?" +
                        "scope={0}&state={1}&redirect_uri={2}&client_id={3}&response_type=code&approval_prompt=force&display=popup",
                        HttpUtility.HtmlEncode(_client.Scope),
                        "profile",
                        HttpUtility.UrlEncode(_client.CallBackUrl),
                       HttpUtility.UrlEncode(_client.ClientId));
               // HttpContext.Current.Response.Redirect(_oauthUrl);
                return _oauthUrl;
            }
            else
            {
                throw new Exception("ERROR: [GoogleService] BeginAuth the cleint not found!");
            }
        }

        public string RequestToken()
        {
            string code = HttpContext.Current.Request.Params["code"];
            if (code != null)
            {
                var post = string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code",
                                          code,
                                          HttpUtility.HtmlEncode(_client.ClientId),
                                          _client.ClientSecret,
                                          HttpUtility.HtmlEncode(_client.CallBackUrl));
                HttpWebRequest request = WebRequest.Create("https://accounts.google.com/o/oauth2/token") as HttpWebRequest;
                if (request == null)
                    return "access_denied";
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
            string profileUrl = string.Format("https://www.googleapis.com/oauth2/v1/userinfo?access_token={0}", _client.Token);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(profileUrl);
            request.Headers.Add("Accept-Language", "zh-cn");
            //request.Headers.Add("Authorization", "Bearer " + accessToken);

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

            var data = JsonConvert.DeserializeAnonymousType(result, new GoogleClinet.UserProfile());
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("source", "Google");
            dictionary.Add("id", data.Id);
            dictionary.Add("email", data.Email);
            dictionary.Add("verified_email", data.Verified_email);
            dictionary.Add("name", data.Name);
            dictionary.Add("given_name", data.Given_name);
            dictionary.Add("family_name", data.Family_name);
            dictionary.Add("link", data.Link);
            dictionary.Add("picture", data.Picture);
            dictionary.Add("gender", data.Gender);
            return dictionary;
        }
    }
}
