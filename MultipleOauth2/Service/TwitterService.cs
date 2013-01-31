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
    public class TwitterService : OAuthServiceBase, IClientService
    {
        private static string _oauthUrl = "";
        IClientProvider _client;
        private TwitterOAuthBase _oauth;

        public TwitterService()
        {
        }

        public TwitterService(IClientProvider oCleint)
        {
            _client = oCleint;
            _oauth = new TwitterOAuthBase();
            _oauth.ConsumerKey = _client.ClientId;
            _oauth.ConsumerSecret = _client.ClientSecret;
            _oauth.CallBackUrl = _client.CallBackUrl;
            _oauth.Proxy = _client.Proxy;
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
                _oauthUrl = _oauth.AuthorizationLinkGet();
                return _oauthUrl;
            }
            else
            {
                throw new Exception("ERROR: BeginAuth the cleint not found!");
            }
        }

        public string RequestToken()
        {
            string verifier = HttpContext.Current.Request["oauth_verifier"];
            string token = HttpContext.Current.Request["oauth_token"];
            if (!string.IsNullOrEmpty(verifier) && !string.IsNullOrEmpty(token))
            {
                _oauth.OAuthVerifier = verifier;
                _oauth.Token = token;

                return _oauth.Token;
            }
            return "access_denied";
        }

        public Dictionary<string, string> RequestUserProfile()
        {
            _oauth.AccessTokenGet(_oauth.Token, _oauth.OAuthVerifier);
            string profileUrl = string.Format("https://api.twitter.com/1.1/users/show.json?user_id={0}&screen_name={1}&include_entities=true", _oauth.UserId, _oauth.ScreenName);
            string result = _oauth.oAuthWebRequest(TwitterOAuthBase.Method.GET, profileUrl, string.Empty);

            var data = JsonConvert.DeserializeAnonymousType(result, new TwitterClient.UserProfile());

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("source", "T");
            dictionary.Add("id", data.ID.ToString());
            dictionary.Add("name", data.Name);
            //dictionary.Add("first_name", data.First_name);
            //dictionary.Add("last_name", data.Last_name);
            //dictionary.Add("link", data.Link);
            //dictionary.Add("gender", data.Gender);
            //dictionary.Add("email", data.Email);
            // dictionary.Add("picture", data.Picture);

            return dictionary;
        }
    }
}
