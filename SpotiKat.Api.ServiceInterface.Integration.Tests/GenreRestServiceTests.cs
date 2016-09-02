using NUnit.Framework;
using ServiceStack;

using SpotiKat.Api.ServiceModel.Enums;
using SpotiKat.Api.ServiceModel.Response;

namespace SpotiKat.Api.ServiceInterface.Integration.Tests {
	[TestFixture]
	[Ignore]
	public class GenreRestServiceTests {
		[Test]
		public void OnGet_ValidRequest_ReturnsGenreResponseWithStatusCodeOK() {
			var serviceClient = new JsonServiceClient();

			var response = serviceClient.Get<GenreResponse>("http://spotikat2.local/api/genres/");

			Assert.That(response.Status.StatusCode, Is.EqualTo(StatusCode.OK));
		}
	}
}
