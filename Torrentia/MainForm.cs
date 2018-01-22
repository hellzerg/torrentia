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
using System.Net;
using System.Net.NetworkInformation;

namespace Torrentia
{
    public partial class MainForm : Form
    {
        readonly string _latestVersionLink = "https://raw.githubusercontent.com/hellzerg/torrentia/master/version.txt";
        readonly string _releasesLink = "https://github.com/hellzerg/torrentia/releases";

        readonly string _noNewVersionMessage = "You already have the latest version!";
        readonly string _betaVersionMessage = "You are using an experimental version!";

        TorrentzScraper _scraper;
        List<TorrentzResult> results;

        List<ListViewItem> _items;
        ListViewItem _item;

        string _searchingStatus = "Searching ...";
        string _noResultsStatus = "No results";

        ListViewColumnSorter columnSorter;

        public MainForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
            Options.ApplyTheme(this);

            columnSorter = new ListViewColumnSorter();
            listResults.ListViewItemSorter = columnSorter;

            _scraper = new TorrentzScraper();
            _scraper.AdultFilter = Options.CurrentOptions.AdultFilter;
            _scraper.VerifiedOnly = Options.CurrentOptions.VerifiedOnly;

            checkBox1.Checked = Options.CurrentOptions.VerifiedOnly;
            checkBox2.Checked = Options.CurrentOptions.AdultFilter;

            if (!IsInternetAvailable())
            {
                MessageBox.Show("No internet connection were found!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private bool IsInternetAvailable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }

        private string NewVersionMessage(string latest)
        {
            return string.Format("There is a new version available!\n\nLatest version: {0}\nCurrent version: {1}\n\nDo you want to download it now?", latest, Program.GetCurrentVersionToString());
        }

        private void CheckForUpdate()
        {
            WebClient client = new WebClient
            {
                Encoding = Encoding.UTF8
            };

            string latestVersion = string.Empty;
            try
            {
                latestVersion = client.DownloadString(_latestVersionLink);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (!string.IsNullOrEmpty(latestVersion))
            {
                if (float.Parse(latestVersion) > Program.GetCurrentVersion())
                {
                    if (MessageBox.Show(NewVersionMessage(latestVersion), "Update available", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            Process.Start(_releasesLink);
                        }
                        catch { }
                    }
                }
                else if (float.Parse(latestVersion) == Program.GetCurrentVersion())
                {
                    MessageBox.Show(_noNewVersionMessage, "No update available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(_betaVersionMessage, "No update available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void RestoreWindowState()
        {
            this.WindowState = Options.CurrentOptions.WindowState;
            this.Size = Options.CurrentOptions.WindowSize;

            if (Options.CurrentOptions.WindowLocation != null)
            {
                this.Location = (Point)Options.CurrentOptions.WindowLocation;
            }
            else
            {
                this.CenterToScreen();
            }
        }

        private void SaveWindowState()
        {
            Options.CurrentOptions.WindowState = this.WindowState;
           
            if (this.WindowState == FormWindowState.Normal)
            {
                Options.CurrentOptions.WindowLocation = this.Location;
                Options.CurrentOptions.WindowSize = this.Size;
            }
            else
            {
                Options.CurrentOptions.WindowLocation = this.RestoreBounds.Location;
                Options.CurrentOptions.WindowSize = this.RestoreBounds.Size;
            }
        }

        private void StartSearching()
        {
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                lblStatus.Text = _searchingStatus;
                lblStatus.Visible = true;
                txtSearch.Enabled = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;

                Task.Factory.StartNew(() => Search());
            }
            else
            {
                listResults.Items.Clear();
                lblStatus.Text = "Enter keyword and press 'Search'";
            }
        }

        private void Search()
        {
            listResults.Items.Clear();

            try
            {
                results = _scraper.Search(txtSearch.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Torrentia", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (results != null)
            {
                _items = new List<ListViewItem>();
                foreach (TorrentzResult x in results)
                {
                    _item = new ListViewItem(x.Verified);
                    _item.SubItems.Add(x.Title);
                    _item.SubItems.Add(x.Size);
                    _item.SubItems.Add(x.Seeders);
                    _item.SubItems.Add(x.Leechers);
                    _item.SubItems.Add(x.Age);

                    _items.Add(_item);
                }

                listResults.Items.AddRange(_items.ToArray());
            }

            if (listResults.Items.Count > 0)
            {
                lblStatus.Text = string.Format("{0} torrents found", results.Count);
                lblStatus.Visible = true;

                listResults.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listResults.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            else
            {
                lblStatus.Text = _noResultsStatus;
                lblStatus.Visible = true;
            }

            txtSearch.Enabled = true;
            checkBox1.Enabled = true;
            checkBox2.Enabled = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RestoreWindowState();

            this.Text += Program.GetCurrentVersionToString();
            txtSearch.Focus();
        }

        private void listResults_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listResults.SelectedItems.Count == 1)
            {
                int i = results.FindIndex(x => listResults.SelectedItems[0].SubItems[1].Text.Contains(x.Title));

                DetailsForm f = new DetailsForm(results[i]);
                f.ShowDialog();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            _scraper.AdultFilter = checkBox2.Checked;
            Options.CurrentOptions.AdultFilter = checkBox2.Checked;
            StartSearching();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _scraper.VerifiedOnly = checkBox1.Checked;
            Options.CurrentOptions.VerifiedOnly = checkBox1.Checked;
            StartSearching();
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            OptionsForm f = new OptionsForm(this);
            f.ShowDialog(this);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveWindowState();
            Options.SaveSettings();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutForm f = new AboutForm();
            f.ShowDialog(this);
        }

        private void listResults_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == columnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (columnSorter.Order == SortOrder.Ascending)
                {
                    columnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    columnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                columnSorter.SortColumn = e.Column;
                columnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            listResults.Sort();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            CheckForUpdate();
        }

        private void btnFeed_Click(object sender, EventArgs e)
        {
            RssForm f = new RssForm();
            f.Show();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                StartSearching();
                txtSearch.Focus();
            }
        }
    }
}
