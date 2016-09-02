using NUnit.Framework;
using ServiceStack;

using SpotiKat.Api.ServiceModel.Enums;
using SpotiKat.Api.ServiceModel.Response;

namespace SpotiKat.Api.ServiceInterface.Integration.Tests {
	[TestFixture]
	[Ignore]
	public class RelatedAlbumRestServiceTests {
		[Test]
		public void OnGet_ValidRequest_ReturnsRelatedAlbumResponseWithStatusCodeOK() {
			var serviceClient = new JsonServiceClient();

			var response = serviceClient.Get<RelatedAlbumResponse>("http://spotikat2.local/api/relatedalbums/se/?boomkaturl=http://boomkat.com/downloads/568054-the-xx-coexist");

			Assert.That(response.Status.StatusCode, Is.EqualTo(StatusCode.OK));
		}

		[Test]
		public void OnGet_NotValidRequest_ReturnsRelatedAlbumResponseWithStatusCodeBadRequest() {
			var serviceClient = new JsonServiceClient();

			var response = serviceClient.Get<RelatedAlbumResponse>("http://spotikat2.local/api/relatedalbums/se/");

			Assert.That(response.Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}
	}

}
