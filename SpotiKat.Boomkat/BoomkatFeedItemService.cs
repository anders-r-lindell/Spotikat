using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SpotiKat.Abstractions.HtmlAgilityPack;
using SpotiKat.Abstractions.Interfaces.HtmlAgilityPack;
using SpotiKat.Boomkat.Comparer;
using SpotiKat.Boomkat.Exceptions;
using SpotiKat.Boomkat.Interfaces;
using SpotiKat.Boomkat.Interfaces.Configuration;
using SpotiKat.Boomkat.Interfaces.HtmlParser;
using SpotiKat.Entities;
using SpotiKat.Interfaces.Logging;
using SpotiKat.Interfaces.Net.Http;

namespace SpotiKat.Boomkat {
    public class BoomkatFeedItemService : IBoomkatFeedItemService {
        private const string HtmlWebLoadErrorMessageFormat = "Failed to load html web for url '{0}': {1}";
        private const string HtmlParserParseErrorMessageFormat = "Failed to parse html document for url '{0}': {1}";
        private readonly IAlbumsFeedItemHtmlParser _albumsFeedItemHtmlParser;
        private readonly IBoomkatConfiguration _boomkatConfiguration;
        private readonly ILastAlbumsFeedItemHtmlParser _lastAlbumsFeedItemHtmlParser;
        private readonly ILogFactory _logFactory;
        private readonly IUrlBuilder _urlBuilder;
        private readonly IWebClient _webClient;

        public BoomkatFeedItemService(ILastAlbumsFeedItemHtmlParser lastAlbumsFeedItemHtmlParser,
            IAlbumsFeedItemHtmlParser albumsFeedItemHtmlParser, IUrlBuilder urlBuilder, ILogFactory logFactory,
            IWebClient webClient, IBoomkatConfiguration boomkatConfiguration) {
            _lastAlbumsFeedItemHtmlParser = lastAlbumsFeedItemHtmlParser;
            _albumsFeedItemHtmlParser = albumsFeedItemHtmlParser;
            _urlBuilder = urlBuilder;
            _boomkatConfiguration = boomkatConfiguration;
            _logFactory = logFactory;
            _webClient = webClient;
        }

        public async Task<FeedItemsResult> GetFeedItemsAsync(int page) {
            var url = _urlBuilder.BuildFeedItemUrl(page);
            var htmlDocument = await GetHtmlDocumentAsync(url);
            return GetFeedItemsResult(htmlDocument, null, url);
        }

        public async Task<FeedItemsResult> GetFeedItemsByGenreAsync(string genre, int page) {
            var url = _urlBuilder.BuildFeedItemByGenreUrl(genre, page);
            var htmlDocument = await GetHtmlDocumentAsync(url);
            return GetFeedItemsResult(htmlDocument, genre, url);
        }

        private async Task<IHtmlDocument> GetHtmlDocumentAsync(string url) {
            try {
                _webClient.UserAgent = _boomkatConfiguration.WebClientUserAgent;

                var htmlContent = await _webClient.GetAsync(url);

                var htmlDocument = new HtmlDocumentImpl(new HtmlDocument());
                htmlDocument.LoadHtml(htmlContent);
                return htmlDocument;
            }
            catch (Exception ex) {
                _logFactory.GetLogger(typeof (BoomkatFeedItemService))
                    .ErrorFormat(HtmlWebLoadErrorMessageFormat, url, ex.Message);
                throw new BoomkatServiceException(string.Format(HtmlWebLoadErrorMessageFormat, url, ex.Message));
            }
        }

        private FeedItemsResult GetFeedItemsResult(IHtmlDocument htmlDocument, string genre, string url) {
            try {
                IList<string> pages;
                var feedItems = (genre != null && genre != "0")
                    ? _albumsFeedItemHtmlParser.Parse(htmlDocument, out pages).Distinct(new FeedItemComparer()).ToList()
                    : _lastAlbumsFeedItemHtmlParser.Parse(htmlDocument, out pages)
                        .Distinct(new FeedItemComparer())
                        .ToList();
                return new FeedItemsResult {Items = feedItems, Pages = pages};
            }
            catch (Exception ex) {
                _logFactory.GetLogger(typeof (BoomkatFeedItemService))
                    .ErrorFormat(HtmlParserParseErrorMessageFormat, url, ex.Message);
                throw new BoomkatServiceException(string.Format(HtmlParserParseErrorMessageFormat, url, ex.Message));
            }
        }
    }
}