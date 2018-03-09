using SimpleWebCrawler.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SimpleWebCrawler.Services
{
    public class DefaultWebCrawler:IWebCrawler
    {
        public Uri WebUri { get; private set; }

        public DefaultWebCrawler(Uri uri)
        {
            this.WebUri = uri;
        }

        public DefaultWebCrawler(string url)
        {
            Uri uri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
                throw new Exception("Invalid URL");
            this.WebUri = uri;
        }

        public WebCrawlerHtmlDocument Craw()
        {
            if (this.WebUri == null)
                return null;
            var parsedHtmlDoc = new WebCrawlerHtmlDocument(this.WebUri);

            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(this.WebUri);

            var links = htmlDoc.DocumentNode.SelectSingleNode("//body").SelectNodes("//a").Select(e=>e.GetAttributeValue("href", null)).Where(s=> !string.IsNullOrEmpty(s)).ToList();
            this.ExtractLinks(parsedHtmlDoc, links);
            return parsedHtmlDoc;
        }

        private void ExtractLinks(WebCrawlerHtmlDocument parsedHtmlDoc, IList<string> links)
        {
            Uri uri;
            foreach (var link in links)
            {
                if (Uri.TryCreate(link, UriKind.Absolute, out uri))
                {
                    parsedHtmlDoc.AddLink(new WebCrawlerHtmlLink(){ Url=uri.AbsoluteUri, IsExternal = (uri.Host!=this.WebUri.Host) });
                }
                else
                {
                    //log errors
                }
                    
            }
        }
    }
}
