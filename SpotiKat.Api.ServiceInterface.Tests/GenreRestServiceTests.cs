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

using ServiceStackLogging = global::ServiceStack.Logging;

namespace SpotiKat.Api.ServiceInterface.Tests {
	[TestFixture]
	public class GenreRestServiceTests {
		[Test]
		public void OnGet_RequestIsValid_CallsGenreServiceGetGenres() {
			var genreServiceFake = A.Fake<IGenreService>();

			var service = new GenreRestService {
                LogFactory = A.Fake<ServiceStackLogging.ILogFactory>(),
			                                   	GenreService = genreServiceFake
			                                   };

			var response = service.OnGet(new GenreRequest());

			A.CallTo(() => genreServiceFake.GetGenres()).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void OnGet_GenreServiceGetGenresThrowsException_ReturnsErrorResponseWithStatusCodeInternalServerError() {
			var genreServiceFake = A.Fake<IGenreService>();
			A.CallTo(() => genreServiceFake.GetGenres()).Throws(new Exception("foo"));

			var service = new GenreRestService {
                LogFactory = A.Fake<ServiceStackLogging.ILogFactory>(),
			                                   	GenreService = genreServiceFake
			                                   };

			var response = service.OnGet(new GenreRequest());

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.InternalServerError));
		}

		[Test]
		public void OnGet_GenreServiceGetGenresReturnsGenres_ReturnsGenreResponse() {
			var genreServiceFake = A.Fake<IGenreService>();
			A.CallTo(() => genreServiceFake.GetGenres()).Returns(new List<IGenre> {
			                                                                      	A.Fake<IGenre>()
			                                                                      });

			var service = new GenreRestService {
                LogFactory = A.Fake<ServiceStackLogging.ILogFactory>(),
			                                   	GenreService = genreServiceFake
			                                   };

			var response = service.OnGet(new GenreRequest());

			Assert.That(response, Is.TypeOf<GenreResponse>());
			var genreResponse = (GenreResponse)response;
			Assert.That(genreResponse.Genres.Count, Is.EqualTo(1));
		}
	}
}