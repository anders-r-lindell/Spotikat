using NUnit.Framework;
using ServiceStack;

using SpotiKat.Api.ServiceModel.Enums;
using SpotiKat.Api.ServiceModel.Response;

namespace SpotiKat.Api.ServiceInterface.Integration.Tests {
	[TestFixture]
	[Ignore]
	public class ArtistAlbumRestServiceTests {
		[Test]
		public void OnGet_ValidRequest_ReturnsArtistAlbumResponseWithStatusCodeOK() {
			var serviceClient = new JsonServiceClient();

			var response = serviceClient.Get<ArtistAlbumResponse>("http://spotikat2.local/api/artistalbums/se/?artisthref=spotify:artist:3iOvXCl6edW5Um0fXEBRXy");

			Assert.That(response.Status.StatusCode, Is.EqualTo(StatusCode.OK));
		}

		[Test]
		public void OnGet_NotValidRequest_ReturnsArtistAlbumResponseWithStatusCodeBadRequest() {
			var serviceClient = new JsonServiceClient();

			var response = serviceClient.Get<ArtistAlbumResponse>("http://spotikat2.local/api/artistalbums/se/");

			Assert.That(response.Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}
	}

}
