using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
            var uri = WebCrawlerUtil.GetResponseUri(new Uri(url));
            DefaultWebCrawler crawler = new DefaultWebCrawler(uri);
            var result = crawler.Craw();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestUri()
        {
            var uri1 = new Uri("http://wiprodigital.com/");
            var uri2 = new Uri("http://wiprodigital.com");
            var uri3 = new Uri("https://wiprodigital.com/");

            Assert.AreEqual(uri1, uri2);
            //Assert.AreEqual(uri2, uri3);
            //Assert.AreEqual(uri3, uri1);
        }

    }
}
