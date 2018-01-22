using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torrentia
{
    public class TorrentzResult
    {
        public string Title { get; set; }
        public string InfoHash { get; set; }
        public string Age { get; set; }
        public string Contents { get; set; }
        public string Verified { get; set; }
        public string Seeders { get; set; }
        public string Leechers { get; set; }
        public string Size { get; set; }
        public List<string> Dates { get; set; }
        public List<string> Links { get; set; }
        public List<string> Trackers { get; set; }
    }
}
