using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleWebCrawler.Services.Models
{
    public class ParsedHtmlDocumentResult
    {
        public Uri Uri { get; private set; }
        public HashSet<string> StaticContents { get; private set; }
        public HashSet<string> ExternalLinks { get; private set; }
        public HashSet<string> InternalLinks { get; private set; }

        public ParsedHtmlDocumentResult(Uri uri)
        {
            this.Uri = uri;
            this.StaticContents = new HashSet<string>();
            this.ExternalLinks = new HashSet<string>();
            this.InternalLinks = new HashSet<string>();
        }

        public void AddInternalLink(Uri link)
        {
            if (!this.InternalLinks.Contains(link.AbsoluteUri))
                this.InternalLinks.Add(link.AbsoluteUri);
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
    }
}
