using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
            DefaultWebCrawler crawler = new DefaultWebCrawler((Uri)null);
            WebCrawlerHtmlDocument doc = crawler.Craw();
            Assert.IsNull(doc);
        }

        [TestMethod]
        public void ShouldReturnSimpleWebDocument()
        {
            var url = "http://wiprodigital.com/";
            var uri = new Uri(url);
            DefaultWebCrawler crawler = new DefaultWebCrawler(uri);
            WebCrawlerHtmlDocument doc = crawler.Craw();
            Assert.IsNotNull(doc);
            Assert.AreEqual(uri.AbsoluteUri, doc.Uri.AbsoluteUri);
        }
    }
}
