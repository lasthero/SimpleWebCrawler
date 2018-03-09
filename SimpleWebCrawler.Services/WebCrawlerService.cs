using System;
using System.Collections.Generic;
using SimpleWebCrawler.Services.Models;

namespace SimpleWebCrawler.Services
{
    public class WebCrawlerService
    {
        public IList<ParsedHtmlDocumentResult> Run(Uri uri)
        {
           if (uri == null)
            return null;

            return new DefaultWebCrawler(uri).Craw();
        }
    }
}
