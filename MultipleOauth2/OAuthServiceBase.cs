using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultipleOauth2
{
    public abstract class OAuthServiceBase
    {
        internal abstract void CreateOAuthClient(IClientProvider oClient);
        internal abstract void CreateOAuthClient(MultiOAuthContext oContext);
    }
}
