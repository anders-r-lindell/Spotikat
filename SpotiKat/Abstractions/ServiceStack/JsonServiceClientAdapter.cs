using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace SpotiKat.Abstractions.ServiceStack {
	public class JsonServiceClientAdapter : IJsonServiceClient {
		private readonly JsonServiceClient jsonServiceClient;

		public JsonServiceClientAdapter(JsonServiceClient jsonServiceClient) {
			this.jsonServiceClient = jsonServiceClient;
		}

		public TResponse Get<TResponse>(string absoluteUrl) {
			return this.jsonServiceClient.Get<TResponse>(absoluteUrl);
		}
	}
}