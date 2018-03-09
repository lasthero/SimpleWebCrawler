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
            var result = crawler.Craw();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldReturnResult()
        {
            var url = "http://wiprodigital.com/";
            //var url = "http://iot.wiprodigital.com/";
            var uri = new Uri(url);
            DefaultWebCrawler crawler = new DefaultWebCrawler(uri);
            var result = crawler.Craw();
            Assert.IsNotNull(result);
        }
    }
}
