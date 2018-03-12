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
        
        //Implemented BST for web crawing
        public IEnumerable<ParsedHtmlDocumentResult> Craw()
        {
            IDictionary<Uri, ParsedHtmlDocumentResult> visitedPages = new Dictionary<Uri, ParsedHtmlDocumentResult>();
            //Page queue stores pages to be visited
            Queue<Uri> pageQueue = new Queue<Uri>();
            // Test if the url is active
            var responseUri = WebCrawlerUtil.GetResponseUri(this.WebUri);
            this.WebUri = responseUri ?? throw new Exception(string.Format("The URL is either invalid or not found: {0}", this.WebUri.AbsoluteUri));

            //Start from the main page
            var parsedHtmlDoc = this.ParseHtmlDoc(this.WebUri, pageQueue, visitedPages);
            visitedPages.Add(this.WebUri, parsedHtmlDoc);
            yield return parsedHtmlDoc;
            
            //Process queue
            while (pageQueue.Count > 0)
            {
                var item = pageQueue.Dequeue();
                if (!visitedPages.ContainsKey(item))
                {
                    parsedHtmlDoc = this.ParseHtmlDoc(item, pageQueue, visitedPages);
                    visitedPages.Add(item, parsedHtmlDoc);
                    yield return parsedHtmlDoc;
                }
            }
        }

        //Use HTML agility pack to parse HTML pages; 
        //if the page is within the same domain and it has not been visited and is not in the queue, add to the queue for processing
        private ParsedHtmlDocumentResult ParseHtmlDoc(Uri uri, Queue<Uri> pageQueue, IDictionary<Uri, ParsedHtmlDocumentResult> visitedPages)
        {
            var web = new HtmlWeb();
            var htmlDoc = web.Load(uri);
            var parsedHtmlDoc = new ParsedHtmlDocumentResult(uri);
            try
            {
                var nodes = htmlDoc.DocumentNode.SelectNodes(
                    "//a[@href] | //link[@rel='stylesheet' and @href] | //img[@src] | //script[@type='text/javascript' and @src='*.js']");
                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        var linkUrl = this.GetNodeLink(node);
                        uri = WebCrawlerUtil.ConvertToAbsoluteUri(linkUrl, parsedHtmlDoc.Uri);
                        if (uri != null && uri != this.WebUri && uri != parsedHtmlDoc.Uri)
                        {
                            if (uri.Host == this.WebUri.Host) //internal links
                            {
                                if (node.Name == "a") //links to internal pages
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
                            //log messages
                        }
                    }
                }

                return parsedHtmlDoc;
            }
            catch (Exception ex)
            {
                parsedHtmlDoc.ErrorMessage = string.Format("Exception occured while peocessing {0}; message: {1}",
                    parsedHtmlDoc.Uri.AbsoluteUri, ex.Message);
                return parsedHtmlDoc;
            }
        }

        //for this implementation, we are only interested at anchor, image, script, and css links 
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
    }
}
