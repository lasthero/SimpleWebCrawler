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
            Uri uri2;
            Uri.TryCreate("https://wiprodigital.com/who-we-are/", UriKind.Absolute, out uri2);
            Uri uri3;
            Uri.TryCreate("https://wiprodigital.com/who-we-are", UriKind.Absolute, out uri3);

            Queue<Uri> q = new Queue<Uri>();
            q.Enqueue(uri3);
            
            var a = q.Contains(uri2);
            Assert.AreEqual(uri2, uri3);
            //Assert.AreEqual(uri2, uri3);
            //Assert.AreEqual(uri3, uri1);
        }

    }
}
