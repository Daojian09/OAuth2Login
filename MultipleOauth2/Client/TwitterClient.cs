using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace MultipleOauth2.Client
{
    public class TwitterClient : IClientProvider
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

        public TwitterClient() { }

        public TwitterClient(string oClientid, string oClientsecret, string oCallbackUrl, string oScope, string oAcceptedUrl, string oFailedUrl, string oProxy)
        {
            ServiceType = typeof(MultipleOauth2.Service.TwitterService);
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
            /// <summary>
            /// Gets or sets the user's Twitter ID
            /// </summary>
            public long ID { get; set; }

            /// <summary>
            /// Gets or sets the user's Twitter screen name.
            /// </summary>
            public string ScreenName { get; set; }

            /// <summary>
            /// Gets or sets the user's display name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the user's URL.
            /// </summary>
            public string Url { get; set; }

            /// <summary>
            /// Gets or sets the URL of the user's profile.
            /// </summary>
            public string ProfileUrl
            {
                get { return "http://twitter.com/" + this.ScreenName; }
            }

            /// <summary>
            /// Gets or sets the URL of the user's profile image in "normal" size (48x48).
            /// </summary>
            public string ProfileImageUrl { get; set; }

            /// <summary>
            /// Gets or sets the user's description
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Gets or sets the user's location.
            /// </summary>
            public string Location { get; set; }

            /// <summary>
            /// Gets or sets the date that the Twitter profile was created.
            /// </summary>
            public DateTime? CreatedDate { get; set; }

            /// <summary>
            /// Gets or sets the user's preferred language.
            /// </summary>
            public string Language { get; set; }

            /// <summary>
            /// Gets or sets the number of tweets this user has posted.
            /// </summary>
            public int StatusesCount { get; set; }

            /// <summary>
            /// Gets or sets the number of friends the user has (the number of users this user follows).
            /// </summary>
            public int FriendsCount { get; set; }

            /// <summary>
            /// Gets or sets the number of followers the user has.
            /// </summary>
            public int FollowersCount { get; set; }

            /// <summary>
            /// Gets or sets the number of tweets that the user has marked as favorites.
            /// </summary>
            public int FavoritesCount { get; set; }

            /// <summary>
            /// Gets or sets the number of lists the user is listed on.
            /// </summary>
            public int ListedCount { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether or not the authenticated user is following this user.
            /// </summary>
            public bool IsFollowing { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether or not a request has been sent by the authenticating user to follow this user.
            /// </summary>
            public bool IsFollowRequestSent { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether or not the user's tweets are protected.
            /// </summary>
            public bool IsProtected { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether or not the user has mobile notifications enabled.
            /// </summary>
            public bool IsNotificationsEnabled { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether or not the user is verified with Twitter.
            /// </summary>
            /// <remarks>
            /// See http://support.twitter.com/groups/31-twitter-basics/topics/111-features/articles/119135-about-verified-accounts.
            /// </remarks>
            public bool IsVerified { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether or not the user has enabled their account with geo location.
            /// </summary>
            public bool IsGeoEnabled { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether or not this profile is enabled for contributors. 
            /// </summary>
            public bool IsContributorsEnabled { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether or not this user is a translator. 
            /// </summary>
            public bool IsTranslator { get; set; }

            /// <summary>
            /// Gets or sets the user's time zone. 
            /// </summary>
            public string TimeZone { get; set; }

            /// <summary>
            /// Gets or sets the user's UTC offset in seconds.
            /// </summary>
            public int UtcOffset { get; set; }

            /// <summary>
            /// Gets or sets the color of the sidebar border on the user's Twitter profile page.
            /// </summary>
            public string SidebarBorderColor { get; set; }

            /// <summary>
            /// Gets or sets the color of the sidebar fill on the user's Twitter profile page.
            /// </summary>
            public string SidebarFillColor { get; set; }

            /// <summary>
            /// Gets or sets the color of the background of the user's Twitter profile page.
            /// </summary>
            public string BackgroundColor { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether or not the user's Twitter profile page uses a background image.
            /// </summary>
            public bool UseBackgroundImage { get; set; }

            /// <summary>
            /// Gets or sets the URL to a background image shown on the user's Twitter profile page.
            /// </summary>
            public string BackgroundImageUrl { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether or not the background image is tiled.
            /// </summary>
            public bool IsBackgroundImageTiled { get; set; }

            /// <summary>
            /// Gets or sets the text color on the user's Twitter profile page.
            /// </summary>
            public string TextColor { get; set; }

            /// <summary>
            /// Gets or sets the link color on the user's Twitter profile page.
            /// </summary>
            public string LinkColor { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether or not the user has selected to see all inline media from everyone. 
            /// If false, they will only see inline media from the users they follow.
            /// </summary>
            public bool ShowAllInlineMedia { get; set; }
        }
    }


    public class TwitterOAuthBase
    {
        private string _consumerKey = "";
        private string _consumerSecret = "";
        private string _token = "";
        private string _tokenSecret = "";
        private string _callBackUrl = "oob";
        private string _oauthVerifier = "";

        public enum Method { GET, POST, DELETE };
        public const string REQUEST_TOKEN = "http://twitter.com/oauth/request_token";
        public const string AUTHORIZE = "http://twitter.com/oauth/authorize";
        public const string ACCESS_TOKEN = "http://twitter.com/oauth/access_token";

        protected const string OAuthVersion = "1.0";
        protected const string OAuthParameterPrefix = "oauth_";
        //
        // List of know and used oauth parameters' names
        //        
        protected const string OAuthConsumerKeyKey = "oauth_consumer_key";
        protected const string OAuthCallbackKey = "oauth_callback";
        protected const string OAuthVersionKey = "oauth_version";
        protected const string OAuthSignatureMethodKey = "oauth_signature_method";
        protected const string OAuthSignatureKey = "oauth_signature";
        protected const string OAuthTimestampKey = "oauth_timestamp";
        protected const string OAuthNonceKey = "oauth_nonce";
        protected const string OAuthTokenKey = "oauth_token";
        protected const string OAuthTokenSecretKey = "oauth_token_secret";
        protected const string OAuthVerifierKey = "oauth_verifier";
        protected const string HMACSHA1SignatureType = "HMAC-SHA1";
        protected const string PlainTextSignatureType = "PLAINTEXT";
        protected const string RSASHA1SignatureType = "RSA-SHA1";
        protected Random random = new Random();
        protected string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        #region Properties
        public string ConsumerKey { get { return _consumerKey; } set { _consumerKey = value; } }
        public string ConsumerSecret { get { return _consumerSecret; } set { _consumerSecret = value; } }
        public string Token { get { return _token; } set { _token = value; } }
        public string TokenSecret { get { return _tokenSecret; } set { _tokenSecret = value; } }
        public string CallBackUrl { get { return _callBackUrl; } set { _callBackUrl = value; } }
        public string OAuthVerifier { get { return _oauthVerifier; } set { _oauthVerifier = value; } }
        public string Proxy { get; set; }


        //userId
        public string UserId { get; set; }
        public string ScreenName { get; set; }
        #endregion

        /// <summary>
        /// Provides a predefined set of algorithms that are supported officially by the protocol
        /// </summary>
        public enum SignatureTypes { HMACSHA1, PLAINTEXT, RSASHA1 }

        /// <summary>
        /// Provides an internal structure to sort the query parameter
        /// </summary>
        protected class QueryParameter
        {
            private string name = null;
            private string value = null;

            public QueryParameter(string name, string value)
            {
                this.name = name;
                this.value = value;
            }

            public string Name
            {
                get { return name; }
            }

            public string Value
            {
                get { return value; }
            }
        }

        /// <summary>
        /// Comparer class used to perform the sorting of the query parameters
        /// </summary>
        protected class QueryParameterComparer : IComparer<QueryParameter>
        {

            #region IComparer<QueryParameter> Members

            public int Compare(QueryParameter x, QueryParameter y)
            {
                if (x.Name == y.Name)
                {
                    return System.String.CompareOrdinal(x.Value, y.Value);
                }
                return System.String.CompareOrdinal(x.Name, y.Name);
            }

            #endregion
        }

        /// <summary>
        /// Helper function to compute a hash value
        /// </summary>
        /// <param name="hashAlgorithm">The hashing algoirhtm used. If that algorithm needs some initialization, like HMAC and its derivatives, they should be initialized prior to passing it to this function</param>
        /// <param name="data">The data to hash</param>
        /// <returns>a Base64 string of the hash value</returns>
        private string ComputeHash(HashAlgorithm hashAlgorithm, string data)
        {
            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException("hashAlgorithm");
            }

            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data");
            }

            byte[] dataBuffer = Encoding.ASCII.GetBytes(data);
            byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Internal function to cut out all non oauth query string parameters (all parameters not begining with "oauth_")
        /// </summary>
        /// <param name="parameters">The query string part of the Url</param>
        /// <returns>A list of QueryParameter each containing the parameter name and value</returns>
        private List<QueryParameter> GetQueryParameters(string parameters)
        {
            if (parameters.StartsWith("?"))
            {
                parameters = parameters.Remove(0, 1);
            }

            List<QueryParameter> result = new List<QueryParameter>();

            if (!string.IsNullOrEmpty(parameters))
            {
                string[] p = parameters.Split('&');
                foreach (string s in p)
                {
                    if (!string.IsNullOrEmpty(s) && !s.StartsWith(OAuthParameterPrefix))
                    {
                        if (s.IndexOf('=') > -1)
                        {
                            string[] temp = s.Split('=');
                            result.Add(new QueryParameter(temp[0], temp[1]));
                        }
                        else
                        {
                            result.Add(new QueryParameter(s, string.Empty));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// This is a different Url Encode implementation since the default .NET one outputs the percent encoding in lower case.
        /// While this is not a problem with the percent encoding spec, it is used in upper case throughout OAuth
        /// </summary>
        /// <param name="value">The value to Url encode</param>
        /// <returns>Returns a Url encoded string</returns>
        public string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();

            foreach (char symbol in value)
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + String.Format("{0:X2}", (int)symbol));
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Normalizes the request parameters according to the spec
        /// </summary>
        /// <param name="parameters">The list of parameters already sorted</param>
        /// <returns>a string representing the normalized parameters</returns>
        protected string NormalizeRequestParameters(IList<QueryParameter> parameters)
        {
            StringBuilder sb = new StringBuilder();
            QueryParameter p = null;
            for (int i = 0; i < parameters.Count; i++)
            {
                p = parameters[i];
                sb.AppendFormat("{0}={1}", p.Name, p.Value);

                if (i < parameters.Count - 1)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generate the signature base that is used to produce the signature
        /// </summary>
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="consumerKey">The consumer key</param>        
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="callBackUrl">The callback URL (for OAuth 1.0a).If your client cannot accept callbacks, the value MUST be 'oob' </param>
        /// <param name="oauthVerifier">This value MUST be included when exchanging Request Tokens for Access Tokens. Otherwise pass a null or an empty string</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <param name="signatureType">The signature type. To use the default values use <see cref="OAuthBase.SignatureTypes">OAuthBase.SignatureTypes</see>.</param>
        /// <returns>The signature base</returns>
        public string GenerateSignatureBase(Uri url, string consumerKey, string token, string tokenSecret, string callBackUrl, string oauthVerifier, string httpMethod, string timeStamp, string nonce, string signatureType, out string normalizedUrl, out string normalizedRequestParameters)
        {
            if (token == null)
            {
                token = string.Empty;
            }

            if (tokenSecret == null)
            {
                tokenSecret = string.Empty;
            }

            if (string.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentNullException("consumerKey");
            }

            if (string.IsNullOrEmpty(httpMethod))
            {
                throw new ArgumentNullException("httpMethod");
            }

            if (string.IsNullOrEmpty(signatureType))
            {
                throw new ArgumentNullException("signatureType");
            }

            normalizedUrl = null;
            normalizedRequestParameters = null;

            List<QueryParameter> parameters = GetQueryParameters(url.Query);
            parameters.Add(new QueryParameter(OAuthVersionKey, OAuthVersion));
            parameters.Add(new QueryParameter(OAuthNonceKey, nonce));
            parameters.Add(new QueryParameter(OAuthTimestampKey, timeStamp));
            parameters.Add(new QueryParameter(OAuthSignatureMethodKey, signatureType));
            parameters.Add(new QueryParameter(OAuthConsumerKeyKey, consumerKey));

            if (!string.IsNullOrEmpty(callBackUrl))
            {
                parameters.Add(new QueryParameter(OAuthCallbackKey, UrlEncode(callBackUrl)));
            }


            if (!string.IsNullOrEmpty(oauthVerifier))
            {
                parameters.Add(new QueryParameter(OAuthVerifierKey, oauthVerifier));
            }

            if (!string.IsNullOrEmpty(token))
            {
                parameters.Add(new QueryParameter(OAuthTokenKey, token));
            }

            parameters.Sort(new QueryParameterComparer());

            normalizedUrl = string.Format("{0}://{1}", url.Scheme, url.Host);
            if (!((url.Scheme == "http" && url.Port == 80) || (url.Scheme == "https" && url.Port == 443)))
            {
                normalizedUrl += ":" + url.Port;
            }
            normalizedUrl += url.AbsolutePath;
            normalizedRequestParameters = NormalizeRequestParameters(parameters);

            StringBuilder signatureBase = new StringBuilder();
            signatureBase.AppendFormat("{0}&", httpMethod.ToUpper());
            signatureBase.AppendFormat("{0}&", UrlEncode(normalizedUrl));
            signatureBase.AppendFormat("{0}", UrlEncode(normalizedRequestParameters));

            return signatureBase.ToString();
        }

        /// <summary>
        /// Generate the signature value based on the given signature base and hash algorithm
        /// </summary>
        /// <param name="signatureBase">The signature based as produced by the GenerateSignatureBase method or by any other means</param>
        /// <param name="hash">The hash algorithm used to perform the hashing. If the hashing algorithm requires initialization or a key it should be set prior to calling this method</param>
        /// <returns>A base64 string of the hash value</returns>
        public string GenerateSignatureUsingHash(string signatureBase, HashAlgorithm hash)
        {
            return ComputeHash(hash, signatureBase);
        }

        /// <summary>
        /// Generates a signature using the HMAC-SHA1 algorithm
        /// </summary>		
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="consumerKey">The consumer key</param>
        /// <param name="consumerSecret">The consumer seceret</param>
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="callBackUrl">The callback URL (for OAuth 1.0a).If your client cannot accept callbacks, the value MUST be 'oob' </param>
        /// <param name="oauthVerifier">This value MUST be included when exchanging Request Tokens for Access Tokens. Otherwise pass a null or an empty string</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <returns>A base64 string of the hash value</returns>
        public string GenerateSignature(Uri url, string consumerKey, string consumerSecret, string token, string tokenSecret, string callBackUrl, string oauthVerifier, string httpMethod, string timeStamp, string nonce, out string normalizedUrl, out string normalizedRequestParameters)
        {
            return GenerateSignature(url, consumerKey, consumerSecret, token, tokenSecret, callBackUrl, oauthVerifier, httpMethod, timeStamp, nonce, SignatureTypes.HMACSHA1, out normalizedUrl, out normalizedRequestParameters);
        }

        /// <summary>
        /// Generates a signature using the specified signatureType 
        /// </summary>		
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="consumerKey">The consumer key</param>
        /// <param name="consumerSecret">The consumer seceret</param>
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="callBackUrl">The callback URL (for OAuth 1.0a).If your client cannot accept callbacks, the value MUST be 'oob' </param>
        /// <param name="oauthVerifier">This value MUST be included when exchanging Request Tokens for Access Tokens. Otherwise pass a null or an empty string</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <param name="signatureType">The type of signature to use</param>
        /// <returns>A base64 string of the hash value</returns>
        public string GenerateSignature(Uri url, string consumerKey, string consumerSecret, string token, string tokenSecret, string callBackUrl, string oauthVerifier, string httpMethod, string timeStamp, string nonce, SignatureTypes signatureType, out string normalizedUrl, out string normalizedRequestParameters)
        {
            normalizedUrl = null;
            normalizedRequestParameters = null;

            switch (signatureType)
            {
                case SignatureTypes.PLAINTEXT:
                    return HttpUtility.UrlEncode(string.Format("{0}&{1}", consumerSecret, tokenSecret));
                case SignatureTypes.HMACSHA1:
                    string signatureBase = GenerateSignatureBase(url, consumerKey, token, tokenSecret, callBackUrl, oauthVerifier, httpMethod, timeStamp, nonce, HMACSHA1SignatureType, out normalizedUrl, out normalizedRequestParameters);

                    HMACSHA1 hmacsha1 = new HMACSHA1();
                    hmacsha1.Key = Encoding.ASCII.GetBytes(string.Format("{0}&{1}", UrlEncode(consumerSecret), string.IsNullOrEmpty(tokenSecret) ? "" : UrlEncode(tokenSecret)));

                    return GenerateSignatureUsingHash(signatureBase, hmacsha1);
                case SignatureTypes.RSASHA1:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException("Unknown signature type", "signatureType");
            }
        }

        /// <summary>
        /// Generate the timestamp for the signature        
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateTimeStamp()
        {
            // Default implementation of UNIX time of the current UTC time
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Generate a nonce
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateNonce()
        {
            // Just a simple implementation of a random number between 123400 and 9999999
            return random.Next(123400, 9999999).ToString(CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// Get the link to Twitter's authorization page for this application.
        /// </summary>
        /// <returns>The url with a valid request token, or a null string.</returns>
        public string AuthorizationLinkGet()
        {
            string ret = null;

            string response = oAuthWebRequest(Method.POST, REQUEST_TOKEN, String.Empty);
            if (response.Length > 0)
            {
                //response contains token and token secret.  We only need the token.
                NameValueCollection qs = HttpUtility.ParseQueryString(response);

                if (qs["oauth_callback_confirmed"] != null)
                {
                    if (qs["oauth_callback_confirmed"] != "true")
                    {
                        throw new Exception("OAuth callback not confirmed.");
                    }
                }

                if (qs["oauth_token"] != null)
                {
                    ret = AUTHORIZE + "?oauth_token=" + qs["oauth_token"] + "&force_login=true&display=popup";
                }
            }
            return ret;
        }

        /// <summary>
        /// Exchange the request token for an access token.
        /// </summary>
        /// <param name="authToken">The oauth_token is supplied by Twitter's authorization page following the callback.</param>
        /// <param name="oauthVerifier">An oauth_verifier parameter is provided to the client either in the pre-configured callback URL</param>
        public void AccessTokenGet(string authToken, string oauthVerifier)
        {
            this.Token = authToken;
            this.OAuthVerifier = oauthVerifier;

            string response = oAuthWebRequest(Method.GET, ACCESS_TOKEN, String.Empty);

            if (response.Length > 0)
            {
                //Store the Token and Token Secret
                NameValueCollection qs = HttpUtility.ParseQueryString(response);
                if (qs["oauth_token"] != null)
                {
                    this.Token = qs["oauth_token"];
                }
                if (qs["oauth_token_secret"] != null)
                {
                    this.TokenSecret = qs["oauth_token_secret"];
                }

                if (qs["user_id"] != null)
                {
                    this.UserId = qs["user_id"];
                }

                if (qs["screen_name"] != null)
                {
                    this.ScreenName = qs["screen_name"];
                }
            }
        }

        /// <summary>
        /// Submit a web request using oAuth.
        /// </summary>
        /// <param name="method">GET or POST</param>
        /// <param name="url">The full url, including the querystring.</param>
        /// <param name="postData">Data to post (querystring format)</param>
        /// <returns>The web server response.</returns>
        public string oAuthWebRequest(Method method, string url, string postData)
        {
            string outUrl = "";
            string querystring = "";
            string ret = "";


            //Setup postData for signing.
            //Add the postData to the querystring.
            if (method == Method.POST || method == Method.DELETE)
            {
                if (postData.Length > 0)
                {
                    //Decode the parameters and re-encode using the oAuth UrlEncode method.
                    NameValueCollection qs = HttpUtility.ParseQueryString(postData);
                    postData = "";
                    foreach (string key in qs.AllKeys)
                    {
                        if (postData.Length > 0)
                        {
                            postData += "&";
                        }
                        qs[key] = HttpUtility.UrlDecode(qs[key]);
                        qs[key] = this.UrlEncode(qs[key]);
                        postData += key + "=" + qs[key];

                    }
                    if (url.IndexOf("?", System.StringComparison.Ordinal) > 0)
                    {
                        url += "&";
                    }
                    else
                    {
                        url += "?";
                    }
                    url += postData;
                }
            }

            Uri uri = new Uri(url);

            string nonce = this.GenerateNonce();
            string timeStamp = this.GenerateTimeStamp();

            //Generate Signature
            string sig = this.GenerateSignature(uri,
                this.ConsumerKey,
                this.ConsumerSecret,
                this.Token,
                this.TokenSecret,
                this.CallBackUrl,
                this.OAuthVerifier,
                method.ToString(),
                timeStamp,
                nonce,
                out outUrl,
                out querystring);

            querystring += "&oauth_signature=" + this.UrlEncode(sig);

            //Convert the querystring to postData
            if (method == Method.POST || method == Method.DELETE)
            {
                postData = querystring;
                querystring = "";
            }

            if (querystring.Length > 0)
            {
                outUrl += "?";
            }

            ret = WebRequest(method, outUrl + querystring, postData);

            return ret;
        }

        /// <summary>
        /// Web Request Wrapper
        /// </summary>
        /// <param name="method">Http Method</param>
        /// <param name="url">Full url to the web resource</param>
        /// <param name="postData">Data to post in querystring format</param>
        /// <returns>The web server response.</returns>
        public string WebRequest(Method method, string url, string postData)
        {
            HttpWebRequest webRequest = null;
            StreamWriter requestWriter = null;
            string responseData = "";

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            if (webRequest != null)
            {
                if (!string.IsNullOrEmpty(this.Proxy))
                {
                    IWebProxy proxy = new WebProxy(this.Proxy);
                    proxy.Credentials = new NetworkCredential();
                    webRequest.Proxy = proxy;
                }
                webRequest.Method = method.ToString();
                webRequest.ServicePoint.Expect100Continue = false;
                //webRequest.UserAgent  = "Identify your application please.";
                //webRequest.Timeout = 20000;

                if (method == Method.POST || method == Method.DELETE)
                {
                    webRequest.ContentType = "application/x-www-form-urlencoded";

                    //POST the data.
                    requestWriter = new StreamWriter(webRequest.GetRequestStream());
                    try
                    {
                        requestWriter.Write(postData);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        requestWriter.Close();
                        requestWriter = null;
                    }
                }

                responseData = WebResponseGet(webRequest);
            }

            webRequest = null;

            return responseData;

        }

        /// <summary>
        /// Process the web response.
        /// </summary>
        /// <param name="webRequest">The request object.</param>
        /// <returns>The response data.</returns>
        public string WebResponseGet(HttpWebRequest webRequest)
        {
            StreamReader responseReader = null;
            string responseData = "";

            try
            {
                if (webRequest != null) responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                if (responseReader != null) responseData = responseReader.ReadToEnd();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (webRequest != null) webRequest.GetResponse().GetResponseStream().Close();
                if (responseReader != null) responseReader.Close();
                responseReader = null;
            }

            return responseData;
        }

    }

}
