using System.Collections.Generic;
using SpotiKat.Abstractions.Interfaces.HtmlAgilityPack;
using SpotiKat.Entities;

namespace SpotiKat.Sbwr.Interfaces.HtmlParser {
    public interface IFeedItemHtmlParser {
        IList<FeedItem> Parse(IHtmlDocument htmlDocument);
    }
}