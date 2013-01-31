using System;
using System.Collections.Generic;

namespace Oauth2Login
{
    public abstract class AbstractClientProvider
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
        public string ProfileJsonString { get; set; }

        protected AbstractClientProvider() { }

        protected AbstractClientProvider(string oClientid, string oClientsecret, string oCallbackUrl, string oScope, string oAcceptedUrl, string oFailedUrl, string oProxy)
        {
            ClientId = oClientid;
            ClientSecret = oClientsecret;
            CallBackUrl = oCallbackUrl;
            Scope = oScope;
            AcceptedRedirectUrl = oAcceptedUrl;
            FailedRedirectUrl = oFailedUrl;
            Proxy = oProxy;
            Token = "";
        }
    }
}
