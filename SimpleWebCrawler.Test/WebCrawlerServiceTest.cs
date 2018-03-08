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
        public void ShouldReturnTrue_ValidUrl()
        {
            var validUrl = "https://wiprodigital.com/";
            var result = _service.Run(validUrl);
            
            Assert.IsTrue(result, "Valid urls should return true");
        }

        [TestMethod]
        public void ShouldReturnFalse_InvalidUrl()
        {
            var invalidUrl = "abc";
            var result = _service.Run(invalidUrl);

            Assert.IsFalse(result, "Invalid urls should return false");
        }
    }
}
