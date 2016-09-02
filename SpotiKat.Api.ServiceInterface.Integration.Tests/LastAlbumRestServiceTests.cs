using NUnit.Framework;
using ServiceStack;

using SpotiKat.Api.ServiceModel.Enums;
using SpotiKat.Api.ServiceModel.Response;

namespace SpotiKat.Api.ServiceInterface.Integration.Tests {
	[TestFixture]
	[Ignore]
	public class LastAlbumRestServiceTests {
		[Test]
		public void OnGet_ValidRequestGenreIsNull_ReturnsLastAlbumResponseWithStatusCodeOK() {
			var serviceClient = new JsonServiceClient();

			var response = serviceClient.Get<LastAlbumResponse>("http://spotikat2.local/api/lastalbums/se/");

			Assert.That(response.Status.StatusCode, Is.EqualTo(StatusCode.OK));
		}

		[Test]
		public void OnGet_ValidRequestGenreIsNotNull_ReturnsLastAlbumResponseWithStatusCodeOK() {
			var serviceClient = new JsonServiceClient();

			var response = serviceClient.Get<LastAlbumResponse>("http://spotikat2.local/api/lastalbums/se/32/");

			Assert.That(response.Status.StatusCode, Is.EqualTo(StatusCode.OK));
		}

		[Test]
		public void OnGet_NotValidRequest_ReturnsLastAlbumResponseWithStatusCodeBadRequest() {
			var serviceClient = new JsonServiceClient();

			var response = serviceClient.Get<LastAlbumResponse>("http://spotikat2.local/api/lastalbums/se/-1/");

			Assert.That(response.Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}
	}

}
