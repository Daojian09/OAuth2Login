using System;

namespace Oauth2Login.Client
{
    public class WindowsLiveClient :AbstractClientProvider
    {
        public WindowsLiveClient()
        {
        }

        public WindowsLiveClient(string oClientid, string oClientsecret, string oCallbackUrl, string oScope,
                                 string oAcceptedUrl, string oFailedUrl, string oProxy)
            : base(oClientid, oClientsecret, oCallbackUrl, oScope, oAcceptedUrl, oFailedUrl, oProxy)
        {
            ServiceType = typeof (Oauth2Login.Service.WindowsLiveService);
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
