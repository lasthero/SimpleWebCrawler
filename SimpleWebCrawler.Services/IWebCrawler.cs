using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SimpleWebCrawler.Services.Models;

namespace SimpleWebCrawler.Services
{
    interface IWebCrawler
    {
        IList<ParsedHtmlDocumentResult> Craw();
    }
}
