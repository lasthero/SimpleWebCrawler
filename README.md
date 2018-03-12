# Simple Web Crawler
This is my implementation of a simple web crawler which will visit all pages within the same domain. This implementation is based on Breadth First Search algorithm.
https://en.wikipedia.org/wiki/Breadth-first_search

## Assumptions
- Pages with the same url but different query strings are considered different pages.
- We are only interested at anchor, image, css, and script tags in the pages within the same domain.

## Architecture
This console App solution was implemented with .NET Core which takes a url from command line argument then outputs results to console as it craws web pages then eventually writes all results to a XMl file under the "sitemaps" folder. 
 
 ## Prerequisites
 - The latest .NET Core SDK is required in order to build and run this application:
 [.NET Core SDK](https://www.microsoft.com/net/download/windows)

 - It is recommended to have an IDE such as VS Code or Visual Studio installed to the machine . 

 ## Build Instructions
 ### Use command line tool/terminal:
 - To run: At the solution root directory, "cd" into "SimpleWebCrawler.ConsoleApp" then:
 >dotnet run `url`

 The url argument needs to be a valid url starting with either "http://" or "https://"; otherwise an error message will show.
 - To run unit tests: under the solution root directory, "cd" into "SimpleWebCrawler.Test" then:
 >dotnet test

 ## Future Improvements 
  - I used TDD approach to get myself started, however more thorough unit tests need to be added and a mock framework with Dependency Injection can be considered as well.
  - Duplicated pages with and without trailing forward slash '/' at the end of url are considered different(e.g. 'https://wiprodigital.com/who-we-are' and '"https://wiprodigital.com/who-we-are/'); as a result both will be processed. I'd like to do more research and address this issue if I have more time. More information about this issue:
  (https://webmasters.googleblog.com/2010/04/to-slash-or-not-to-slash.html) 
  - More error handling and logging: 
  Test for if the link is alive before visiting (There's a performance concern, however). Also I'd like to write processing messages(summary, errors, exception,..) into logs for trouble shooting.
  - Thinking much further, each web site can potentially contain thousands of pages therefore performance can be slow. To future imporvements, results can be stored at a data store and a cache mechanism can be used. 