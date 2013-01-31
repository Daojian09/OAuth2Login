using System;
using System.Collections.Generic;

namespace MultipleOauth2.Client
{
    public class WindowsLiveClient : IClientProvider
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

        public WindowsLiveClient() { }

        public WindowsLiveClient(string oClientid, string oClientsecret, string oCallbackUrl, string oScope, string oAcceptedUrl, string oFailedUrl,string oProxy)
        {
            ServiceType = typeof(MultipleOauth2.Service.WindowsLiveService);
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
            public Emails Emails { get; set; }
        }

        public class Emails
        {
            public string Preferred { get; set; }
            public string Account { get; set; }
            public string Personal { get; set; }
            public string Business { get; set; }
        }

    }
}
