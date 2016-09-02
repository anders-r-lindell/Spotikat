using System;
using System.Collections.Generic;

using FakeItEasy;

using NUnit.Framework;

using SpotiKat.Services.Configuration;
using SpotiKat.Spotify.Entities.Search;
using SpotiKat.Spotify.Exceptions;
using SpotiKat.Spotify.Interfaces;

namespace SpotiKat.Services.Tests {
	[TestFixture]
	public class SpotifyArtistServiceTests {
		[Test]
		public void FindArtist_ArtistNameIsNull_ThrowsArgumentNullException() {
			var spotifyArtistService = new SpotifyArtistService(A.Fake<ISearchService>());

			Assert.Throws<ArgumentNullException>(() => spotifyArtistService.FindArtist(null));
		}

		[Test]
		public void FindArtist_ArtistNameIsNotNull_CallsSearchServiceArtistSearchWithArtistName() {
			var searchServiceFake = A.Fake<ISearchService>();
			var spotifyArtistService = new SpotifyArtistService(searchServiceFake);

			var artist = spotifyArtistService.FindArtist("abc");

			A.CallTo(() => searchServiceFake.ArtistSearch("abc")).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void FindArtist_ArtistSeachReturnsNull_ReturnsNull() {
			var searchServiceFake = A.Fake<ISearchService>();
			A.CallTo(() => searchServiceFake.ArtistSearch(A<string>.Ignored)).Returns(null);

			var spotifyArtistService = new SpotifyArtistService(searchServiceFake);

			var artist = spotifyArtistService.FindArtist("abc");

			Assert.That(artist, Is.Null);
		}

		[Test]
		public void FindArtist_ArtistSeachArtistsIsNull_ReturnsNull() {
			var searchServiceFake = A.Fake<ISearchService>();
			A.CallTo(() => searchServiceFake.ArtistSearch(A<string>.Ignored)).Returns(new ArtistSearch { Artists = null });

			var spotifyArtistService = new SpotifyArtistService(searchServiceFake);

			var artist = spotifyArtistService.FindArtist("abc");

			Assert.That(artist, Is.Null);
		}

		[Test]
		public void FindArtist_ArtistSeachArtistsIsEmpty_ReturnsNull() {
			var searchServiceFake = A.Fake<ISearchService>();
			A.CallTo(() => searchServiceFake.ArtistSearch(A<string>.Ignored)).Returns(new ArtistSearch { Artists = new List<Artist>() });

			var spotifyArtistService = new SpotifyArtistService(searchServiceFake);

			var artist = spotifyArtistService.FindArtist("abc");

			Assert.That(artist, Is.Null);
		}

		[Test]
		public void FindArtist_ArtistSeachNoMatchingArtist_ReturnsNull() {
			var searchServiceFake = A.Fake<ISearchService>();
			A.CallTo(() => searchServiceFake.ArtistSearch(A<string>.Ignored)).Returns(new ArtistSearch { Artists = new List<Artist>() { new Artist { Href = "jkl", Name = "mno" } } });

			var spotifyArtistService = new SpotifyArtistService(searchServiceFake);

			var artist = spotifyArtistService.FindArtist("abc");

			Assert.That(artist, Is.Null);
		}

		[Test]
		public void FindArtist_ArtistSeachMatchingArtist_ReturnsArtist() {
			var searchServiceFake = A.Fake<ISearchService>();
			A.CallTo(() => searchServiceFake.ArtistSearch(A<string>.Ignored)).Returns(new ArtistSearch { Artists = new List<Artist>() { new Artist { Href = "jkl", Name = "mno" } } });

			var spotifyArtistService = new SpotifyArtistService(searchServiceFake);

			var artist = spotifyArtistService.FindArtist("mno");

			Assert.That(artist, Is.Not.Null);
		}

		[Test]
		public void FindArtist_ArtistSeachMatchingArtistIgnoreCase_ReturnsArtist() {
			var searchServiceFake = A.Fake<ISearchService>();
			A.CallTo(() => searchServiceFake.ArtistSearch(A<string>.Ignored)).Returns(new ArtistSearch { Artists = new List<Artist>() { new Artist { Href = "jkl", Name = "MnO" } } });

			var spotifyArtistService = new SpotifyArtistService(searchServiceFake);

			var artist = spotifyArtistService.FindArtist("mno");

			Assert.That(artist, Is.Not.Null);
		}
	}
}
