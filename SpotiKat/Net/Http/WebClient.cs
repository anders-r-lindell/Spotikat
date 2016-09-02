using System;
using System.Net.Http;
using System.Threading.Tasks;
using SpotiKat.Interfaces.Logging;
using SpotiKat.Interfaces.Net.Http;

namespace SpotiKat.Net.Http {
    public class WebClient : IWebClient {
        private const string WebClientErrorMessageFormat =
            "Failed to get url '{0}': {1}";

        private readonly ILogFactory _logFactory;

        public WebClient(ILogFactory logFactory) {
            _logFactory = logFactory;
        }

        public string UserAgent { get; set; }

        public async Task<string> GetAsync(string url) {
            try {
                using (var client = new HttpClient()) {
                    if (!string.IsNullOrWhiteSpace(UserAgent)) {
                        client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
                    }

                    var response = await client.GetAsync(url);
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex) {
                _logFactory.GetLogger(typeof (WebClient)).ErrorFormat(
                    WebClientErrorMessageFormat, url, ex.Message);
                throw;
            }
        }
    }
}