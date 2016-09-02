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
	public class RelatedAlbumRestServiceTests {
		[TestCase(null)]
		[TestCase("")]
		[TestCase("  ")]
		public void OnGet_TerritoryIsNullOrTrimEmpty_ReturnsErrorResponseWithStatusCodeBadRequest(string territory) {
			var service = new RelatedAlbumRestService {
			                                          	LogFactory = A.Fake<ILogFactory>()
			                                          };

			var response = service.OnGet(new RelatedAlbumRequest { Territory = territory, BoomkatUrl = "abc" });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}

		[TestCase("a")]
		[TestCase("abc")]
		public void OnGet_TerritoryInvalidLength_ReturnsErrorResponseWithStatusCodeBadRequest(string territory) {
			var service = new RelatedAlbumRestService {
			                                          	LogFactory = A.Fake<ILogFactory>()
			                                          };

			var response = service.OnGet(new RelatedAlbumRequest { Territory = territory, BoomkatUrl = "abc" });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}

		[TestCase(null)]
		[TestCase("")]
		[TestCase("  ")]
		public void OnGet_BoomkatUrlIsNullOrTrimEmpty_ReturnsErrorResponseWithStatusCodeBadRequest(string boomkatUrl) {
			var service = new RelatedAlbumRestService {
			                                          	LogFactory = A.Fake<ILogFactory>()
			                                          };

			var response = service.OnGet(new RelatedAlbumRequest { Territory = "se", BoomkatUrl = boomkatUrl });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}

		[Test]
		public void OnGet_RequestIsValid_CallsRelatedAlbumServiceGetAlbums() {
			var relatedAlbumServiceFake = A.Fake<IRelatedAlbumService>();

			var service = new RelatedAlbumRestService {
			                                          	LogFactory = A.Fake<ILogFactory>(),
			                                          	RelatedAlbumService = relatedAlbumServiceFake
			                                          };

			var response = service.OnGet(new RelatedAlbumRequest { Territory = "se", BoomkatUrl = "abc" });

			A.CallTo(() => relatedAlbumServiceFake.GetAlbums("se", "abc")).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void OnGet_RequestIsValidTerritoryIsNullValue_CallsRelatedAlbumServiceGetAlbumsWithTerritoryIsNull() {
			var relatedAlbumServiceFake = A.Fake<IRelatedAlbumService>();

			var service = new RelatedAlbumRestService {
			                                          	LogFactory = A.Fake<ILogFactory>(),
			                                          	RelatedAlbumService = relatedAlbumServiceFake
			                                          };

			var response = service.OnGet(new RelatedAlbumRequest { Territory = "-", BoomkatUrl = "abc" });

			A.CallTo(() => relatedAlbumServiceFake.GetAlbums(null, "abc")).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void OnGet_RelatedAlbumServiceGetAlbumsThrowsException_ReturnsErrorResponseWithStatusCodeInternalServerError() {
			var relatedAlbumServiceFake = A.Fake<IRelatedAlbumService>();
			A.CallTo(() => relatedAlbumServiceFake.GetAlbums(A<string>.Ignored, A<string>.Ignored)).Throws(new Exception("foo"));

			var service = new RelatedAlbumRestService {
			                                          	LogFactory = A.Fake<ILogFactory>(),
			                                          	RelatedAlbumService = relatedAlbumServiceFake
			                                          };

			var response = service.OnGet(new RelatedAlbumRequest { Territory = "se", BoomkatUrl = "abc" });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.InternalServerError));
		}

		[Test]
		public void OnGet_RelatedAlbumServiceGetAlbumsReturnsAlbums_ReturnsRelatedAlbumResponse() {
			var relatedAlbumServiceFake = A.Fake<IRelatedAlbumService>();
			A.CallTo(() => relatedAlbumServiceFake.GetAlbums(A<string>.Ignored, A<string>.Ignored)).Returns(new List<IAlbum> {
			                                                                                                                 	A.Fake<Album>()
			                                                                                                                 });

			var service = new RelatedAlbumRestService {
			                                          	LogFactory = A.Fake<ILogFactory>(),
			                                          	RelatedAlbumService = relatedAlbumServiceFake
			                                          };

			var response = service.OnGet(new RelatedAlbumRequest { Territory = "se", BoomkatUrl = "abc" });

			Assert.That(response, Is.TypeOf<RelatedAlbumResponse>());
			var relatedAlbumResponse = (RelatedAlbumResponse)response;
			Assert.That(relatedAlbumResponse.Info.Territory, Is.EqualTo("se"));
			Assert.That(relatedAlbumResponse.Info.BoomkatUrl, Is.EqualTo("abc"));
			Assert.That(relatedAlbumResponse.Albums.Count, Is.EqualTo(1));
		}
	}
}