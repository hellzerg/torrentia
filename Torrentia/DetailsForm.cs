using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Torrentia
{
    public partial class DetailsForm : Form
    {
        SearchResult _result;
        TorrentzScraper _scraper = new TorrentzScraper();
        string _magnet = string.Empty;

        public DetailsForm(SearchResult sr)
        {
            InitializeComponent();
            Options.ApplyTheme(this);

            _result = sr;
        }

        private void OpenTorrentLink()
        {
            if (listResults.SelectedIndices.Count == 1)
            {
                try
                {
                    Process.Start(_result.Links[listResults.SelectedIndices[0]]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Torrentia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void CopyMagnetURI()
        {
            if (listResults.SelectedIndices.Count == 1)
            {
                try
                {
                    _magnet = _scraper.GetMagnetUri(_result.Links[listResults.SelectedIndices[0]]);
                    Clipboard.SetText(_magnet);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    _magnet = string.Empty;
                }
            }
        }

        private void DetailsForm_Load(object sender, EventArgs e)
        {
            lblTorrent.Text = _result.Verified + " " + _result.Title;
            txtContents.Text = _result.Contents.Trim();

            int counter = 0;
            foreach (string x in _result.Trackers)
            {
                ListViewItem item = new ListViewItem(x);
                
                if (counter <= _result.Dates.Count - 1)
                {
                    item.SubItems.Add(_result.Dates[counter++]);
                }

                listResults.Items.Add(item);
            }

            listResults.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listResults.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void listResults_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listResults.SelectedIndices.Count == 1)
            {
                LaunchTorrentClient();
            }
        }

        private void LaunchTorrentClient()
        {
            try
            {
                _magnet = _scraper.GetMagnetUri(_result.Links[listResults.SelectedIndices[0]]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Torrentia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _magnet = string.Empty;
            }

            if (!string.IsNullOrEmpty(_magnet))
            {
                try
                {
                    Process.Start(_magnet);
                }
                catch { }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenTorrentLink();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CopyMagnetURI();
        }
    }
}
