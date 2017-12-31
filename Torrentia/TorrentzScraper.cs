using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Torrentia
{
    public class TorrentzScraper
    {
        string _BaseUrl = "https://torrentz2.eu";
        string _SearchUrl = "https://torrentz2.eu/search?f=";
        string _VerifiedUrl = "https://torrentz2.eu/verified?f=";
        string _AdultFilterEnabledPostFix = "&safe=1";
        string _AdultFilterDisabledPostFix = "&safe=0";
        char _VerifiedChar = '✓';
        string _MagnetPrefix = "magnet";
        string _Term = string.Empty;
        string _Response = string.Empty;
        WebClient _Client;
        List<string> _Results;
        List<string> _TorrentDetails;
        List<SearchResult> _SearchResults;
        HtmlAgilityPack.HtmlDocument _Document = new HtmlAgilityPack.HtmlDocument();
        SearchResult SearchResult;
        List<string> torrentDates;
        List<string> torrentLinks;
        //List<string> trackerLinks;
        string _TorrentTitle = string.Empty;
        string _TorrentContents = string.Empty;
        string _TorrentSize = string.Empty;
        string _TorrentSeeders = string.Empty;
        string _TorrentLeechers = string.Empty;
        string _TorrentVerified = string.Empty;
        string _Url = string.Empty;
        string _MagnetUri = string.Empty;

        public bool AdultFilter { get; set; }
        public bool VerifiedOnly { get; set; }

        public TorrentzScraper()
        {
            _Client = new WebClient();
            _Client.Encoding = Encoding.UTF8;
            AdultFilter = true;
            VerifiedOnly = false;
        }

        public TorrentzScraper(string term)
        {
            _Term = term;
            _Client = new WebClient();
            _Client.Encoding = Encoding.UTF8;
            AdultFilter = true;
            VerifiedOnly = false;
        }

        public List<SearchResult> Search(string term = null)
        {
            _SearchResults = new List<SearchResult>();
            _Results = new List<string>();
            _TorrentDetails = new List<string>();

            if (!string.IsNullOrEmpty(term))
            {
                if (VerifiedOnly && AdultFilter) { _Response = _Client.DownloadString(_VerifiedUrl + term + _AdultFilterEnabledPostFix); }
                if (VerifiedOnly && !AdultFilter) { _Response = _Client.DownloadString(_VerifiedUrl + term + _AdultFilterDisabledPostFix); }
                if (!VerifiedOnly && AdultFilter) { _Response = _Client.DownloadString(_SearchUrl + term + _AdultFilterEnabledPostFix); }
                if (!VerifiedOnly && !AdultFilter) { _Response = _Client.DownloadString(_SearchUrl + term + _AdultFilterDisabledPostFix); }
            }
            else
            {
                if (VerifiedOnly && AdultFilter) { _Response = _Client.DownloadString(_VerifiedUrl + _Term + _AdultFilterEnabledPostFix); }
                if (VerifiedOnly && !AdultFilter) { _Response = _Client.DownloadString(_VerifiedUrl + _Term + _AdultFilterDisabledPostFix); }
                if (!VerifiedOnly && AdultFilter) { _Response = _Client.DownloadString(_SearchUrl + _Term + _AdultFilterEnabledPostFix); }
                if (!VerifiedOnly && !AdultFilter) { _Response = _Client.DownloadString(_SearchUrl + _Term + _AdultFilterDisabledPostFix); }
            }

            _Document.LoadHtml(_Response);

            RemoveAds(); 

            var links = _Document.DocumentNode.SelectNodes("//a[@href]");
            var ddNodes = _Document.DocumentNode.SelectNodes("//dd");

            if (links != null)
            {
                foreach (var link in links)
                {
                    _Results.Add(link.GetAttributeValue("href", string.Empty));
                }
            }
            if (ddNodes != null)
            {
                foreach (var dd in ddNodes)
                {
                    _TorrentDetails.Add(SanitizeTorrentDetails(dd.InnerHtml));
                }
            }

            FilterResults(ref _Results);

            int counter = 0;
            foreach (string s in _Results)
            {
                _SearchResults.Add(GetSearchResult(s, _TorrentDetails[counter++]));
            }

            return _SearchResults;
        }

        private SearchResult GetSearchResult(string hash, string details)
        {
            string[] tmpArray = details.Split('|');

            SearchResult = new SearchResult();
            SearchResult.Trackers = new List<string>();
            torrentDates = new List<string>();
            torrentLinks = new List<string>();
            //trackerLinks = new List<string>();
            _TorrentTitle = string.Empty;
            _TorrentContents = string.Empty;
            _TorrentSize = string.Empty;
            _TorrentSeeders = string.Empty;
            _TorrentLeechers = string.Empty;

            SearchResult.InfoHash = hash.Replace("/", string.Empty);

            if (tmpArray[0] == _VerifiedChar.ToString())
            {
                _TorrentVerified = string.Format("[{0}]", _VerifiedChar);
            }
            else
            {
                _TorrentVerified = "[-]";
            }
            _TorrentSize = tmpArray[2];
            _TorrentSeeders = tmpArray[3];
            _TorrentLeechers = tmpArray[4];

            _Document = new HtmlAgilityPack.HtmlDocument();
            _Response = _Client.DownloadString(_BaseUrl + hash);
            _Document.LoadHtml(_Response);

            var downloadNode = _Document.DocumentNode.SelectSingleNode("//div[@class='downlinks']");
            var trackersNode = _Document.DocumentNode.SelectSingleNode("//div[@class='trackers']");
            var filesNode = _Document.DocumentNode.SelectSingleNode("//div[@class='files']");

            if (downloadNode != null)
            {
                var datesNode = downloadNode.SelectNodes("//span[@title]");
                var titleNode = downloadNode.SelectSingleNode("//h2");
                var dtNode = downloadNode.SelectNodes("//dt");

                if (datesNode != null)
                {
                    foreach (var d in datesNode)
                    {
                        torrentDates.Add(d.InnerHtml.Trim());
                    }
                }
                if (titleNode != null)
                {
                    string[] tmp = titleNode.InnerHtml.Split(new string[] { "</span>" }, StringSplitOptions.RemoveEmptyEntries);
                    tmp[0] = tmp[0].Replace("<span>", string.Empty);
                    _TorrentTitle = tmp[0].Trim();
                }
                if (dtNode != null)
                {
                    foreach (var dt in dtNode)
                    {
                        var linkNode = dt.SelectNodes("//a[@href]");
                        //var siteNode = dt.SelectNodes("//span[@class='u']");
                        var nameNode = dt.SelectNodes("//span[@class='n']");

                        if (linkNode != null)
                        {
                            foreach (var c in linkNode)
                            {
                                if (!torrentLinks.Contains(c.GetAttributeValue("href", string.Empty)))
                                {
                                    torrentLinks.Add(c.GetAttributeValue("href", string.Empty));
                                }
                            }
                            //FilterResults(ref torrentLinks);
                        }

                        //if (siteNode != null)
                        //{
                        //    foreach (var sn in siteNode)
                        //    {
                        //        if (!IsOnlyDigits(sn.InnerText))
                        //        {
                        //            if (!trackerLinks.Contains(sn.InnerText))
                        //            {
                        //                trackerLinks.Add(sn.InnerText);
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }

                SearchResult.Dates = torrentDates;
                SearchResult.Age = torrentDates[0];
                SearchResult.Links = torrentLinks;
                SearchResult.Title = _TorrentTitle;
                SearchResult.Dates.Remove(SearchResult.Dates.Last());
                //SearchResult.Dates.Remove(SearchResult.Dates.First());
                //SearchResult.Trackers = trackerLinks;
                SearchResult.Leechers = _TorrentLeechers;
                SearchResult.Seeders = _TorrentSeeders;
                SearchResult.Size = _TorrentSize;
                SearchResult.Verified = _TorrentVerified;
            }

            if (trackersNode != null)
            {
                var dtNodes = trackersNode.SelectNodes("//dt");

                if (dtNodes != null)
                {
                    foreach (var dt in dtNodes)
                    {
                        SearchResult.Trackers.Add(dt.InnerText.Trim());
                    }
                }
            }

            if (filesNode != null)
            {
                var liNodes = filesNode.SelectNodes("//li[@class='t']");

                if (liNodes != null)
                {
                    foreach (var li in liNodes)
                    {
                        _TorrentContents = SanitizeTorrentContents(li.InnerHtml);
                        break;
                    }
                }
            }

            SearchResult.Contents = _TorrentContents;
            return SearchResult;
        }

        public string GetMagnetUri(string url)
        {
            _Url = url;
            string tmp = string.Empty;

            if (!string.IsNullOrEmpty(_Url))
            {
                _Response = _Client.DownloadString(_Url);
            }

            _Document = new HtmlAgilityPack.HtmlDocument();
            _Document.LoadHtml(_Response);

            var magnetNode = _Document.DocumentNode.SelectNodes("//a[@href]");
            if (magnetNode != null)
            {
                foreach (var node in magnetNode)
                {
                    if (node.GetAttributeValue("href", string.Empty).StartsWith(_MagnetPrefix))
                    {
                        tmp = node.GetAttributeValue("href", string.Empty);
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(tmp))
            {
                _MagnetUri = tmp;
            }

            return _MagnetUri;
        }

        private void FilterResults(ref List<string> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].Contains("transmission") || list[i].Contains("deluge") || list[i].Contains("qbittorrent") || list[i] == "/" || list[i].StartsWith("/search") || list[i].StartsWith("/my") || list[i].StartsWith("/help") || list[i].StartsWith("/verified") || list[i].StartsWith("/feed") || list[i].StartsWith("/announcelist") || list[i].StartsWith("/report"))
                {
                    list.RemoveAt(i);
                }
            }
        }

        private bool IsOnlyDigits(string data)
        {
            int digitsCounter = 0;
            int commasCounter = 0;
            foreach (char c in data)
            {
                if (c == ',')
                {
                    commasCounter++;
                }
                if (char.IsDigit(c))
                {
                    digitsCounter++;
                }
            }
            if (digitsCounter == data.Length - commasCounter)
            {
                return true;
            }
            return false;
        }

        private void RemoveAds()
        {
            string[] nodesToRemove = { "//div[@class='HalfRRRAcceptableAds']", "//div[@class='AcceptableTextAds']", "//div[@class='SimpleAcceptableTextAds']", "//div[@class='SemiAcceptableAds']", "//div[@class='HalfAcceptableAds']" };

            IEnumerable<HtmlAgilityPack.HtmlNode> nodesFound;

            foreach (string x in nodesToRemove)
            {
                try
                {
                    nodesFound = _Document.DocumentNode.SelectNodes(x).ToList();
                    foreach (var y in nodesFound)
                    {
                        y.Remove();
                    }
                }
                catch { }
            }
        }

        private string SanitizeTorrentContents(string data)
        {
            return data = data.Replace("<ul>", string.Empty).Replace("</ul>", string.Empty).Replace("<li>", string.Empty).Replace("</li>", string.Empty).Replace("<span>", " ").Replace("</span>", string.Empty).Replace(@"<li class=""t"">", string.Empty).Replace(@"<li class=""t2"">", string.Empty).Replace(@"<ul class=""u"">", string.Empty).Replace(@"<ul class=""u2"">", string.Empty);
        }

        private string SanitizeTorrentDetails(string data)
        {
            return data = data.Replace("<span>", string.Empty).Replace("</span>", "|");
        }
    }
}
