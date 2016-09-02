using NUnit.Framework;
using ServiceStack;

using SpotiKat.Api.ServiceModel.Enums;
using SpotiKat.Api.ServiceModel.Response;

namespace SpotiKat.Api.ServiceInterface.Integration.Tests {
	[TestFixture]
	[Ignore]
	public class AlbumRestServiceTests {
		[Test]
		public void OnGet_ValidRequestGenreIsNull_ReturnsAlbumResponseWithStatusCodeOK() {
			var serviceClient = new JsonServiceClient();

			var response = serviceClient.Get<AlbumResponse>("http://spotikat2.local/api/albums/se/1/");

			Assert.That(response.Status.StatusCode, Is.EqualTo(StatusCode.OK));
		}

		[Test]
		public void OnGet_ValidRequestGenreIsNotNull_ReturnsAlbumResponseWithStatusCodeOK() {
			var serviceClient = new JsonServiceClient();

			var response = serviceClient.Get<AlbumResponse>("http://spotikat2.local/api/albums/se/32/1/");

			Assert.That(response.Status.StatusCode, Is.EqualTo(StatusCode.OK));
		}

		[Test]
		public void OnGet_NotValidRequest_ReturnsAlbumResponseWithStatusCodeBadRequest() {
			var serviceClient = new JsonServiceClient();

			var response = serviceClient.Get<AlbumResponse>("http://spotikat2.local/api/albums/se/0/");

			Assert.That(response.Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}
	}

}
