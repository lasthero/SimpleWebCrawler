using System;

namespace SimpleWebCrawler.Services
{
    public class WebCrawlerService
    {
        public bool Run(string url)
        {
           var uri = this.GetUri(url);
           if (!string.IsNullOrEmpty(url) && uri != null)
            return true;

            return false;
        }

        private Uri GetUri(string url)
        {
            Uri result;
            Uri.TryCreate(url, UriKind.Absolute, out result);
            return result;
        }
    }
}
