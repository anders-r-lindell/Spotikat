using System;

using FakeItEasy;

using NUnit.Framework;

using ServiceStack.Logging;

using SpotiKat.Interfaces;
using SpotiKat.Spotify.Entities.Lookup;
using SpotiKat.Spotify.Exceptions;
using SpotiKat.Spotify.Interfaces;

namespace SpotiKat.Spotify.Tests {
	[TestFixture]
	public class LookupServiceTests {
		[Test]
		public void ArtistLookup_ServiceClientThrowsException_LogsErrorAndThrowsSpotifyServiceException() {
			var serviceClientFake = A.Fake<IRestServiceClient>();
			A.CallTo(() => serviceClientFake.Get<ArtistInfo>(A<string>.Ignored)).Throws(new Exception("abc"));
			var logFactoryFake = A.Fake<ILogFactory>();
			var logFake = A.Fake<ILog>();
			A.CallTo(() => logFactoryFake.GetLogger(A<Type>.Ignored)).Returns(logFake);

			var searchService = new LookupService(A.Fake<IUrlBuilder>(), serviceClientFake, logFactoryFake);

			try {
				var artistSearch = searchService.ArtistLookup("def");
			} catch(Exception ex) {
				Assert.That(ex, Is.TypeOf<SpotifyServiceException>());
			}

			A.CallTo(() => logFake.ErrorFormat(A<string>.Ignored, A<object[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void ArtistLookup_ServiceClientReturnsArtist_ReturnsArtist() {
			var serviceClientFake = A.Fake<IRestServiceClient>();
			A.CallTo(() => serviceClientFake.Get<ArtistInfo>(A<string>.Ignored)).Returns(A.Fake<ArtistInfo>());

			var lookupService = new LookupService(A.Fake<IUrlBuilder>(), serviceClientFake, A.Fake<ILogFactory>());

			var artist = lookupService.ArtistLookup("def");

			Assert.That(artist, Is.Not.Null);
		}
	}
}