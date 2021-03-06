﻿using System;
using System.Collections.Generic;
using System.Text;
using SimpleWebCrawler.Services.Models;

namespace SimpleWebCrawler.Services
{
    interface IWebCrawlerService
    {
        IEnumerable<ParsedHtmlDocumentResult> Run(Uri uri);
    }
}
