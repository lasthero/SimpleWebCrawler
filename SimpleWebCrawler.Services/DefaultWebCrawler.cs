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
            
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(this.WebUri);
            return ParseHtmlDoc(htmlDoc);
        }

        private WebCrawlerHtmlDocument ParseHtmlDoc(HtmlDocument htmlDoc)
        {
            var parsedHtmlDoc = new WebCrawlerHtmlDocument(this.WebUri);
            var links = htmlDoc.DocumentNode.SelectSingleNode("//body").SelectNodes("//a").Select(e => e.GetAttributeValue("href", null)).Where(s => !string.IsNullOrEmpty(s)).GroupBy(a=>a).Select(a=>a.Key).ToList();
            Uri uri;
            foreach (var link in links)
            {
                if (Uri.TryCreate(link, UriKind.Absolute, out uri))
                {
                    if (uri.Host != this.WebUri.Host) //external links
                    {
                        parsedHtmlDoc.AddExternalLink(uri);
                    }
                    else // links to internal pages
                    {
                        parsedHtmlDoc.AddNode(new WebCrawlerHtmlDocument(uri));
                    }
                }
                else
                {
                    //log errors
                }
            }
            return parsedHtmlDoc;
        }
    }
}
