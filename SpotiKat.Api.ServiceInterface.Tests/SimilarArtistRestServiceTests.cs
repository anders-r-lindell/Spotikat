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
	public class SimilarArtistRestServiceTests {
		[TestCase(null)]
		[TestCase("")]
		[TestCase("  ")]
		public void OnGet_ArtistNameIsNullOrTrimEmpty_ReturnsErrorResponseWithStatusCodeBadRequest(string artistName) {
			var service = new SimilarArtistRestService() {
			                                          	LogFactory = A.Fake<ILogFactory>()
			                                          };

			var response = service.OnGet(new SimilarArtistRequest { ArtistName = artistName });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.BadRequest));
		}

		[Test]
		public void OnGet_RequestIsValid_CallsSimilarArtistServiceGetAlbums() {
			var similarArtistServiceFake = A.Fake<ISimilarArtistService>();

			var service = new SimilarArtistRestService {
			                                          	LogFactory = A.Fake<ILogFactory>(),
			                                          	SimilarArtistService = similarArtistServiceFake
			                                          };

			var response = service.OnGet(new SimilarArtistRequest { ArtistName = "abc" });

			A.CallTo(() => similarArtistServiceFake.GetArtists("abc")).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void OnGet_ArtistServiceGetSimilarArtistsThrowsException_ReturnsErrorResponseWithStatusCodeInternalServerError() {
			var similarArtistServiceFake = A.Fake<ISimilarArtistService>();
			A.CallTo(() => similarArtistServiceFake.GetArtists(A<string>.Ignored)).Throws(new Exception("foo"));

			var service = new SimilarArtistRestService {
				LogFactory = A.Fake<ILogFactory>(),
				SimilarArtistService = similarArtistServiceFake
			};

			var response = service.OnGet(new SimilarArtistRequest { ArtistName = "abc" });

			Assert.That(response, Is.TypeOf<ErrorResponse>());
			Assert.That(((ErrorResponse)response).Status.StatusCode, Is.EqualTo(StatusCode.InternalServerError));
		}

		[Test]
		public void OnGet_ArtistServiceGetSimilarArtistsReturnsArtists_ReturnsSimilarArtistResponse() {
			var similarArtistServiceFake = A.Fake<ISimilarArtistService>();
			A.CallTo(() => similarArtistServiceFake.GetArtists(A<string>.Ignored)).Returns(new List<IArtist> {
			                                                                                                 	A.Fake<IArtist>()
			                                                                                                 });

			var service = new SimilarArtistRestService {
				LogFactory = A.Fake<ILogFactory>(),
				SimilarArtistService = similarArtistServiceFake
			};

			var response = service.OnGet(new SimilarArtistRequest { ArtistName = "abc" });
	
			Assert.That(response, Is.TypeOf<SimilarArtistResponse>());
			var similarArtistResponse = (SimilarArtistResponse)response;
			Assert.That(similarArtistResponse.Info.ArtistName, Is.EqualTo("abc"));
			Assert.That(similarArtistResponse.Artists.Count, Is.EqualTo(1));
		}
	}
}