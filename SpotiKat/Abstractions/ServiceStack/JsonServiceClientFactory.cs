using System;

using ServiceStack.ServiceClient.Web;

namespace SpotiKat.Abstractions.ServiceStack {
	public class JsonServiceClientFactory : IJsonServiceClientFactory {
		public IJsonServiceClient Create() {
			return new JsonServiceClientAdapter(new JsonServiceClient());
		}
	}
}