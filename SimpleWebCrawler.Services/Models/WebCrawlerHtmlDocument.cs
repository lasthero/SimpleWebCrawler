using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleWebCrawler.Services.Models
{
    public class WebCrawlerHtmlDocument
    {
        public Uri Uri { get; private set; }
        public HashSet<string> StaticContents { get; private set; }
        public IDictionary<string, WebCrawlerHtmlLink> Links { get; private set; }

        public WebCrawlerHtmlDocument(Uri uri)
        {
            this.Uri = uri;
            this.StaticContents = new HashSet<string>();
            this.Links = new Dictionary<string, WebCrawlerHtmlLink>();
        }

        public void AddLink(WebCrawlerHtmlLink link)
        {
            if (!this.Links.ContainsKey(link.Url))
                this.Links.Add(link.Url, link);
        }

        public void AddStaticContent(string content)
        {
            if (!this.StaticContents.Contains(content))
                this.StaticContents.Add(content);
        }

        public IList<String> GetLinkList(bool isExternal)
        {
            return this.Links.Values.Where(a => a.IsExternal == isExternal).Select(a=>a.Url).ToList();
        }
    }
}
