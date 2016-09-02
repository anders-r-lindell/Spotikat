using NUnit.Framework;
using ServiceStack;

using SpotiKat.Api.ServiceModel.Enums;
using SpotiKat.Api.ServiceModel.Response;

namespace SpotiKat.Api.ServiceInterface.Integration.Tests {
	[TestFixture]
	[Ignore]
	public class SimilarArtistRestServiceTests {
		[Test]
		public void OnGet_ValidRequest_ReturnsSimilarArtistResponseWithStatusCodeOK() {
			var serviceClient = new JsonServiceClient();

			var response = serviceClient.Get<SimilarArtistResponse>("http://spotikat2.local/api/similarartists/?artistname=the xx");

			Assert.That(response.Status.StatusCode, Is.EqualTo(StatusCode.OK));
		}

		[Test]
		public void OnGet_NotValidRequest_ReturnsSimilarArtistResponseWithStatusCodeBadRequest() {
			var serviceClient = new JsonServiceClient();

			var response = serviceClient.Get<SimilarArtistResponse>("http://spotikat2.local/api/similarartists/");

			Assert.That(response.Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}
	}
}
