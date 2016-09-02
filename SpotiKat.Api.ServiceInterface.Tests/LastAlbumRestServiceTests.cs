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
	public class LastLastAlbumRestServiceTests {
		[TestCase(null)]
		[TestCase("")]
		[TestCase("  ")]
		public void OnGet_TerritoryIsNullOrTrimEmpty_ReturnsErrorResponseWithStatusCodeBadRequest(string territory) {
			var service = new LastAlbumRestService {
			                                       	LogFactory = A.Fake<ILogFactory>()
			                                       };

			var response = service.OnGet(new LastAlbumRequest { Territory = territory, Genre = null });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}

		[TestCase("a")]
		[TestCase("abc")]
		public void OnGet_TerritoryInvalidLength_ReturnsErrorResponseWithStatusCodeBadRequest(string territory) {
			var service = new LastAlbumRestService {
			                                       	LogFactory = A.Fake<ILogFactory>()
			                                       };

			var response = service.OnGet(new LastAlbumRequest { Territory = territory, Genre = null });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}

		[Test]
		public void OnGet_GenreIsLessThanZero_ReturnsErrorResponseWithStatusCodeBadRequest() {
			var service = new LastAlbumRestService {
				LogFactory = A.Fake<ILogFactory>()
			};

			var response = service.OnGet(new LastAlbumRequest { Territory = "se", Genre = -1 });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}

		[Test]
		public void OnGet_PageIsLessThanOne_ReturnsErrorResponseWithStatusCodeBadRequest() {
			var service = new LastAlbumRestService {
				LogFactory = A.Fake<ILogFactory>()
			};

			var response = service.OnGet(new LastAlbumRequest { Territory = "se", Genre = null, Page = 0 });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}

		[Test]
		public void OnGet_RequestIsValidGenreIsNull_CallsLastAlbumServiceGetAlbums() {
			var lastLastAlbumServiceFake = A.Fake<ILastAlbumService>();

			var service = new LastAlbumRestService {
			                                       	LogFactory = A.Fake<ILogFactory>(),
			                                       	LastAlbumService = lastLastAlbumServiceFake
			                                       };

			var response = service.OnGet(new LastAlbumRequest { Territory = "se", Genre = null });

			A.CallTo(() => lastLastAlbumServiceFake.GetAlbums("se", 1)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void OnGet_RequestIsValidGenreIsNotNull_CallsLastAlbumServiceGetAlbumsByGenre() {
			var lastLastAlbumServiceFake = A.Fake<ILastAlbumService>();

			var service = new LastAlbumRestService {
			                                       	LogFactory = A.Fake<ILogFactory>(),
			                                       	LastAlbumService = lastLastAlbumServiceFake
			                                       };

			var response = service.OnGet(new LastAlbumRequest { Territory = "se", Genre = 2 });

			A.CallTo(() => lastLastAlbumServiceFake.GetAlbumsByGenre("se", 2)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void OnGet_RequestIsValidGenreIsNullTerritoryIsNullValue_CallsLastAlbumServiceGetAlbumsWithTerritoryIsNull() {
			var lastLastAlbumServiceFake = A.Fake<ILastAlbumService>();

			var service = new LastAlbumRestService {
			                                       	LogFactory = A.Fake<ILogFactory>(),
			                                       	LastAlbumService = lastLastAlbumServiceFake
			                                       };

			var response = service.OnGet(new LastAlbumRequest { Territory = "-", Genre = null });

			A.CallTo(() => lastLastAlbumServiceFake.GetAlbums(null, 1)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void OnGet_RequestIsValidGenreIsNotNullTerritoryIsNullValue_CallsLastAlbumServiceGetAlbumsByGenreWithTerritoryIsNull() {
			var lastLastAlbumServiceFake = A.Fake<ILastAlbumService>();

			var service = new LastAlbumRestService {
			                                       	LogFactory = A.Fake<ILogFactory>(),
			                                       	LastAlbumService = lastLastAlbumServiceFake
			                                       };

			var response = service.OnGet(new LastAlbumRequest { Territory = "-", Genre = 2 });

			A.CallTo(() => lastLastAlbumServiceFake.GetAlbumsByGenre(null, 2)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void OnGet_LastAlbumServiceGetAlbumsThrowsException_ReturnsErrorResponseWithStatusCodeInternalServerError() {
			var lastLastAlbumServiceFake = A.Fake<ILastAlbumService>();
			A.CallTo(() => lastLastAlbumServiceFake.GetAlbums(A<string>.Ignored, A<int>.Ignored)).Throws(new Exception("foo"));

			var service = new LastAlbumRestService {
			                                       	LogFactory = A.Fake<ILogFactory>(),
			                                       	LastAlbumService = lastLastAlbumServiceFake
			                                       };

			var response = service.OnGet(new LastAlbumRequest { Territory = "se", Genre = null });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.InternalServerError));
		}

		[Test]
		public void OnGet_LastAlbumServiceGetAlbumsReturnsAlbums_ReturnsAlbumResponse() {
			var lastLastAlbumServiceFake = A.Fake<ILastAlbumService>();
			A.CallTo(() => lastLastAlbumServiceFake.GetAlbums(A<string>.Ignored, A<int>.Ignored)).Returns(new List<IAlbum> {
			                                                                                               	A.Fake<Album>()
			                                                                                               });

			var service = new LastAlbumRestService {
			                                       	LogFactory = A.Fake<ILogFactory>(),
			                                       	LastAlbumService = lastLastAlbumServiceFake
			                                       };

			var response = service.OnGet(new LastAlbumRequest { Territory = "se", Genre = null });

			Assert.That(response, Is.TypeOf<LastAlbumResponse>());
			var albumResponse = (LastAlbumResponse)response;
			Assert.That(albumResponse.Info.Territory, Is.EqualTo("se"));
			Assert.That(albumResponse.Info.Genre, Is.Null);
			Assert.That(albumResponse.Albums.Count, Is.EqualTo(1));
		}

		[Test]
		public void OnGet_LastAlbumServiceGetAlbumsByGenreThrowsException_ReturnsErrorResponseWithStatusCodeInternalServerError() {
			var lastLastAlbumServiceFake = A.Fake<ILastAlbumService>();
			A.CallTo(() => lastLastAlbumServiceFake.GetAlbumsByGenre(A<string>.Ignored, A<int>.Ignored)).Throws(new Exception("foo"));

			var service = new LastAlbumRestService {
			                                       	LogFactory = A.Fake<ILogFactory>(),
			                                       	LastAlbumService = lastLastAlbumServiceFake
			                                       };

			var response = service.OnGet(new LastAlbumRequest { Territory = "se", Genre = 2 });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.InternalServerError));
		}

		[Test]
		public void OnGet_LastAlbumServiceGetAlbumsByGenreReturnsAlbums_ReturnsAlbumResponse() {
			var lastLastAlbumServiceFake = A.Fake<ILastAlbumService>();
			A.CallTo(() => lastLastAlbumServiceFake.GetAlbumsByGenre(A<string>.Ignored, A<int>.Ignored)).Returns(new List<IAlbum> {
			                                                                                                                      	A.Fake<Album>()
			                                                                                                                      });

			var service = new LastAlbumRestService {
			                                       	LogFactory = A.Fake<ILogFactory>(),
			                                       	LastAlbumService = lastLastAlbumServiceFake
			                                       };

			var response = service.OnGet(new LastAlbumRequest { Territory = "se", Genre = 2 });

			Assert.That(response, Is.TypeOf<LastAlbumResponse>());
			var albumResponse = (LastAlbumResponse)response;
			Assert.That(albumResponse.Info.Territory, Is.EqualTo("se"));
			Assert.That(albumResponse.Info.Genre, Is.EqualTo(2));
			Assert.That(albumResponse.Albums.Count, Is.EqualTo(1));
		}
	}
}