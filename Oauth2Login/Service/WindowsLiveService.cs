using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Newtonsoft.Json;
using Oauth2Login.Client;
using Oauth2Login.Core;

namespace Oauth2Login.Service
{
    public class WindowsLiveService : IClientService
    {
        private static string _oauthUrl = "";
        private AbstractClientProvider _client;

        public WindowsLiveService()
        {
        }

        public WindowsLiveService(AbstractClientProvider oCleint)
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
                _oauthUrl = string.Format("https://login.live.com/oauth20_authorize.srf?" +
                        "client_id={0}&scope={1}&display=popup&target=self&response_type=code&redirect_uri={2}",
                        HttpUtility.HtmlEncode(_client.ClientId),
                        HttpUtility.HtmlEncode(_client.Scope),
                        HttpUtility.HtmlEncode(_client.CallBackUrl)
                        );
                return _oauthUrl;
            }
            throw new Exception("ERROR: windows live BeginAuth the cleint not found!");
        }

        public string RequestToken()
        {
            string code = HttpContext.Current.Request.Params["code"];
            if (code != null)
            {
                string tokenUrl = string.Format("https://login.live.com/oauth20_token.srf");
                string post = string.Format("client_id={0}&redirect_uri={1}&target=self&client_secret={2}&code={3}&grant_type=authorization_code",
                                            HttpUtility.HtmlEncode(_client.ClientId),
                                            HttpUtility.HtmlEncode(_client.CallBackUrl),
                                            _client.ClientSecret,
                                            code);
                string resonseJson = RestfullRequest.Request(tokenUrl, "POST", "application/x-www-form-urlencoded", null, post, _client.Proxy);
                return JsonConvert.DeserializeAnonymousType(resonseJson, new { access_token = "" }).access_token;
            }
            return "access_denied";
        }

        public Dictionary<string, string> RequestUserProfile()
        {
           
            string profileUrl = string.Format("https://apis.live.net/v5.0/me?access_token={0}", _client.Token);
            NameValueCollection header = new NameValueCollection();
            header.Add("Accept-Language", "en_US");
            string result = RestfullRequest.Request(profileUrl, "GET",null, header, null, _client.Proxy);
           _client.ProfileJsonString = result;
            WindowsLiveClient.UserProfile data = JsonConvert.DeserializeAnonymousType(result, new WindowsLiveClient.UserProfile());

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
