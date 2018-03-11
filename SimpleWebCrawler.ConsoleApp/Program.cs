using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                var counter = 0;
                foreach (var result in new WebCrawlerService().Run(uri))
                {
                    DisplayResult(result, ++counter);
                }
                sw.Stop();
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
            Console.WriteLine("\tInternal Page Links:");
            foreach (var link in item.InternalLinks)
                Console.WriteLine("\t\t{0}", link);

            Console.WriteLine("\tInternal Static Content");
            foreach (var content in item.StaticContents)
                Console.WriteLine("\t\t{0}", content);

            Console.WriteLine("\tExternal Links:");
            foreach (var link in item.ExternalLinks)
                Console.WriteLine("\t\t{0}", link);
        }
    }
}
