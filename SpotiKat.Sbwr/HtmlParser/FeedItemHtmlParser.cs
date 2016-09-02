using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using SpotiKat.Abstractions.Interfaces.HtmlAgilityPack;
using SpotiKat.Entities;
using SpotiKat.Sbwr.Interfaces.HtmlParser;

//ncrunch: no coverage start

namespace SpotiKat.Sbwr.HtmlParser {
    public class FeedItemHtmlParser : IFeedItemHtmlParser {
        private const string FeedItemNodesXPath = "//div[@id='main']/article";
        private const string ArtistAlbumNodeXPath = "div[@class='cb-meta']/h2/a";
        private const string ArtistAlbumSeparator = " – ";
        private const string AlbumStartCharacter = "‘";
        private const string AlbumEndCharacter = "’";

        public IList<FeedItem> Parse(IHtmlDocument htmlDocument) {
            return GetFeedItems(htmlDocument);
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
            var artistAlbumNode = feedItemNode.SelectSingleNode(ArtistAlbumNodeXPath);

            if (artistAlbumNode == null) {
                return null;
            }

            var artist = GetArtist(artistAlbumNode);
            var album = GetAlbum(artistAlbumNode);

            if (!string.IsNullOrWhiteSpace(artist) && !string.IsNullOrWhiteSpace(album)) {
                return new FeedItem {
                    Artist = artist,
                    Album = album
                };
            }

            return null;
        }

        private string GetArtist(HtmlNode artistAlbumNode) {
            var artistAlbumValue = HttpUtility.HtmlDecode(artistAlbumNode.InnerText);

            var separatorIndex = artistAlbumValue.IndexOf(ArtistAlbumSeparator, StringComparison.InvariantCulture);

            return separatorIndex == -1 ? null : artistAlbumValue.Substring(0, separatorIndex).ToLower().Trim();
        }

        private string GetAlbum(HtmlNode artistAlbumNode) {
            var artistAlbumValue = HttpUtility.HtmlDecode(artistAlbumNode.InnerText);

            var separatorIndex = artistAlbumValue.IndexOf(ArtistAlbumSeparator, StringComparison.InvariantCulture);
            if (separatorIndex == -1) {
                return null;
            }

            var result = artistAlbumValue.Substring(separatorIndex + 3, artistAlbumValue.Length - (separatorIndex + 3));

            if (result.StartsWith(AlbumStartCharacter)) {
                result = result.Substring(1);
            }

            var albumEndCharacterIndex = result.LastIndexOf(AlbumEndCharacter, StringComparison.InvariantCulture);

            if (albumEndCharacterIndex == -1) {
                return null;
            }

            return result.Substring(0, albumEndCharacterIndex).ToLower().Trim();
        }
    }
}

//ncrunch: no coverage end