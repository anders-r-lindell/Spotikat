using System;

using FakeItEasy;

using NUnit.Framework;

using SpotiKat.Spotify.Configuration;
using SpotiKat.Spotify.Enums;
using SpotiKat.Spotify.Interfaces;

namespace SpotiKat.Spotify.Tests {
	[TestFixture]
	public class UrlBuilderTests {
		[Test]
		public void BuildArtistSearchUrl_QueryIsNull_ThrowsArgumentNullException() {
			var urlBuilder = new UrlBuilder(A.Fake<ISpotifyConfiguration>());

			Assert.Throws<ArgumentNullException>(() => urlBuilder.BuildArtistSearchUrl(null, 1));
		}

		[Test]
		public void BuildArtistSearchUrl_QueryIsNotNull_ReturnsUrl() {
			var spotifyConfigurationFake = A.Fake<ISpotifyConfiguration>();
			A.CallTo(() => spotifyConfigurationFake.SearchServiceUrlFormat).Returns("http://ws.spotify.com/search/1/{0}.json?q={1}&page={2}");

			var urlBuilder = new UrlBuilder(spotifyConfigurationFake);

			var url = urlBuilder.BuildArtistSearchUrl("abc", 1);

			Assert.That(url, Is.EqualTo("http://ws.spotify.com/search/1/artist.json?q=abc&page=1"));
		}

		[Test]
		public void BuildArtistLookupUrl_HrefIsNull_ThrowsArgumentNullException() {
			var urlBuilder = new UrlBuilder(A.Fake<ISpotifyConfiguration>());

			Assert.Throws<ArgumentNullException>(() => urlBuilder.BuildArtistLookupUrl(null, ArtistLookupExtras.Album));
		}

		[Test]
		public void BuildArtistLookupUrl_HrefIsNotNull_ReturnsUrl() {
			var spotifyConfigurationFake = A.Fake<ISpotifyConfiguration>();
			A.CallTo(() => spotifyConfigurationFake.LookupServiceUrlFormat).Returns("http://ws.spotify.com/lookup/1/.json?uri={0}&extras={1}");

			var urlBuilder = new UrlBuilder(spotifyConfigurationFake);

			var url = urlBuilder.BuildArtistLookupUrl("abc", ArtistLookupExtras.None);

			Assert.That(url, Is.EqualTo("http://ws.spotify.com/lookup/1/.json?uri=abc&extras="));
		}

		[Test]
		public void BuildArtistLookupUrl_ExtrasIsAlbum_ReturnsUrlWithAlbumExtrasParameter() {
			var spotifyConfigurationFake = A.Fake<ISpotifyConfiguration>();
			A.CallTo(() => spotifyConfigurationFake.LookupServiceUrlFormat).Returns("http://ws.spotify.com/lookup/1/.json?uri={0}&extras={1}");

			var urlBuilder = new UrlBuilder(spotifyConfigurationFake);

			var url = urlBuilder.BuildArtistLookupUrl("abc", ArtistLookupExtras.Album);

			Assert.That(url, Is.EqualTo("http://ws.spotify.com/lookup/1/.json?uri=abc&extras=album"));
		}

		[Test]
		public void BuildArtistLookupUrl_ExtrasIsAlbumDetail_ReturnsUrlWithAlbumDetailExtrasParameter() {
			var spotifyConfigurationFake = A.Fake<ISpotifyConfiguration>();
			A.CallTo(() => spotifyConfigurationFake.LookupServiceUrlFormat).Returns("http://ws.spotify.com/lookup/1/.json?uri={0}&extras={1}");

			var urlBuilder = new UrlBuilder(spotifyConfigurationFake);

			var url = urlBuilder.BuildArtistLookupUrl("abc", ArtistLookupExtras.AlbumDetail);

			Assert.That(url, Is.EqualTo("http://ws.spotify.com/lookup/1/.json?uri=abc&extras=albumdetail"));
		}

		[Test]
		public void BuildArtistLookupUrl_ExtrasIsNone_ReturnsUrlWithEmptyExtrasParameter() {
			var spotifyConfigurationFake = A.Fake<ISpotifyConfiguration>();
			A.CallTo(() => spotifyConfigurationFake.LookupServiceUrlFormat).Returns("http://ws.spotify.com/lookup/1/.json?uri={0}&extras={1}");

			var urlBuilder = new UrlBuilder(spotifyConfigurationFake);

			var url = urlBuilder.BuildArtistLookupUrl("abc", ArtistLookupExtras.None);

			Assert.That(url, Is.EqualTo("http://ws.spotify.com/lookup/1/.json?uri=abc&extras="));
		}
	}

}
