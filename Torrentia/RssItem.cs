using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torrentia
{
    public class RssItem
    {
        public string Release { get; set; }
        public string Link { get; set; }

        public RssItem(string release, string link)
        {
            Release = release;
            Link = link;
        }
    }
}
