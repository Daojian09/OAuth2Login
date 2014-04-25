using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Newtonsoft.Json;
using Oauth2Login.Client;
using Oauth2Login.Core;

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
                string post = string.Format("client_id={0}&redirect_uri={1}&client_secret={2}&code={3}",
                                            _client.ClientId,
                                            HttpUtility.HtmlEncode(_client.CallBackUrl),
                                            _client.ClientSecret,
                                            code);
                string resonseJson = RestfullRequest.Request(tokenUrl, "POST", "application/x-www-form-urlencoded", 
                                                                                        null, post, _client.Proxy);
                resonseJson = "{\"" + resonseJson.Replace("=", "\":\"").Replace("&", "\",\"") + "\"}";
                return JsonConvert.DeserializeAnonymousType(resonseJson, new { access_token = "" }).access_token;
            }
            return "access_denied";
        }

        public Dictionary<string, string> RequestUserProfile()
        {
            string profileUrl = string.Format("https://graph.facebook.com/me?access_token={0}", _client.Token);
            NameValueCollection header = new NameValueCollection();
            header.Add("Accept-Language", "en-US");
            string result = RestfullRequest.Request(profileUrl, "GET", "application/x-www-form-urlencoded", header,null, _client.Proxy);
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
