using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using SimpleWebCrawler.Services;
using SimpleWebCrawler.Services.Models;

namespace SimpleWebCrawler.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri uri;
            if (Uri.TryCreate(args[0], UriKind.Absolute, out uri))
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var fileName = Path.Combine(GetRootPath(), "sitemaps", string.Format("{0}_{1:yyyy-MM-dd_HHmmss}.xml", uri.Host, DateTime.Now));
                XmlSerializer ser = new XmlSerializer(typeof(ParsedHtmlDocumentXmlSerialization));
                var resultsXmlSerialization = new ParsedHtmlDocumentXmlSerialization();
                
                TextWriter writer = new StreamWriter(fileName);
                var counter = 0;
                foreach (var result in new WebCrawlerService().Run(uri))
                {
                    DisplayResult(result, ++counter);
                    resultsXmlSerialization.Pages.Add(result);
                }
                sw.Stop();
                ser.Serialize(writer, resultsXmlSerialization);
                writer.Close();
                Console.WriteLine(string.Format("Found total {0} pages", counter));
                Console.WriteLine(string.Format("Time elapsed: {0} seconds", sw.Elapsed.TotalSeconds));
            }
            else
            {
                Console.WriteLine(string.Format("Invalid URL: {0}; please enter a valid url: http(s)://<url>/", args[0]));
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        private static void DisplayResult(ParsedHtmlDocumentResult item, int pageNumber)
        {
            Console.WriteLine(string.Format("Page #{0}: {1}",pageNumber, item.Uri.AbsoluteUri));
            
            Console.WriteLine("\tInternal Static Content");
            foreach (var content in item.StaticContents)
                Console.WriteLine("\t\t{0}", content);

            Console.WriteLine("\tExternal Links:");
            foreach (var link in item.ExternalLinks)
                Console.WriteLine("\t\t{0}", link);
        }

        private static string GetRootPath()
        {
            var debugPath = string.Empty;
#if (DEBUG)
            debugPath = "..\\..\\..\\";
#endif
            return Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath), debugPath);
        }
    }
}
