using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWebCrawler.Services.Models
{
    public class SimpleWebDocument
    {
        public string Url { get; private set; }

        public SimpleWebDocument(string url)
        {
            this.Url = url;
        }
    }
}
