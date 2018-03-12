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
        [ExpectedException(typeof(Exception))]
        public void ShouldThrowException_InValidUri()
        {
            var resut =  _service.Run(new Uri("http://extregegfample.com/"));
            foreach (var item in resut) { }
        }
    }
}
