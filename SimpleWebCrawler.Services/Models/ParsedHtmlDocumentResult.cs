using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SimpleWebCrawler.Services.Models
{
    public class ParsedHtmlDocumentResult
    {
        [XmlIgnore]
        public Uri Uri { get; set; }

        [XmlAttribute]
        public string PageUrl { get; set; }

        [XmlArrayItem("URL")]
        public HashSet<string> StaticContents { get; private set; }

        [XmlArrayItem("URL")]
        public HashSet<string> ExternalLinks { get; private set; }

        public ParsedHtmlDocumentResult()
        {
            this.StaticContents = new HashSet<string>();
            this.ExternalLinks = new HashSet<string>();
        }

        public ParsedHtmlDocumentResult(Uri uri)
        {
            this.Uri = uri;
            this.StaticContents = new HashSet<string>();
            this.ExternalLinks = new HashSet<string>();
            this.PageUrl = uri.AbsoluteUri;
        }

        public void AddExternalLink(Uri link)
        {
            if (!this.ExternalLinks.Contains(link.AbsoluteUri))
                this.ExternalLinks.Add(link.AbsoluteUri);
        }

        public void AddStaticContent(Uri link)
        {
            if (!this.StaticContents.Contains(link.AbsoluteUri))
                this.StaticContents.Add(link.AbsoluteUri);
        }
    }
}
