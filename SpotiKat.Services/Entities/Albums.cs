using System.Collections.Generic;
using SpotiKat.Entities;

namespace SpotiKat.Services.Entities {
    public class Albums {
        public IList<Album> Items { get; set; }
        public IList<Page> Pages { get; set; }
    }
}