using System.Collections.Generic;

namespace SpotiKat.Entities {
    public class FeedItemsResult {
        public IList<FeedItem> Items { get; set; }
        public IList<string> Pages { get; set; }
    }
}