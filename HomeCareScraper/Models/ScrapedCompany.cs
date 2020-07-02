using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCareScraper.Models
{
    public class ScrapedCompany
    {
        public string Name { get; set; }
        public string ServiceProvides { get; set; }
        public int ReviewsNumber { get; set; }
        public int StarsNumber { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string CityOrCounty { get; set; }
    }
}
