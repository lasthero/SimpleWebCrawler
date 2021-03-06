﻿using System;
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
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Please pass a valid url as an argument: http(s)://<url>/");
            }
            else
            {
                if (Uri.TryCreate(args[0], UriKind.Absolute, out uri))
                {
                    try
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        //setup for xml serialization
                        var rootPath = GetRootPath();
                        if (!Directory.Exists(Path.Combine(rootPath, "sitemaps")))
                        {
                            Directory.CreateDirectory(Path.Combine(rootPath, "sitemaps"));
                        }
                        var fileName = Path.Combine(rootPath, "sitemaps",
                            string.Format("{0}_{1:yyyy-MM-dd_HHmmss}.xml", uri.Host, DateTime.Now));
                        XmlSerializer ser = new XmlSerializer(typeof(ParsedHtmlDocumentXmlSerialization));
                        var resultsXmlSerialization = new ParsedHtmlDocumentXmlSerialization();
                        TextWriter writer = new StreamWriter(fileName);

                        var counter = 0;
                        IList<string> errorMessages = new List<string>();
                        foreach (var result in new WebCrawlerService().Run(uri))
                        {
                            if (string.IsNullOrEmpty(result.ErrorMessage))
                            {
                                DisplayResult(result, ++counter);
                                resultsXmlSerialization.Pages.Add(result);
                            }
                            else
                            {
                                errorMessages.Add(result.ErrorMessage);
                            }
                        }

                        sw.Stop();
                        ser.Serialize(writer, resultsXmlSerialization);
                        writer.Close();
                        if (errorMessages.Count > 0)
                        {
                            Console.WriteLine("Errors occurred during processing:");
                            foreach (var error in errorMessages)
                                Console.WriteLine("\t{0}", error);
                        }

                        Console.WriteLine(string.Format("Found total {0} pages", counter));
                        Console.WriteLine(string.Format("Sitemap file was created at {0}", fileName));
                        Console.WriteLine(string.Format("Time elapsed: {0} seconds", sw.Elapsed.TotalSeconds));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(string.Format("Exception: {0}", e.Message));
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("Invalid URL: {0}; please enter a valid url: http(s)://<url>/",
                        args[0]));
                }
            }
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        private static void DisplayResult(ParsedHtmlDocumentResult item, int pageNumber)
        {
            Console.WriteLine("Page #{0}: {1}",pageNumber, item.Uri.AbsoluteUri);
            
            Console.WriteLine("\tInternal Static Content");
            foreach (var content in item.StaticContents)
                Console.WriteLine("\t\t{0}", content);

            Console.WriteLine("\tExternal Links:");
            foreach (var link in item.ExternalLinks)
                Console.WriteLine("\t\t{0}", link);
        }

        private static string GetRootPath()
        {
            return Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath);
        }
    }
}
