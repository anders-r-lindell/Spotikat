using SpotiKat.Interfaces.Configuration;

namespace SpotiKat.Configuration {
    public class GenresConfiguration : IGenresConfiguration {
        public string Genres {
            get { return Settings.Default.Genres; }
        }
    }
}