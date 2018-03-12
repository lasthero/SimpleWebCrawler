using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace SimpleWebCrawler.Services
{
    public static class WebCrawlerUtil
    {
        public static Uri GetResponseUri(Uri uri)
        {
            try
            {
                if (uri == null)
                    return null;
                var request = WebRequest.Create(uri);
                var response = (HttpWebResponse) request.GetResponse();
                uri = response.ResponseUri;
                response.Close();
                return uri;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Uri ConvertToAbsoluteUri(string url, Uri baseUri)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return null;
                if (url.StartsWith("http://") || url.StartsWith("https://"))
                    return new Uri(url);
                else
                {
                    return new Uri(baseUri, url);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
