using System.Collections.Generic;
using SpotiKat.Entities;

namespace SpotiKat.Services.Interfaces {
    public interface IGenreService {
        IList<Genre> GetGenres();
    }
}