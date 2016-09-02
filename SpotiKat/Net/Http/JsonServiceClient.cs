using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpotiKat.Interfaces.Configuration;
using SpotiKat.Interfaces.Logging;
using SpotiKat.Interfaces.Net.Http;

namespace SpotiKat.Net.Http {
    public class JsonServiceClient : IJsonServiceClient {
        private const string JsonServiceClientErrorMessageFormat =
            "Failed to get url '{0}': {1}";

        private readonly IJsonServiceClientConfiguration _jsonServiceClientConfiguration;
        private readonly ILogFactory _logFactory;
        private readonly IWebClient _webClient;

        public JsonServiceClient(IWebClient webClient, IJsonServiceClientConfiguration jsonServiceClientConfiguration,
            ILogFactory logFactory) {
            _webClient = webClient;
            _jsonServiceClientConfiguration = jsonServiceClientConfiguration;
            _logFactory = logFactory;
        }

        public async Task<T> GetAsync<T>(string url) {
            var retries = 0;
            var serviceCallIsSuccessfull = false;

            var responseObject = default(T);

            while (!serviceCallIsSuccessfull &&
                   (retries == 0 || retries < _jsonServiceClientConfiguration.MaxNumberOfRetries)) {
                try {
                    if (retries > 0) {
                        var milliseconds = _jsonServiceClientConfiguration.SlowDownFactor*(retries*retries);
                        await Task.Delay(milliseconds);
                    }

                    var response = await _webClient.GetAsync(url);
                    serviceCallIsSuccessfull = true;
                    return JsonConvert.DeserializeObject<T>(response);
                }
                catch (Exception ex) {
                    _logFactory.GetLogger(typeof (WebClient)).ErrorFormat(
                        JsonServiceClientErrorMessageFormat, url, ex.Message);

                    if (retries == _jsonServiceClientConfiguration.MaxNumberOfRetries - 1) {
                        throw;
                    }

                    retries++;
                }
            }

            return responseObject;
        }
    }
}