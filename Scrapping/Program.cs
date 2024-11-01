using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Scrapping
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Enter the URL: ");
            string url = Console.ReadLine();

            try
            {
                string html = await FetchHtmlAsync(url);
                var hrefs = ExtractHrefs(html);
                WriteHrefsToFile(hrefs, "anchors.txt");
                Console.WriteLine("Hrefs extracted and saved to anchors.txt.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task<string> FetchHtmlAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetStringAsync(url);
            }
        }

        static string[] ExtractHrefs(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var anchors = doc.DocumentNode.SelectNodes("//a[@href]");

            if (anchors == null)
                return Array.Empty<string>();

            string[] hrefs = new string[anchors.Count];
            for (int i = 0; i < anchors.Count; i++)
            {
                hrefs[i] = anchors[i].GetAttributeValue("href", string.Empty);
            }

            return hrefs;
        }

        static string[] ExtractHrefs(string html, string hrefEndsWithFilter)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var anchors = doc.DocumentNode.SelectNodes("//a[@href]");

            if (anchors == null)
                return Array.Empty<string>();

            string[] hrefs = new string[anchors.Count];
            for (int i = 0; i < anchors.Count; i++)
            {
                hrefs[i] = anchors[i].GetAttributeValue("href", string.Empty);
            }

            return hrefs.Where(href => href.EndsWith(hrefEndsWithFilter)).ToArray();
        }

        static void WriteHrefsToFile(string[] hrefs, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (var href in hrefs)
                {
                    writer.WriteLine(href);
                }
            }
        }
    }
}
