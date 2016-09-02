namespace SpotiKat.Abstractions.Interfaces.HtmlAgilityPack {
    public interface IHtmlWeb {
        IHtmlDocument Load(string absoluteUrl);
    }
}