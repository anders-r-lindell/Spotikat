using System;
using System.Collections.Generic;
using SpotiKat.Entities;

namespace SpotiKat.Boomkat.Comparer {
    public class FeedItemComparer : IEqualityComparer<FeedItem> {
        public bool Equals(FeedItem x, FeedItem y) {
            if (x == null || y == null) {
                return false;
            }

            return x.Artist.Equals(y.Artist, StringComparison.InvariantCultureIgnoreCase) &&
                   x.Album.Equals(y.Album, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(FeedItem obj) {
            var artist = (obj != null && obj.Artist != null) ? obj.Artist.ToLower() : "";
            var album = (obj != null && obj.Album != null) ? obj.Album.ToLower() : "";
            return artist.GetHashCode() + album.GetHashCode();
        }
    }
}