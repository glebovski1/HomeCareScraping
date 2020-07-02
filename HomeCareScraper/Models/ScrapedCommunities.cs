using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HomeCareScraper.Models
{
    public class ScrapedCommunities
    {
        public string CareType { get; set; }
        public string State { get; set; }
        public Dictionary<string, int> CommunitiesByCity { get; set; }
        public Dictionary<string, int> CommunitiesByCounty { get; set; }
        public ScrapedCommunities()
        {

        }


    }
}
