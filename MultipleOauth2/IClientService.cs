using System;
using System.Collections.Generic;

namespace MultipleOauth2
{
    public interface IClientService
    {
        string BeginAuthentication();
        string RequestToken();
        Dictionary<string, string> RequestUserProfile();
    }
}
