using System;
using System.Collections.Generic;

using FakeItEasy;

using NUnit.Framework;

using ServiceStack.Logging;

using SpotiKat.Api.ServiceModel.Enums;
using SpotiKat.Api.ServiceModel.Request;
using SpotiKat.Api.ServiceModel.Response;
using SpotiKat.Services.Entities;
using SpotiKat.Services.Interfaces;

namespace SpotiKat.Api.ServiceInterface.Tests {
	[TestFixture]
	public class ArtistAlbumRestServiceTests {
		[TestCase(null)]
		[TestCase("")]
		[TestCase("  ")]
		public void OnGet_TerritoryIsNullOrTrimEmpty_ReturnsErrorResponseWithStatusCodeBadRequest(string territory) {
			var service = new ArtistAlbumRestService {
			                                         	LogFactory = A.Fake<ILogFactory>()
			                                         };

			var response = service.OnGet(new ArtistAlbumRequest { Territory = territory, ArtistHref = "abc" });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}

		[TestCase("a")]
		[TestCase("abc")]
		public void OnGet_TerritoryInvalidLength_ReturnsErrorResponseWithStatusCodeBadRequest(string territory) {
			var service = new ArtistAlbumRestService {
			                                         	LogFactory = A.Fake<ILogFactory>()
			                                         };

			var response = service.OnGet(new ArtistAlbumRequest { Territory = territory, ArtistHref = "abc" });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}

		[TestCase(null)]
		[TestCase("")]
		[TestCase("  ")]
		public void OnGet_ArtistHrefIsNullOrTrimEmpty_ReturnsErrorResponseWithStatusCodeBadRequest(string artistHref) {
			var service = new ArtistAlbumRestService {
			                                         	LogFactory = A.Fake<ILogFactory>()
			                                         };

			var response = service.OnGet(new ArtistAlbumRequest { Territory = "se", ArtistHref = artistHref });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}

		[Test]
		public void OnGet_RequestIsValid_CallsArtistAlbumServiceGetAlbums() {
			var artistAlbumServiceFake = A.Fake<IArtistAlbumService>();

			var service = new ArtistAlbumRestService {
			                                         	LogFactory = A.Fake<ILogFactory>(),
			                                         	ArtistAlbumService = artistAlbumServiceFake
			                                         };

			var response = service.OnGet(new ArtistAlbumRequest { Territory = "se", ArtistHref = "abc" });

			A.CallTo(() => artistAlbumServiceFake.GetAlbums("se", "abc")).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void OnGet_RequestIsValidTerritoryIsNullValue_CallsArtistAlbumServiceGetAlbumsWithTerritoryIsNull() {
			var artistAlbumServiceFake = A.Fake<IArtistAlbumService>();

			var service = new ArtistAlbumRestService {
			                                         	LogFactory = A.Fake<ILogFactory>(),
			                                         	ArtistAlbumService = artistAlbumServiceFake
			                                         };

			var response = service.OnGet(new ArtistAlbumRequest { Territory = "-", ArtistHref = "abc" });

			A.CallTo(() => artistAlbumServiceFake.GetAlbums(null, "abc")).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void OnGet_ArtistAlbumServiceGetAlbumsThrowsException_ReturnsErrorResponseWithStatusCodeInternalServerError() {
			var artistAlbumServiceFake = A.Fake<IArtistAlbumService>();
			A.CallTo(() => artistAlbumServiceFake.GetAlbums(A<string>.Ignored, A<string>.Ignored)).Throws(new Exception("foo"));

			var service = new ArtistAlbumRestService {
			                                         	LogFactory = A.Fake<ILogFactory>(),
			                                         	ArtistAlbumService = artistAlbumServiceFake
			                                         };

			var response = service.OnGet(new ArtistAlbumRequest { Territory = "se", ArtistHref = "abc" });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.InternalServerError));
		}

		[Test]
		public void OnGet_ArtistAlbumServiceGetAlbumsReturnsAlbums_ReturnsArtistAlbumResponse() {
			var artistAlbumServiceFake = A.Fake<IArtistAlbumService>();
			A.CallTo(() => artistAlbumServiceFake.GetAlbums(A<string>.Ignored, A<string>.Ignored)).Returns(new List<IAlbum> {
			                                                                                                                	A.Fake<Album>()
			                                                                                                                });

			var service = new ArtistAlbumRestService {
			                                         	LogFactory = A.Fake<ILogFactory>(),
			                                         	ArtistAlbumService = artistAlbumServiceFake
			                                         };

			var response = service.OnGet(new ArtistAlbumRequest { Territory = "se", ArtistHref = "abc" });

			var pages = (IList<string>)null;
			var albumServiceFake = A.Fake<IAlbumService>();
			A.CallTo(() => albumServiceFake.GetAlbums(A<string>.Ignored, A<int>.Ignored, out pages)).Returns(new List<IAlbum> {
			                                                                                                       	A.Fake<Album>()
			                                                                                                       });

			Assert.That(response, Is.TypeOf<ArtistAlbumResponse>());
			var artistAlbumResponse = (ArtistAlbumResponse)response;
			Assert.That(artistAlbumResponse.Info.Territory, Is.EqualTo("se"));
			Assert.That(artistAlbumResponse.Info.ArtistHref, Is.EqualTo("abc"));
			Assert.That(artistAlbumResponse.Albums.Count, Is.EqualTo(1));
		}
	}
}