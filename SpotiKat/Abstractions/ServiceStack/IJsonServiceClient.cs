namespace SpotiKat.Abstractions.ServiceStack {
	public interface IJsonServiceClient {
		TResponse Get<TResponse>(string absoluteUrl);
	}
}