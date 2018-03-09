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
        public HashSet<string> ExternalLinks { get; private set; }
        public IList<WebCrawlerHtmlDocument> Nodes { get; private set; }

        public WebCrawlerHtmlDocument(Uri uri)
        {
            this.Uri = uri;
            this.StaticContents = new HashSet<string>();
            this.ExternalLinks = new HashSet<string>();
            this.Nodes = new List<WebCrawlerHtmlDocument>();
        }

        public void AddNode(WebCrawlerHtmlDocument node)
        {
            this.Nodes.Add(node);
        }

        public void AddExternalLink(Uri link)
        {
            if (!this.ExternalLinks.Contains(link.AbsoluteUri))
                this.ExternalLinks.Add(link.AbsoluteUri);
        }

        public void AddStaticContent(string content)
        {
            if (!this.StaticContents.Contains(content))
                this.StaticContents.Add(content);
        }

        public IList<String> GetInternalLinkList()
        {
            return this.Nodes.Select(a=>a.Uri.AbsoluteUri).ToList();
        }
        
        public override bool Equals(object obj)
        {
            return this.Uri.AbsoluteUri == ((WebCrawlerHtmlDocument) obj).Uri.AbsoluteUri;
        }

        public override int GetHashCode()
        {
            return this.Uri.GetHashCode();
        }
    }
}
