using HomeCareScraper;
using HomeCareScraper.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Excel;
using Ganss.Excel;

namespace ConsoleForTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            ScrapedCommunities communities=null;
            List<ScrapedCompany> companies=null;
            Scraper scraper = new Scraper();
            Console.WriteLine("Write name of state");
            string stateName = Console.ReadLine();
            try
            {
                communities = scraper.ScrapCommunities(stateName);
                companies = scraper.ScrapCompanies(stateName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            string date = DateTime.UtcNow.ToLongDateString();
            Guid guid = Guid.NewGuid();
            string path = "Scraped_data_" + date + "_" + guid.ToString();
            Directory.CreateDirectory(path);
            var root = Directory.GetCurrentDirectory() +"\\" + path; 

            ExcelHelper excel = new ExcelHelper();
            try
            {
                excel.LoadScrapedCommunities(root, communities);
                excel.LoadScrapedCommpanies(root, companies);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            scraper.Close();
            
        }
    }
}
