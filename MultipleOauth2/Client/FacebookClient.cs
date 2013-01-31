using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultipleOauth2.Service;

namespace MultipleOauth2.Client
{
    public class FacebookClient : IClientProvider
    {
        public Type ServiceType { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CallBackUrl { get; set; }
        public string Scope { get; set; }
        public string AcceptedRedirectUrl { get; set; }
        public string FailedRedirectUrl { get; set; }
        public string Proxy { get; set; }
        public string Token { get; set; }
        public Dictionary<string, string> Profile { get; set; }

        public FacebookClient() { }

        public FacebookClient(string oClientid, string oClientsecret, string oCallbackUrl, string oScope, string oAcceptedUrl, string oFailedUrl,string oProxy)
        {
            ServiceType = typeof(FacebookService);
            ClientId = oClientid;
            ClientSecret = oClientsecret;
            CallBackUrl = oCallbackUrl;
            Scope = oScope;
            AcceptedRedirectUrl = oAcceptedUrl;
            FailedRedirectUrl = oFailedUrl;
            Proxy = oProxy;
            Token = "";
        }

        public class UserProfile
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string First_name { get; set; }
            public string Last_name { get; set; }
            public string Link { get; set; }
            public string Gender { get; set; }
            public string Email { get; set; }
            public string Picture { get; set; }
        }
    }
}
