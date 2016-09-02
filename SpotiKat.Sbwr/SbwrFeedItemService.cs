using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SpotiKat.Abstractions.HtmlAgilityPack;
using SpotiKat.Abstractions.Interfaces.HtmlAgilityPack;
using SpotiKat.Entities;
using SpotiKat.Interfaces.Logging;
using SpotiKat.Interfaces.Net.Http;
using SpotiKat.Sbwr.Exceptions;
using SpotiKat.Sbwr.interfaces.Configuration;
using SpotiKat.Sbwr.Interfaces;
using SpotiKat.Sbwr.Interfaces.HtmlParser;

namespace SpotiKat.Sbwr {
    public class SbwrFeedItemService : ISbwrFeedItemService {
        private const string HtmlWebLoadErrorMessageFormat = "Failed to load html web for url '{0}': {1}";
        private const string HtmlParserParseErrorMessageFormat = "Failed to parse html document for url '{0}': {1}";
        private readonly IFeedItemHtmlParser _feedItemHtmlParser;
        private readonly ILogFactory _logFactory;
        private readonly ISbwrConfiguration _sbwrConfiguration;
        private readonly IUrlBuilder _urlBuilder;
        private readonly IWebClient _webClient;

        public SbwrFeedItemService(IFeedItemHtmlParser feedItemHtmlParser,
            IUrlBuilder urlBuilder, ILogFactory logFactory,
            IWebClient webClient, ISbwrConfiguration sbwrConfiguration) {
            _feedItemHtmlParser = feedItemHtmlParser;
            _urlBuilder = urlBuilder;
            _logFactory = logFactory;
            _webClient = webClient;
            _sbwrConfiguration = sbwrConfiguration;
        }

        public async Task<FeedItemsResult> GetFeedItemsByGenreAsync(string genre, int page) {
            var url = _urlBuilder.BuildFeedItemByGenreUrl(genre, page);
            var htmlDocument = await GetHtmlDocumentAsync(url);
            return GetFeedItemsResult(htmlDocument, url);
        }

        private async Task<IHtmlDocument> GetHtmlDocumentAsync(string url) {
            try {
                _webClient.UserAgent = _sbwrConfiguration.WebClientUserAgent;

                var htmlContent = await _webClient.GetAsync(url);

                var htmlDocument = new HtmlDocumentImpl(new HtmlDocument());
                htmlDocument.LoadHtml(htmlContent);
                return htmlDocument;
            }
            catch (Exception ex) {
                _logFactory.GetLogger(typeof (SbwrFeedItemService))
                    .ErrorFormat(HtmlWebLoadErrorMessageFormat, url, ex.Message);
                throw new SbwrServiceException(string.Format(HtmlWebLoadErrorMessageFormat, url, ex.Message));
            }
        }

        private FeedItemsResult GetFeedItemsResult(IHtmlDocument htmlDocument, string url) {
            try {
                var feedItems = _feedItemHtmlParser.Parse(htmlDocument);
                return new FeedItemsResult {Items = feedItems};
            }
            catch (Exception ex) {
                _logFactory.GetLogger(typeof (SbwrFeedItemService))
                    .ErrorFormat(HtmlParserParseErrorMessageFormat, url, ex.Message);
                throw new SbwrServiceException(string.Format(HtmlParserParseErrorMessageFormat, url, ex.Message));
            }
        }
    }
}