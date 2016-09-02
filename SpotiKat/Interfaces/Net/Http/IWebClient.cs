using System.Threading.Tasks;

namespace SpotiKat.Interfaces.Net.Http {
    public interface IWebClient {
        string UserAgent { get; set; }
        Task<string> GetAsync(string url);
    }
}