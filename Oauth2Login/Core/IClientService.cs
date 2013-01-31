using System;
using System.Collections.Generic;

namespace Oauth2Login
{
    public interface IClientService
    {
        void CreateOAuthClient(IOAuthContext oContext);
        void CreateOAuthClient(AbstractClientProvider oClient);
        
        string BeginAuthentication();
        string RequestToken();
        Dictionary<string, string> RequestUserProfile();
    }
}
