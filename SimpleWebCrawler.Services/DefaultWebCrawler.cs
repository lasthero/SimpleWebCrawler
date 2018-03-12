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
        
        public IEnumerable<ParsedHtmlDocumentResult> Craw()
        {
            IDictionary<Uri, ParsedHtmlDocumentResult> visitedPages = new Dictionary<Uri, ParsedHtmlDocumentResult>();
            Queue<Uri> queue = new Queue<Uri>();
            Uri uri;
            // Test if the url is active
            var responseUri = WebCrawlerUtil.GetResponseUri(this.WebUri);
            this.WebUri = responseUri ?? throw new Exception(string.Format("The URL is either invalid or not found: {0}", this.WebUri.AbsoluteUri));

            //Start from the main page
            var parsedHtmlDoc = this.ParseHtmlDoc(this.WebUri, queue, visitedPages);
            visitedPages.Add(this.WebUri, parsedHtmlDoc);
            yield return parsedHtmlDoc;
            
            //Process queue
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                if (!visitedPages.ContainsKey(item))
                {
                    parsedHtmlDoc = this.ParseHtmlDoc(item, queue, visitedPages);
                    visitedPages.Add(item, parsedHtmlDoc);
                    yield return parsedHtmlDoc;
                }
            }
        }

        private ParsedHtmlDocumentResult ParseHtmlDoc(Uri uri, Queue<Uri> pageQueue, IDictionary<Uri, ParsedHtmlDocumentResult> visitedPages)
        {
            var web = new HtmlWeb();
            var htmlDoc = web.Load(uri);
            var parsedHtmlDoc = new ParsedHtmlDocumentResult(uri);
            var nodes = htmlDoc.DocumentNode.SelectNodes("//a[@href] | //link[@rel='stylesheet' and @href] | //img[@src] | //script[@type='text/javascript' and @src='*.js']");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var linkUrl = this.GetNodeLink(node);
                    uri = ConvertToAbsoluteUri(linkUrl, parsedHtmlDoc.Uri);
                    if (uri != null && uri != this.WebUri && uri != parsedHtmlDoc.Uri)
                    {
                        if (uri.Host == this.WebUri.Host) //internal links
                        {
                            if (node.Name == "a")
                            {
                                if (!visitedPages.ContainsKey(uri) && !pageQueue.Contains(uri))
                                    pageQueue.Enqueue(uri);
                            }
                            else
                                parsedHtmlDoc.AddStaticContent(uri);
                        }
                        else // links to external pages
                        {
                            parsedHtmlDoc.AddExternalLink(uri);
                        }
                    }
                    else
                    {
                        //log errors
                    }
                }
            }
            return parsedHtmlDoc;
        }

        private string GetNodeLink(HtmlNode node)
        {
            var returnVal = string.Empty;
            switch (node.Name)
            {
                case "img":
                case "script":
                    returnVal = node.Attributes["src"] != null ? node.Attributes["src"].Value : string.Empty;
                    break;
                case "a":
                case "link":
                    returnVal = node.Attributes["href"] != null? node.Attributes["href"].Value : string.Empty;
                    break;
            }

            return returnVal;
        }

        private Uri ConvertToAbsoluteUri(string url, Uri baseUri)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return null;
                if (url.StartsWith("http://") || url.StartsWith("https://"))
                    return new Uri(url);
                else
                {
                    return new Uri(baseUri, url);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
