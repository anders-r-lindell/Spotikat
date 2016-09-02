using System;
using HtmlAgilityPack;
using SpotiKat.Abstractions.Interfaces.HtmlAgilityPack;

//ncrunch: no coverage start

namespace SpotiKat.Abstractions.HtmlAgilityPack {
    public class HtmlDocumentImpl : IHtmlDocument {
        private readonly HtmlDocument _htmlDocument;

        public HtmlDocumentImpl(HtmlDocument htmlDocument) {
            if (htmlDocument == null) {
                throw new ArgumentNullException("htmlDocument");
            }

            _htmlDocument = htmlDocument;
        }

        public HtmlNode DocumentNode {
            get { return _htmlDocument.DocumentNode; }
        }

        public void LoadHtml(string html) {
            _htmlDocument.LoadHtml(html);
        }
    }
}

//ncrunch: no coverage end