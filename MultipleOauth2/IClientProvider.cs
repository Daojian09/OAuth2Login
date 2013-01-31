using System;
using System.Collections.Generic;

namespace MultipleOauth2
{
    public interface IClientProvider
    {
        Type ServiceType { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string CallBackUrl { get; set; }
        string Scope { get; set; }
        string AcceptedRedirectUrl { get; set; }
        string FailedRedirectUrl { get; set; }
        string Proxy { get; set; }
        string Token { get; set; }
        Dictionary<string, string> Profile { get; set; }
    }
}
