using System;
using System.Collections.Generic;
using SpotiKat.Entities;

namespace SpotiKat.Services.Comparer {
    public class AlbumComparer : IEqualityComparer<Album> {
        public bool Equals(Album x, Album y) {
            if (x == null || y == null) {
                return false;
            }

            return x.Href.Equals(y.Href, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(Album obj) {
            var href = (obj != null && obj.Href != null) ? obj.Href.ToLower() : "";
            return href.GetHashCode();
        }
    }
}