using System;

using FakeItEasy;

using NUnit.Framework;

using ServiceStack.Logging;

using SpotiKat.Interfaces;
using SpotiKat.Spotify.Entities.Search;
using SpotiKat.Spotify.Exceptions;
using SpotiKat.Spotify.Interfaces;

namespace SpotiKat.Spotify.Tests {
	[TestFixture]
	public class SearchServiceTests {
		[Test]
		public void ArtistSearch_ServiceClientThrowsException_LogsErrorAndThrowsSpotifyServiceException() {
			var serviceClientFake = A.Fake<IRestServiceClient>();
			A.CallTo(() => serviceClientFake.Get<ArtistSearch>(A<string>.Ignored)).Throws(new Exception("abc"));
			var logFactoryFake = A.Fake<ILogFactory>();
			var logFake = A.Fake<ILog>();
			A.CallTo(() => logFactoryFake.GetLogger(A<Type>.Ignored)).Returns(logFake);

			var searchService = new SearchService(A.Fake<IUrlBuilder>(), serviceClientFake, logFactoryFake);

			try {
				var artistSearch = searchService.ArtistSearch("def");
			} catch(Exception ex) {
				Assert.That(ex, Is.TypeOf<SpotifyServiceException>());
			}

			A.CallTo(() => logFake.ErrorFormat(A<string>.Ignored, A<object[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void ArtistSearch_ServiceClientReturnsArtistSearch_ReturnsArtistSearch() {
			var serviceClientFake = A.Fake<IRestServiceClient>();
			A.CallTo(() => serviceClientFake.Get<ArtistSearch>(A<string>.Ignored)).Returns(A.Fake<ArtistSearch>());

			var searchService = new SearchService(A.Fake<IUrlBuilder>(), serviceClientFake, A.Fake<ILogFactory>());

			var artistSearch = searchService.ArtistSearch("def");

			Assert.That(artistSearch, Is.Not.Null);
		}
	}
}