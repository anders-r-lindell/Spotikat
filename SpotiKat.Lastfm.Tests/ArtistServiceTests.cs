using System;
using System.Collections.Generic;

using FakeItEasy;

using NUnit.Framework;

using ServiceStack.Logging;

using SpotiKat.Interfaces;
using SpotiKat.Lastfm.Entities;
using SpotiKat.Lastfm.Exceptions;
using SpotiKat.Lastfm.Interfaces;

namespace SpotiKat.Lastfm.Tests {
	[TestFixture]
	public class SearchServiceTests {
		[Test]
		public void GetSimilarArtists_ServiceClientThrowsException_LogsErrorAndThrowsLastfmServiceException() {
			var serviceClientFake = A.Fake<IRestServiceClient>();
			A.CallTo(() => serviceClientFake.Get<SimilarArtistsResponse>(A<string>.Ignored)).Throws(new Exception("abc"));
			var logFactoryFake = A.Fake<ILogFactory>();
			var logFake = A.Fake<ILog>();
			A.CallTo(() => logFactoryFake.GetLogger(A<Type>.Ignored)).Returns(logFake);

			var artistService = new ArtistService(A.Fake<IUrlBuilder>(), serviceClientFake, logFactoryFake);

			try {
				var similarArtistsResponse = artistService.GetSimilarArtists("def");
			} catch(Exception ex) {
				Assert.That(ex, Is.TypeOf<LastfmServiceException>());
			}

			A.CallTo(() => logFake.ErrorFormat(A<string>.Ignored, A<object[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void GetSimilarArtists_ServiceClientReturnsSimilarArtistsResponse_ReturnsSimilarArtistsResponse() {
			var serviceClientFake = A.Fake<IRestServiceClient>();
			A.CallTo(() => serviceClientFake.Get<SimilarArtistsResponse>(A<string>.Ignored)).Returns(
				new SimilarArtistsResponse {
				                           	SimilarArtists = new SimilarArtists {
				                           	                                    	Artists = new Artist[] { }
				                           	                                    }
				                           });

			var artistService = new ArtistService(A.Fake<IUrlBuilder>(), serviceClientFake, A.Fake<ILogFactory>());

			var similarArtistsResponse = artistService.GetSimilarArtists("def");

			Assert.That(similarArtistsResponse, Is.Not.Null);
		}

		[Test]
		public void GetSimilarArtists_ServiceClientReturnsSimilarArtistsResponseWithNullSimilarArtists_ReturnsSimilarArtistsResponseWithNotNullSimilarArtistsAndEmptyArtistList() {
			var serviceClientFake = A.Fake<IRestServiceClient>();
			A.CallTo(() => serviceClientFake.Get<SimilarArtistsResponse>(A<string>.Ignored)).Returns(
				new SimilarArtistsResponse {
				                           	SimilarArtists = null
				                           });

			var artistService = new ArtistService(A.Fake<IUrlBuilder>(), serviceClientFake, A.Fake<ILogFactory>());

			var similarArtistsResponse = artistService.GetSimilarArtists("def");

			Assert.That(similarArtistsResponse, Is.Not.Null);
			Assert.That(similarArtistsResponse.SimilarArtists, Is.Not.Null);
			Assert.That(similarArtistsResponse.SimilarArtists.Artists, Is.Empty);
		}

		[Test]
		public void GetSimilarArtists_ServiceClientReturnsSimilarArtistsResponseWithNullArtistsList_ReturnsSimilarArtistsResponseWithEmptyArtistList() {
			var serviceClientFake = A.Fake<IRestServiceClient>();
			A.CallTo(() => serviceClientFake.Get<SimilarArtistsResponse>(A<string>.Ignored)).Returns(
				new SimilarArtistsResponse {
				                           	SimilarArtists = new SimilarArtists {
				                           	                                    	Artists = null
				                           	                                    }
				                           });

			var artistService = new ArtistService(A.Fake<IUrlBuilder>(), serviceClientFake, A.Fake<ILogFactory>());

			var similarArtistsResponse = artistService.GetSimilarArtists("def");

			Assert.That(similarArtistsResponse, Is.Not.Null);
			Assert.That(similarArtistsResponse.SimilarArtists, Is.Not.Null);
			Assert.That(similarArtistsResponse.SimilarArtists.Artists, Is.Empty);
		}
	}
}