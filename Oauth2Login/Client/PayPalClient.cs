using System;

namespace Oauth2Login.Client
{
    public class PayPalClient : AbstractClientProvider
    {
        public PayPalClient()
        {
        }

        public PayPalClient(string oClientid, string oClientsecret, string oCallbackUrl, string oScope,
                                string oAcceptedUrl, string oFailedUrl, string oProxy)
            : base(oClientid, oClientsecret, oCallbackUrl, oScope, oAcceptedUrl, oFailedUrl, oProxy)
        {
            ServiceType = typeof(Oauth2Login.Service.PayPalService);
        }


        public class UserProfile
        {
            public Address address { get; set; }
            public string Language { get; set; }
            public string Phone_number { get; set; }
            public string Locale { get; set; }
            public string Zoneinfo { get; set; }
            public string Email { get; set; }
            public DateTime Birthday { get; set; }
            public string User_id { get; set; }
            public string Name { get; set; }
            public string Given_name { get; set; }
            public string Family_name { get; set; }
            public string Verified_email { get; set; }
            public string Gender { get; set; }
            public string Picture { get; set; }
        }

        public class Address
        {
            public int Postal_code { get; set; }
            public string Locality { get; set; }
            public string Region { get; set; }
            public string Country { get; set; }
            public string Street_address { get; set; }
        }
    }
}
