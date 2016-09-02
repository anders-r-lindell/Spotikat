using FakeItEasy;

using NUnit.Framework;
using ServiceStack;
using ServiceStack.Logging;

using SpotiKat.Configuration;
using SpotiKat.Lastfm.Interfaces;

namespace SpotiKat.Lastfm.Integration.Tests {
	[TestFixture]
	[Ignore]
	public class ArtistServiceTests {
		[Test]
		public void GetSimilarArtists_ArtistNotFound_ReturnsSimilarArtistsResponseWithEmptySimilarArtistList() {
			var urlBuilderFake = A.Fake<IUrlBuilder>();
			A.CallTo(() => urlBuilderFake.BuildArtistGetSimilatUrl(A<string>.Ignored)).Returns("http://ws.audioscrobbler.com/2.0/?method=artist.getsimilar&artist=the%20xx123123123&api_key=f32a210878b39261834c99ba82fd9e9a&format=json");
			var restClientConfigurationFake = A.Fake<IRestClientConfiguration>();
			A.CallTo(() => restClientConfigurationFake.MaxNumberOfRetries).Returns(5);
			A.CallTo(() => restClientConfigurationFake.SlowDownFactor).Returns(10);

			var artistService = new ArtistService(urlBuilderFake, new RestServiceClient(new JsonServiceClient(), restClientConfigurationFake), A.Fake<ILogFactory>());

			var similarArtistsResponse = artistService.GetSimilarArtists("the xx");

			Assert.That(similarArtistsResponse, Is.Not.Null);
			Assert.That(similarArtistsResponse.SimilarArtists, Is.Not.Null);
			Assert.That(similarArtistsResponse.SimilarArtists.Artists, Is.Empty);
		}

		[Test]
		public void GetSimilarArtists_SimilarArtistsNotFound_ReturnsSimilarArtistsResponseWithEmptySimilarArtistList() {
			var urlBuilderFake = A.Fake<IUrlBuilder>();
			A.CallTo(() => urlBuilderFake.BuildArtistGetSimilatUrl(A<string>.Ignored)).Returns("http://ws.audioscrobbler.com/2.0/?method=artist.getsimilar&artist=the%20xxxx&api_key=f32a210878b39261834c99ba82fd9e9a&format=json");
			var restClientConfigurationFake = A.Fake<IRestClientConfiguration>();
			A.CallTo(() => restClientConfigurationFake.MaxNumberOfRetries).Returns(5);
			A.CallTo(() => restClientConfigurationFake.SlowDownFactor).Returns(10);

			var artistService = new ArtistService(urlBuilderFake, new RestServiceClient(new JsonServiceClient(), restClientConfigurationFake), A.Fake<ILogFactory>());

			var similarArtistsResponse = artistService.GetSimilarArtists("the xx");

			Assert.That(similarArtistsResponse, Is.Not.Null);
			Assert.That(similarArtistsResponse.SimilarArtists, Is.Not.Null);
			Assert.That(similarArtistsResponse.SimilarArtists.Artists, Is.Empty);
		}

		[Test]
		public void GetSimilarArtists_SimilarArtistsFound_ReturnsSimilarArtistsResponseWithNotEmptySimilarArtistList() {
			var urlBuilderFake = A.Fake<IUrlBuilder>();
			A.CallTo(() => urlBuilderFake.BuildArtistGetSimilatUrl(A<string>.Ignored)).Returns("http://ws.audioscrobbler.com/2.0/?method=artist.getsimilar&artist=the%20xx&api_key=f32a210878b39261834c99ba82fd9e9a&format=json");
			var restClientConfigurationFake = A.Fake<IRestClientConfiguration>();
			A.CallTo(() => restClientConfigurationFake.MaxNumberOfRetries).Returns(5);
			A.CallTo(() => restClientConfigurationFake.SlowDownFactor).Returns(10);

			var artistService = new ArtistService(urlBuilderFake, new RestServiceClient(new JsonServiceClient(), restClientConfigurationFake), A.Fake<ILogFactory>());

			var similarArtistsResponse = artistService.GetSimilarArtists("the xx");

			Assert.That(similarArtistsResponse, Is.Not.Null);
			Assert.That(similarArtistsResponse.SimilarArtists, Is.Not.Null);
			Assert.That(similarArtistsResponse.SimilarArtists.Artists, Is.Not.Empty);
		}
	}
}
