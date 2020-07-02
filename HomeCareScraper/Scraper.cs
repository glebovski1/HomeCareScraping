using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeCareScraper.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace HomeCareScraper
{
    public class Scraper
    {
        IWebDriver webDriver;
        public string DefaultState { get; set; } = "Washington";
        public string CareType { get; set; } = "Home Care";
        public Scraper()
        {
            webDriver = new ChromeDriver();
            GoToCaringCom();
        }

        private void GoToCaringCom()
        {
            webDriver.Navigate().GoToUrl("https://www.caring.com/");
        }
        private void GoToState(string state)
        {
            var typeElement = webDriver.FindElement(By.CssSelector("#type"));
            var elements = typeElement.FindElements(By.CssSelector("#type > option"));

            foreach (var element in elements)
            {
                if (element.Text == CareType)
                {
                    element.Click();
                    break;
                }
            }
            var citySearchBar = webDriver.FindElement(By.CssSelector("#search > form > input.form-control"));
            citySearchBar.SendKeys(state);
            citySearchBar.Submit();
        }
        public void Close()
        {
            webDriver.Close();
        }
        public ScrapedCommunities ScrapCommunities(string state)
        {
            
            GoToCaringCom();
            GoToState(state);

            ScrapedCommunities scrapedCommunities = new ScrapedCommunities();
            scrapedCommunities.CareType = this.CareType;
            scrapedCommunities.State = state;
            scrapedCommunities.CommunitiesByCity = new Dictionary<string, int>();
            var cityList = webDriver.FindElement(By.Id("cities"));
            var content = cityList.FindElements(By.ClassName("lrtr-list-item"));
            foreach (var item in content)
            {
                string cityName = item.FindElement(By.ClassName("text-subtitle1")).Text;
                int communitiesNumber = Convert.ToInt32(item.FindElement(By.ClassName("text-subtitle2")).Text);
                scrapedCommunities.CommunitiesByCity.Add(cityName, communitiesNumber);
            }

            scrapedCommunities.CommunitiesByCounty = new Dictionary<string, int>();
            var countyList = webDriver.FindElement(By.Id("counties"));
            content = countyList.FindElements(By.ClassName("lrtr-list-item"));
            foreach (var item in content)
            {
                string countyName = item.FindElement(By.ClassName("text-subtitle1")).Text;
                int communitiesNumber = Convert.ToInt32(item.FindElement(By.ClassName("text-subtitle2")).Text);
                scrapedCommunities.CommunitiesByCounty.Add(countyName, communitiesNumber);

            }
            return scrapedCommunities;
        }

        public List<ScrapedCompany> ScrapCompanies(string state)
        {
            GoToCaringCom();
            GoToState(state);
            var content = webDriver.FindElements(By.ClassName("lrtr-list-item")).ToList();
            List<ScrapedCompany> scrapedCompanies = new List<ScrapedCompany>();
            IWebDriver tempWebDriver = new ChromeDriver();
            foreach (var item in content)
            {
                string url = item.FindElement(By.TagName("a")).GetAttribute("href");
                string cityOrCountyName = item.FindElement(By.ClassName("text-subtitle1")).Text;
                //IWebDriver tempWebDriver = new ChromeDriver();
                tempWebDriver.Navigate().GoToUrl(url);
                var companiesElements = tempWebDriver.FindElements(By.ClassName("search-result"));
                foreach (var companyElement in companiesElements)
                {
                    ScrapedCompany scrapedCompany = new ScrapedCompany();
                    scrapedCompany.State = state;
                    scrapedCompany.CityOrCounty = cityOrCountyName;
                    scrapedCompany.ServiceProvides = companyElement.FindElement(By.ClassName("provides")).Text;
                    scrapedCompany.Name = companyElement.FindElement(By.TagName("h3")).Text;

                    char[] digitChars = companyElement.FindElement(By.ClassName("hidden-xs")).Text.Where(chr => Char.IsDigit(chr)).ToArray();
                    string digitstrings = new String(digitChars);
                    if (!String.IsNullOrEmpty(digitstrings))
                    {
                        scrapedCompany.ReviewsNumber = int.Parse(digitstrings);
                    }
                    else
                    {
                        scrapedCompany.ReviewsNumber = 0;
                    }
                    scrapedCompany.Description = companyElement.FindElement(By.ClassName("description")).Text;
                    try
                    {
                        var starsNumber = companyElement.FindElement(By.ClassName("reviews")).FindElement(By.TagName("input")).GetProperty("value");
                        scrapedCompany.StarsNumber = Convert.ToInt32(Math.Floor(double.Parse(starsNumber)));
                    }
                    catch(Exception ex)
                    {

                    }
                    
                    scrapedCompanies.Add(scrapedCompany);


                }
                
            }
            tempWebDriver.Close();
            return scrapedCompanies;
        }


    }
}
