using SimpleWebCrawler.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWebCrawler.Services
{
    public class DefaultWebCrawler:IWebCrawler
    {
        public string Url { get; private set; }
        public DefaultWebCrawler(string url)
        {
            this.Url = url;
        }

        public SimpleWebDocument Craw()
        {
            if (string.IsNullOrEmpty(this.Url))
                return null;

            return new SimpleWebDocument(this.Url);
        }
    }
}
