using System;

namespace SimpleWebCrawler.Services
{
    public class WebCrawlerService
    {
        public bool Run(string url){
           if (!string.IsNullOrEmpty(url) && this.IsValidUrl(url))
            return true;

            return false;
        }

        private bool IsValidUrl(string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }
    }
}
