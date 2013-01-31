using System;

namespace Oauth2Login.Client
{
    public class FacebookClient : AbstractClientProvider
    {
         public FacebookClient()
        {
        }

         public FacebookClient(string oClientid, string oClientsecret, string oCallbackUrl, string oScope,
                                 string oAcceptedUrl, string oFailedUrl, string oProxy)
            : base(oClientid, oClientsecret, oCallbackUrl, oScope, oAcceptedUrl, oFailedUrl, oProxy)
        {
            ServiceType = typeof (Oauth2Login.Service.FacebookService);
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
