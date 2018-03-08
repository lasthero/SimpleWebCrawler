using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleWebCrawler.Services;
using SimpleWebCrawler.Services.Models;

namespace SimpleWebCrawler.Test
{
    [TestClass]
    public class WebCrawlerTest
    {
        public WebCrawlerTest() { }

        [TestMethod]
        public void ShouldReturnNullForEmptyUrl()
        {
            DefaultWebCrawler crawler = new DefaultWebCrawler(string.Empty);
            SimpleWebDocument doc = crawler.Craw();
            Assert.IsNull(doc);
        }

        [TestMethod]
        public void ShouldReturnSimpleWebDocument()
        {
            var url = "http://wiprodigital.com";
            DefaultWebCrawler crawler = new DefaultWebCrawler(url);
            SimpleWebDocument doc = crawler.Craw();
            Assert.IsNotNull(doc);
            Assert.AreEqual(url, doc.Url);
        }
    }
}
