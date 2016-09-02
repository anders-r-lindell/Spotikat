using System.Collections.Generic;
using SpotiKat.Abstractions.Interfaces.HtmlAgilityPack;
using SpotiKat.Entities;

namespace SpotiKat.Boomkat.Interfaces.HtmlParser {
    public interface IAlbumsFeedItemHtmlParser {
        IList<FeedItem> Parse(IHtmlDocument htmlDocument, out IList<string> pages);
    }
}