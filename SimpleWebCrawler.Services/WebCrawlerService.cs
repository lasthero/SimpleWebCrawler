using System;
using System.Collections.Generic;
using SimpleWebCrawler.Services.Models;

namespace SimpleWebCrawler.Services
{
    public class WebCrawlerService: IWebCrawlerService
    {
        public IEnumerable<ParsedHtmlDocumentResult> Run(Uri uri)
        {
            return new DefaultWebCrawler(uri).Craw();
        }
    }
}
