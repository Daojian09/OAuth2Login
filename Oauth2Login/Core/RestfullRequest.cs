using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;

namespace Oauth2Login.Core
{
    public class RestfullRequest
    {
        public static string Request(string url, string method, string contentType, NameValueCollection header, string data, string proxyAddress = null)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = method;
            if (!string.IsNullOrEmpty(contentType))
                request.ContentType = contentType;
            if (header != null)
                request.Headers.Add(header);

            if (!string.IsNullOrEmpty(proxyAddress))
            {
                IWebProxy proxy = new WebProxy(proxyAddress);
                proxy.Credentials = new NetworkCredential();
                request.Proxy = proxy;
            }

            if (!string.IsNullOrEmpty(data))
            {
                using (StreamWriter swt = new StreamWriter(request.GetRequestStream()))
                {
                    swt.Write(data);
                }
            }

            string result = string.Empty;
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }
    }
}
