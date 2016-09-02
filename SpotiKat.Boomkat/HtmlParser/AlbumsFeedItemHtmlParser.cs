using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using SpotiKat.Abstractions.Interfaces.HtmlAgilityPack;
using SpotiKat.Boomkat.Interfaces.HtmlParser;
using SpotiKat.Entities;

//ncrunch: no coverage start

namespace SpotiKat.Boomkat.HtmlParser {
    public class AlbumsFeedItemHtmlParser : IAlbumsFeedItemHtmlParser {
        private const string PageNodesXPath = "//nav[@class='pagination']";
        private const string FeedItemNodesXPath = "//ul[@id='products']/li/a";
        private const string ArtistNodeXPath = "span[@class='release__details']/span[@class='release__artist']";
        private const string AlbumNodeXPath = "span[@class='release__details']/span[@class='release__title']";

        public IList<FeedItem> Parse(IHtmlDocument htmlDocument, out IList<string> pages) {
            pages = GetPages(htmlDocument);
            return GetFeedItems(htmlDocument);
        }

        private IList<string> GetPages(IHtmlDocument htmlDocument) {
            var pages = new List<string>();
            var pageNodes = htmlDocument.DocumentNode.SelectNodes(PageNodesXPath);
            if (pageNodes == null || pageNodes.Count == 0) {
                return pages;
            }
            if (pageNodes[0].ChildNodes == null || pageNodes[0].ChildNodes.Count == 0) {
                return pages;
            }
            foreach (var paginationNode in pageNodes[0].ChildNodes) {
                int page;
                if (int.TryParse(paginationNode.InnerText.Replace("\n", ""), out page)) {
                    pages.Add(page.ToString());
                }
                else if (paginationNode.InnerText == "&hellip;") {
                    pages.Add("...");
                }
            }
            return pages;
        }

        private IList<FeedItem> GetFeedItems(IHtmlDocument htmlDocument) {
            var feedItems = new List<FeedItem>();
            var feedItemNodes = htmlDocument.DocumentNode.SelectNodes(FeedItemNodesXPath);

            if (feedItemNodes != null) {
                feedItems.AddRange(feedItemNodes.Select(GetFeedItem).Where(feedItem => feedItem != null));
            }

            return feedItems;
        }

        private FeedItem GetFeedItem(HtmlNode feedItemNode) {
            var artist = GetArtist(feedItemNode);
            var album = GetAlbum(feedItemNode);

            if (!string.IsNullOrWhiteSpace(artist) && !string.IsNullOrWhiteSpace(album)) {
                return new FeedItem {
                    Artist = HttpUtility.HtmlDecode(artist).ToLower().Trim(),
                    Album = HttpUtility.HtmlDecode(album).ToLower().Trim()
                };
            }

            return null;
        }

        private string GetArtist(HtmlNode feedItemNode) {
            return HttpUtility.HtmlDecode(feedItemNode.SelectSingleNode(ArtistNodeXPath).InnerText)
                .Replace("&apos;", "'");
        }

        private string GetAlbum(HtmlNode feedItemNode) {
            var nodes = feedItemNode.SelectNodes(AlbumNodeXPath);
            if (nodes != null && nodes.Count > 0) {
                var album = HttpUtility.HtmlDecode(nodes[0].InnerText).Replace("&apos;", "'");
                var indexOfStartComment = album.IndexOf("<!--", StringComparison.InvariantCultureIgnoreCase);
                if (indexOfStartComment > -1) {
                    album = album.Substring(0, indexOfStartComment);
                }

                return album.Replace("\n", "");
            }
            return "";
        }
    }
}

//ncrunch: no coverage end