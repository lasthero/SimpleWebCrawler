using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SimpleWebCrawler.Services.Models
{
    [XmlRoot(ElementName = "SiteMap")]
    public class ParsedHtmlDocumentXmlSerialization
    {
        public List<ParsedHtmlDocumentResult> Pages { get; set; }

        public ParsedHtmlDocumentXmlSerialization()
        {
            this.Pages = new List<ParsedHtmlDocumentResult>();
        }
    }
}
