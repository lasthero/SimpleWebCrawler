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
                var result = new WebCrawlerService().Run(uri);
                DisplayResult(result.OrderBy(a=>a.Uri.AbsoluteUri).ToList());
                sw.Stop();
                Console.WriteLine(string.Format("Time elapsed: {0} seconds", sw.Elapsed.Seconds));
            }
            else
            {
                Console.WriteLine(string.Format("Invalid URL: {0}; please enter a valid url: http(s)://<url>/", args[0]));
            }
        }

        private static void DisplayResult(IList<ParsedHtmlDocumentResult> result)
        {
            foreach (var item in result)
            {
                Console.WriteLine(string.Format("Page URL: {0}", item.Uri.AbsoluteUri));
                Console.WriteLine("\tInternal Page Links:");
                Console.WriteLine("\tInternal Static Content");
                Console.WriteLine("\tExternal Links:");
            }
        }
    }
}
