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
    public class DefaultWebCrawler : IWebCrawler
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

        public IList<ParsedHtmlDocumentResult> Craw()
        {
            if (this.WebUri == null)
                return null;
            IDictionary<Uri, ParsedHtmlDocumentResult> visitedPages = new Dictionary<Uri, ParsedHtmlDocumentResult>();
            Queue<Uri> queue = new Queue<Uri>();
            //Start from the main page
            
            var parsedHtmlDoc = this.ParseHtmlDoc(this.WebUri);
            visitedPages.Add(this.WebUri, parsedHtmlDoc);
            foreach (var childLink in parsedHtmlDoc.InternalLinks)
            {
                var newUri = new Uri(childLink);
                if (!queue.Contains(newUri))
                    queue.Enqueue(newUri);
            }

            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                if (!visitedPages.ContainsKey(item))
                {
                    var parsedPage = this.ParseHtmlDoc(item);
                    visitedPages.Add(item, parsedPage);
                    foreach (var link in parsedPage.InternalLinks)
                    {
                        var newUri = new Uri(link);
                        if (!queue.Contains(newUri))
                            queue.Enqueue(newUri);
                    }
                }
            }
            return visitedPages.Values.ToList();
        }

        private ParsedHtmlDocumentResult ParseHtmlDoc(Uri uri)
        {
            var web = new HtmlWeb();
            var htmlDoc = web.Load(uri);
            var parsedHtmlDoc = new ParsedHtmlDocumentResult(uri);
            ParseLinks(parsedHtmlDoc, htmlDoc);
            ParseStaticContents(parsedHtmlDoc, htmlDoc);
            return parsedHtmlDoc;
        }

        private void ParseLinks(ParsedHtmlDocumentResult parsedHtmlDoc, HtmlDocument htmlDoc)
        {
            //Use Group By to elimate duplicates
            var links = htmlDoc.DocumentNode.SelectSingleNode("//body").SelectNodes("//a")
                .Select(e => e.GetAttributeValue("href", null)).Where(s => !string.IsNullOrEmpty(s)).GroupBy(a => a)
                .Select(a => a.Key).ToList();
            Uri uri;
            foreach (var link in links)
            {
                if (Uri.TryCreate(link, UriKind.Absolute, out uri) && uri != this.WebUri)
                {
                    if (uri.Host != this.WebUri.Host) //external links
                    {
                        parsedHtmlDoc.AddExternalLink(uri);
                    }
                    else // links to internal pages
                    {
                        parsedHtmlDoc.AddInternalLink(uri);
                    }
                }
                else
                {
                    //log errors
                }
            }
        }

        private void ParseStaticContents(ParsedHtmlDocumentResult parsedHtmlDoc, HtmlDocument htmlDoc)
        {
            //get static images, .css, and .js
            var imageUrls = htmlDoc.DocumentNode.Descendants("img")
                .Select(e => e.GetAttributeValue("src", null))
                .Where(s => !string.IsNullOrEmpty(s)).Distinct();
            foreach (var img in imageUrls)
            {
                Uri uri;
                if (Uri.TryCreate(img, UriKind.Absolute, out uri) && uri.Host == this.WebUri.Host)
                    parsedHtmlDoc.AddStaticContent(uri.AbsoluteUri);
            }
        }
    }
}
