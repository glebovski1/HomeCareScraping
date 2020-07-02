using HomeCareScraper.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace HomeCareScraper
{
    public class ExcelHelper
    {
        public void LoadScrapedCommunities(string path, ScrapedCommunities communities)
        {
            byte[] fileContent;
            using (var package = new ExcelPackage())
            {
                var sheet = package.Workbook.Worksheets.Add(communities.CareType + "Of" + communities.State);

                int row = 1;
                sheet.Cells[row, 1].Value = communities.State;
                row++;
                sheet.Cells[row, 1].Value = "City/County";
                sheet.Cells[row, 2].Value = "Communities";
                row++;
                var keys = communities.CommunitiesByCity.Keys;
                foreach (var key in keys)
                {
                    sheet.Cells[row, 1].Value = key;
                    sheet.Cells[row, 2].Value = communities.CommunitiesByCity[key];
                    row++;
                }
                keys = communities.CommunitiesByCounty.Keys;
                foreach (var key in keys)
                {
                    sheet.Cells[row, 1].Value = key;
                    sheet.Cells[row, 2].Value = communities.CommunitiesByCounty[key];
                    row++;
                }
                fileContent = package.GetAsByteArray();

            }
            if (fileContent == null || fileContent.Length == 0)
            {
                throw new Exception("fileContent is null or empty");
            }
            string fileName = "CommunitiesOf" + communities.State + ".xlsx";
            using (FileStream stream = new FileStream($"{path}\\" + fileName, FileMode.Create))
            {
                stream.Write(fileContent, 0, fileContent.Length);
            }
        }
        public void LoadScrapedCommpanies(string path, List<ScrapedCompany> companies)
        {
            byte[] fileContent;
            using (var package = new ExcelPackage())
            {
                var sheet = package.Workbook.Worksheets.Add("Companies");
                int row = 1;
                sheet.Cells[row, 1].Value = "Company name";
                sheet.Cells[row, 2].Value = "Service Provides";
                sheet.Cells[row, 3].Value = "State"; 
                sheet.Cells[row, 4].Value = "City/County";
                sheet.Cells[row, 5].Value = "Reviews";
                sheet.Cells[row, 6].Value = "Stars";
                sheet.Cells[row, 7].Value = "Description";
                row++;
                foreach (var company in companies)
                {
                    sheet.Cells[row, 1].Value = company.Name; //"Company name"
                    sheet.Cells[row, 2].Value = company.ServiceProvides; //"Service Provides"
                    sheet.Cells[row, 3].Value = company.State; //"State"
                    sheet.Cells[row, 4].Value = company.CityOrCounty; //"City/County"
                    sheet.Cells[row, 5].Value = company.ReviewsNumber; //"Reviews"
                    sheet.Cells[row, 6].Value = company.StarsNumber; //"Stars"
                    sheet.Cells[row, 7].Value = company.Description; //"Description"
                    row++;
                }
                fileContent = package.GetAsByteArray();

            }
            if (fileContent == null || fileContent.Length == 0)
            {
                throw new Exception("fileContent is null or empty");
            }
            string fileName = "Companies.xlsx";
            using (FileStream stream = new FileStream($"{path}\\" + fileName, FileMode.Create))
            {
                stream.Write(fileContent, 0, fileContent.Length);
            }
        }
    }
}
