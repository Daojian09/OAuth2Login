using System;
using System.Collections.Generic;

namespace Oauth2Login
{
    public interface IOAuthContext
    {
        AbstractClientProvider Client { get;set;}
        IClientService Service { get; set; }

        string Token { get; set; }
        Dictionary<string, string> Profile { get; set; }
        string BeginAuth();
    }
}
