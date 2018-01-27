using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Torrentia
{
    public class TorrentzScraper
    {
        string _baseUrl = "https://torrentz2.eu";
        string _searchUrl = "https://torrentz2.eu/search?f=";
        string _verifiedUrl = "https://torrentz2.eu/verified?f=";
        string _adultFilterEnabledPostFix = "&safe=1";
        string _adultFilterDisabledPostFix = "&safe=0";
        char _verifiedChar = '✓';
        string _magnetPrefix = "magnet";
        string _term = string.Empty;
        string _response = string.Empty;
        WebClient _client;
        List<string> _results;
        List<string> _torrentDetails;
        List<TorrentzResult> _searchResults;
        HtmlAgilityPack.HtmlDocument _document = new HtmlAgilityPack.HtmlDocument();
        TorrentzResult _searchResult;
        List<string> _torrentDates;
        List<string> _torrentLinks;
        //List<string> _trackerLinks;
        string _torrentTitle = string.Empty;
        string _torrentContents = string.Empty;
        string _torrentSize = string.Empty;
        string _torrentSeeders = string.Empty;
        string _torrentLeechers = string.Empty;
        string _torrentVerified = string.Empty;
        string _url = string.Empty;
        string _magnetUri = string.Empty;

        public bool AdultFilter { get; set; }
        public bool VerifiedOnly { get; set; }

        public TorrentzScraper()
        {
            _client = new WebClient();
            _client.Encoding = Encoding.UTF8;
            AdultFilter = true;
            VerifiedOnly = false;
        }

        public TorrentzScraper(string term)
        {
            _term = term;
            _client = new WebClient();
            _client.Encoding = Encoding.UTF8;
            AdultFilter = true;
            VerifiedOnly = false;
        }

        public List<TorrentzResult> Search(string term = null)
        {
            _searchResults = new List<TorrentzResult>();
            _results = new List<string>();
            _torrentDetails = new List<string>();

            if (!string.IsNullOrEmpty(term))
            {
                if (VerifiedOnly && AdultFilter) { _response = _client.DownloadString(_verifiedUrl + term + _adultFilterEnabledPostFix); }
                if (VerifiedOnly && !AdultFilter) { _response = _client.DownloadString(_verifiedUrl + term + _adultFilterDisabledPostFix); }
                if (!VerifiedOnly && AdultFilter) { _response = _client.DownloadString(_searchUrl + term + _adultFilterEnabledPostFix); }
                if (!VerifiedOnly && !AdultFilter) { _response = _client.DownloadString(_searchUrl + term + _adultFilterDisabledPostFix); }
            }
            else
            {
                if (VerifiedOnly && AdultFilter) { _response = _client.DownloadString(_verifiedUrl + _term + _adultFilterEnabledPostFix); }
                if (VerifiedOnly && !AdultFilter) { _response = _client.DownloadString(_verifiedUrl + _term + _adultFilterDisabledPostFix); }
                if (!VerifiedOnly && AdultFilter) { _response = _client.DownloadString(_searchUrl + _term + _adultFilterEnabledPostFix); }
                if (!VerifiedOnly && !AdultFilter) { _response = _client.DownloadString(_searchUrl + _term + _adultFilterDisabledPostFix); }
            }

            _document.LoadHtml(_response);

            RemoveAds(); 

            var links = _document.DocumentNode.SelectNodes("//a[@href]");
            var ddNodes = _document.DocumentNode.SelectNodes("//dd");

            if (links != null)
            {
                foreach (var link in links)
                {
                    _results.Add(link.GetAttributeValue("href", string.Empty));
                }
            }
            if (ddNodes != null)
            {
                foreach (var dd in ddNodes)
                {
                    _torrentDetails.Add(SanitizeTorrentDetails(dd.InnerHtml));
                }
            }

            FilterResults(ref _results);

            int counter = 0;
            foreach (string s in _results)
            {
                _searchResults.Add(GetSearchResult(s, _torrentDetails[counter++]));
            }

            return _searchResults;
        }

        private TorrentzResult GetSearchResult(string hash, string details)
        {
            string[] tmpArray = details.Split('|');

            _searchResult = new TorrentzResult();
            _searchResult.Trackers = new List<string>();
            _torrentDates = new List<string>();
            _torrentLinks = new List<string>();
            //trackerLinks = new List<string>();
            _torrentTitle = string.Empty;
            _torrentContents = string.Empty;
            _torrentSize = string.Empty;
            _torrentSeeders = string.Empty;
            _torrentLeechers = string.Empty;

            _searchResult.InfoHash = hash.Replace("/", string.Empty);

            if (tmpArray[0] == _verifiedChar.ToString())
            {
                _torrentVerified = string.Format("[{0}]", _verifiedChar);
            }
            else
            {
                _torrentVerified = "[-]";
            }
            _torrentSize = tmpArray[2];
            _torrentSeeders = tmpArray[3];
            _torrentLeechers = tmpArray[4];

            _document = new HtmlAgilityPack.HtmlDocument();
            _response = _client.DownloadString(_baseUrl + hash);
            _document.LoadHtml(_response);

            var downloadNode = _document.DocumentNode.SelectSingleNode("//div[@class='downlinks']");
            var trackersNode = _document.DocumentNode.SelectSingleNode("//div[@class='trackers']");
            var filesNode = _document.DocumentNode.SelectSingleNode("//div[@class='files']");

            if (downloadNode != null)
            {
                var datesNode = downloadNode.SelectNodes("//span[@title]");
                var titleNode = downloadNode.SelectSingleNode("//h2");
                var dtNode = downloadNode.SelectNodes("//dt");

                if (datesNode != null)
                {
                    foreach (var d in datesNode)
                    {
                        _torrentDates.Add(d.InnerHtml.Trim());
                    }
                }
                if (titleNode != null)
                {
                    string[] tmp = titleNode.InnerHtml.Split(new string[] { "</span>" }, StringSplitOptions.RemoveEmptyEntries);
                    tmp[0] = tmp[0].Replace("<span>", string.Empty);
                    _torrentTitle = tmp[0].Trim();
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
                                if (!_torrentLinks.Contains(c.GetAttributeValue("href", string.Empty)))
                                {
                                    _torrentLinks.Add(c.GetAttributeValue("href", string.Empty));
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

                _searchResult.Dates = _torrentDates;
                _searchResult.Age = _torrentDates[0];
                _searchResult.Links = _torrentLinks;
                _searchResult.Title = _torrentTitle;
                _searchResult.Dates.Remove(_searchResult.Dates.Last());
                //SearchResult.Dates.Remove(SearchResult.Dates.First());
                //SearchResult.Trackers = trackerLinks;
                _searchResult.Leechers = _torrentLeechers;
                _searchResult.Seeders = _torrentSeeders;
                _searchResult.Size = _torrentSize;
                _searchResult.Verified = _torrentVerified;
            }

            if (trackersNode != null)
            {
                var dtNodes = trackersNode.SelectNodes("//dt");

                if (dtNodes != null)
                {
                    foreach (var dt in dtNodes)
                    {
                        if (dt.InnerText.Contains("://")) _searchResult.Trackers.Add(dt.InnerText.Trim());
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
                        _torrentContents = SanitizeTorrentContents(li.InnerHtml);
                        break;
                    }
                }
            }

            _searchResult.Contents = _torrentContents;
            return _searchResult;
        }

        public string GetMagnetUri(string url)
        {
            _url = url;
            string tmp = string.Empty;

            if (!string.IsNullOrEmpty(_url))
            {
                _response = _client.DownloadString(_url);
            }

            _document = new HtmlAgilityPack.HtmlDocument();
            _document.LoadHtml(_response);

            var magnetNode = _document.DocumentNode.SelectNodes("//a[@href]");
            if (magnetNode != null)
            {
                foreach (var node in magnetNode)
                {
                    if (node.GetAttributeValue("href", string.Empty).StartsWith(_magnetPrefix))
                    {
                        tmp = node.GetAttributeValue("href", string.Empty);
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(tmp))
            {
                _magnetUri = tmp;
            }

            return _magnetUri;
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
            string[] nodesToRemove = { "//div[@class='HalfRRRAcceptableAds']", "//div[@class='AcceptableTextAds']", "//div[@class='SimpleAcceptableTextAds']", "//div[@class='SemiAcceptableAds']", "//div[@class='HalfAcceptableAds']", "//div[@class='ads_box adskeeperWrap']", "//p[@class='generic']", "//p[@class='generi']", "//p[@class='gener']", "//p[@class='gene']", "//p[@class='gen']" };

            IEnumerable<HtmlAgilityPack.HtmlNode> nodesFound;

            foreach (string x in nodesToRemove)
            {
                try
                {
                    nodesFound = _document.DocumentNode.SelectNodes(x).ToList();
                    foreach (var y in nodesFound)
                    {
                        y.Remove();
                    }
                }
                catch { continue; }
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
