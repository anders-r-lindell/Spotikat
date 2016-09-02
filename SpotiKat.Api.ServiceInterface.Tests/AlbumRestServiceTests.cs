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
	public class AlbumRestServiceTests {
		[TestCase(null)]
		[TestCase("")]
		[TestCase("  ")]
		public void OnGet_TerritoryIsNullOrTrimEmpty_ReturnsErrorResponseWithStatusCodeBadRequest(string territory) {
			var service = new AlbumRestService {
			                                   	LogFactory = A.Fake<ILogFactory>()
			                                   };

			var response = service.OnGet(new AlbumRequest { Territory = territory, Genre = null, Page = 1 });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}

		[TestCase("a")]
		[TestCase("abc")]
		public void OnGet_TerritoryInvalidLength_ReturnsErrorResponseWithStatusCodeBadRequest(string territory) {
			var service = new AlbumRestService {
				LogFactory = A.Fake<ILogFactory>()
			};

			var response = service.OnGet(new AlbumRequest { Territory = territory, Genre = null, Page = 1 });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}

		[TestCase(0)]
		[TestCase(-1)]
		public void OnGet_PageIsLessThanOne_ReturnsErrorResponseWithStatusCodeBadRequest(int page) {
			var service = new AlbumRestService {
			                                   	LogFactory = A.Fake<ILogFactory>()
			                                   };

			var response = service.OnGet(new AlbumRequest { Territory = "se", Genre = null, Page = page });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}

		[Test]
		public void OnGet_GenreIsLessThanZero_ReturnsErrorResponseWithStatusCodeBadRequest() {
			var service = new AlbumRestService {
				LogFactory = A.Fake<ILogFactory>()
			};

			var response = service.OnGet(new AlbumRequest { Territory = "se", Genre = -1, Page = 1 });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}

		[Test]
		public void OnGet_RequestIsValidGenreIsNull_CallsAlbumServiceGetAlbums() {
			var albumServiceFake = A.Fake<IAlbumService>();

			var service = new AlbumRestService {
			                                   	LogFactory = A.Fake<ILogFactory>(),
			                                   	AlbumService = albumServiceFake
			                                   };

			var response = service.OnGet(new AlbumRequest { Territory = "se", Genre = null, Page = 1 });

			var pages = (IList<string>)null;
			A.CallTo(() => albumServiceFake.GetAlbums("se", 1, out pages)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void OnGet_RequestIsValidGenreIsNotNull_CallsAlbumServiceGetAlbumsByGenre() {
			var albumServiceFake = A.Fake<IAlbumService>();

			var service = new AlbumRestService {
			                                   	LogFactory = A.Fake<ILogFactory>(),
			                                   	AlbumService = albumServiceFake
			                                   };

			var response = service.OnGet(new AlbumRequest { Territory = "se", Genre = 2, Page = 1 });

			var pages = (IList<string>)null;
			A.CallTo(() => albumServiceFake.GetAlbumsByGenre("se", 2, 1, out pages)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void OnGet_RequestIsValidGenreIsNullTerritoryIsNullValue_CallsAlbumServiceGetAlbumsWithTerritoryIsNull() {
			var albumServiceFake = A.Fake<IAlbumService>();

			var service = new AlbumRestService {
				LogFactory = A.Fake<ILogFactory>(),
				AlbumService = albumServiceFake
			};

			var response = service.OnGet(new AlbumRequest { Territory = "-", Genre = null, Page = 1 });

			var pages = (IList<string>)null;
			A.CallTo(() => albumServiceFake.GetAlbums(null, 1, out pages)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void OnGet_RequestIsValidGenreIsNotNullTerritoryIsNullValue_CallsAlbumServiceGetAlbumsByGenreWithTerritoryIsNull() {
			var albumServiceFake = A.Fake<IAlbumService>();

			var service = new AlbumRestService {
				LogFactory = A.Fake<ILogFactory>(),
				AlbumService = albumServiceFake
			};

			var response = service.OnGet(new AlbumRequest { Territory = "-", Genre = 2, Page = 1 });

			var pages = (IList<string>)null;
			A.CallTo(() => albumServiceFake.GetAlbumsByGenre(null, 2, 1, out pages)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void OnGet_AlbumServiceGetAlbumsThrowsException_ReturnsErrorResponseWithStatusCodeInternalServerError() {
			var pages = (IList<string>)null;
			var albumServiceFake = A.Fake<IAlbumService>();
			A.CallTo(() => albumServiceFake.GetAlbums(A<string>.Ignored, A<int>.Ignored, out pages)).Throws(new Exception("foo"));

			var service = new AlbumRestService {
			                                   	LogFactory = A.Fake<ILogFactory>(),
			                                   	AlbumService = albumServiceFake
			                                   };

			var response = service.OnGet(new AlbumRequest { Territory = "se", Genre = null, Page = 1 });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.InternalServerError));
		}

		[Test]
		public void OnGet_AlbumServiceGetAlbumsReturnsAlbums_ReturnsAlbumResponse() {
			var pages = (IList<string>)null;
			var albumServiceFake = A.Fake<IAlbumService>();
			A.CallTo(() => albumServiceFake.GetAlbums(A<string>.Ignored, A<int>.Ignored, out pages)).Returns(new List<IAlbum> {
			                                                                                                       	A.Fake<Album>()
			                                                                                                       });

			var service = new AlbumRestService {
			                                   	LogFactory = A.Fake<ILogFactory>(),
			                                   	AlbumService = albumServiceFake
			                                   };

			var response = service.OnGet(new AlbumRequest { Territory = "se", Genre = null, Page = 1 });

			Assert.That(response, Is.TypeOf<AlbumResponse>());
			var albumResponse = (AlbumResponse)response;
			Assert.That(albumResponse.Info.Territory, Is.EqualTo("se"));
			Assert.That(albumResponse.Info.Genre, Is.Null);
			Assert.That(albumResponse.Info.Page, Is.EqualTo(1));
			Assert.That(albumResponse.Albums.Count, Is.EqualTo(1));
		}

		[Test]
		public void OnGet_AlbumServiceGetAlbumsByGenreThrowsException_ReturnsErrorResponseWithStatusCodeInternalServerError() {
			var pages = (IList<string>)null;
			var albumServiceFake = A.Fake<IAlbumService>();
			A.CallTo(() => albumServiceFake.GetAlbumsByGenre(A<string>.Ignored, A<int>.Ignored, A<int>.Ignored, out pages)).Throws(new Exception("foo"));

			var service = new AlbumRestService {
			                                   	LogFactory = A.Fake<ILogFactory>(),
			                                   	AlbumService = albumServiceFake
			                                   };

			var response = service.OnGet(new AlbumRequest { Territory = "se", Genre = 2, Page = 1 });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.InternalServerError));
		}

		[Test]
		public void OnGet_AlbumServiceGetAlbumsByGenreReturnsAlbums_ReturnsAlbumResponse() {
			var pages = (IList<string>)null;
			var albumServiceFake = A.Fake<IAlbumService>();
			A.CallTo(() => albumServiceFake.GetAlbumsByGenre(A<string>.Ignored, A<int>.Ignored, A<int>.Ignored, out pages)).Returns(new List<IAlbum> {
			                                                                                                                              	A.Fake<Album>()
			                                                                                                                              });

			var service = new AlbumRestService {
			                                   	LogFactory = A.Fake<ILogFactory>(),
			                                   	AlbumService = albumServiceFake
			                                   };

			var response = service.OnGet(new AlbumRequest { Territory = "se", Genre = 2, Page = 1 });

			Assert.That(response, Is.TypeOf<AlbumResponse>());
			var albumResponse = (AlbumResponse)response;
			Assert.That(albumResponse.Info.Territory, Is.EqualTo("se"));
			Assert.That(albumResponse.Info.Genre, Is.EqualTo(2));
			Assert.That(albumResponse.Info.Page, Is.EqualTo(1));
			Assert.That(albumResponse.Albums.Count, Is.EqualTo(1));
		}
	}
}