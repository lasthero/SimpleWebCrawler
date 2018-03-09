using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleWebCrawler.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWebCrawler.Test
{
    [TestClass]
    public class WebCrawlerServiceTest
    {
        private readonly WebCrawlerService _service;

        public WebCrawlerServiceTest()
        {
            _service = new WebCrawlerService();
        }

        [TestMethod]
        public void ShouldReturnResult_ValidUrl()
        {
            var validUrl = "https://wiprodigital.com/";
            var result = _service.Run(new Uri(validUrl));
            
            Assert.IsTrue(result!=null, "Valid urls should return true");
        }

        [TestMethod]
        public void ShouldReturnNull_InvalidUrl()
        {
            var result = _service.Run(null);

            Assert.IsFalse(result!=null, "Invalid urls should return false");
        }
    }
}
