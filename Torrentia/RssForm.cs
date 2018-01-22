using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Xml.Linq;
using System.Diagnostics;

namespace Torrentia
{
    public partial class RssForm : Form
    {
        string _rssFeedLink = "http://predb.me/?rss=1";
        WebClient _client;

        ListViewItem _item;
        List<ListViewItem> _items;

        List<RssItem> _rssItems;

        public RssForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
            Options.ApplyTheme(this);

            _rssItems = new List<RssItem>();

            _client = new WebClient();
            _client.Encoding = Encoding.UTF8;
            _client.Headers["User-Agent"] = @"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";

            RetrieveRSS();

            rssTimer.Interval = 60000;
            rssTimer.Start();
        }

        private void RssForm_Load(object sender, EventArgs e)
        {
            
        }

        private void RetrieveRSS()
        {
            _client = new WebClient();
            _client.Encoding = Encoding.UTF8;
            _client.Headers["User-Agent"] = @"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";

            XDocument xml = XDocument.Parse(_client.DownloadString(_rssFeedLink));

            var feeds = from feed in xml.Descendants("item")
                        select new
                        {
                            Release = feed.Element("title").Value,
                            Link = feed.Element("link").Value
                        };

            _rssItems.Clear();
            listResults.Items.Clear();

            foreach (var x in feeds)
            {
                _rssItems.Add(new RssItem(x.Release, x.Link));

                _items = new List<ListViewItem>();

                _item = new ListViewItem(x.Release);
                _items.Add(_item);

                listResults.Items.AddRange(_items.ToArray());
            }

            listResults.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listResults.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void listResults_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                Process.Start(_rssItems[listResults.SelectedIndices[0]].Link);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RetrieveRSS();
        }

        private void rssTimer_Tick(object sender, EventArgs e)
        {
            RetrieveRSS();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;
            this.Activate();
        }
    }
}
