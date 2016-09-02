using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SpotiKat.Entities;
using SpotiKat.Interfaces.Configuration;
using SpotiKat.Services.Interfaces;

namespace SpotiKat.Services {
    public class GenreService : IGenreService {
        private readonly IGenresConfiguration _genresConfiguration;

        public GenreService(IGenresConfiguration genresConfiguration) {
            _genresConfiguration = genresConfiguration;
        }

        public IList<Genre> GetGenres() {
            var xDocument = XDocument.Parse(_genresConfiguration.Genres);

            return xDocument.Descendants("option").Select(option => new Genre {
                Text = option.Value,
                Value = option.Attribute("value").Value,
                Source =
                    (option.Attribute("source") != null)
                        ? (FeedItemSource) Enum.Parse(typeof (FeedItemSource), option.Attribute("source").Value)
                        : FeedItemSource.Boomkat,
                IsLastAlbumRoute =
                    (option.Attribute("isLastAlbumRoute") != null)
                        ? bool.Parse(option.Attribute("isLastAlbumRoute").Value)
                        : false
            }).ToList();
        }
    }
}