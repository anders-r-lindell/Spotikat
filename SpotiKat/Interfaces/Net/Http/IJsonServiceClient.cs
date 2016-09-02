using System.Threading.Tasks;

namespace SpotiKat.Interfaces.Net.Http {
    public interface IJsonServiceClient {
        Task<T> GetAsync<T>(string url);
    }
}