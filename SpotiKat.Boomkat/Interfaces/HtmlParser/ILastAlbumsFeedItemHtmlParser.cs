using System.Collections.Generic;
using SpotiKat.Abstractions.Interfaces.HtmlAgilityPack;
using SpotiKat.Entities;

namespace SpotiKat.Boomkat.Interfaces.HtmlParser {
    public interface ILastAlbumsFeedItemHtmlParser {
        IList<FeedItem> Parse(IHtmlDocument htmlDocument, out IList<string> pages);
    }
}