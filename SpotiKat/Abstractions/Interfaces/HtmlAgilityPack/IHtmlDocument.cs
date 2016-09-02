using HtmlAgilityPack;

namespace SpotiKat.Abstractions.Interfaces.HtmlAgilityPack {
    public interface IHtmlDocument {
        HtmlNode DocumentNode { get; }
        void LoadHtml(string html);
    }
}