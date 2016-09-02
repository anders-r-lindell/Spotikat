using System;

using FakeItEasy;

using NUnit.Framework;

using SpotiKat.Spotify.Configuration;

namespace SpotiKat.Spotify.Tests {
	[TestFixture]
	public class AlbumHrefIdParserTests {
		[Test]
		public void Parse_HrefIsNull_ThrowsArgumentNullException() {
			var hrefIdParser = new AlbumHrefIdParser(A.Fake<ISpotifyConfiguration>());

			Assert.Throws<ArgumentNullException>(() => hrefIdParser.Parse(null));
		}

		[Test]
		public void Parse_HrefFormatNotSupported_ThrowsNotSupportedException() {
			var spotifyConfigurationFake = A.Fake<ISpotifyConfiguration>();
			A.CallTo(() => spotifyConfigurationFake.AlbumHrefFormat).Returns("abc:def:{0}");

			var hrefIdParser = new AlbumHrefIdParser(spotifyConfigurationFake);

			Assert.Throws<NotSupportedException>(() => hrefIdParser.Parse("abc"));
		}

		[Test]
		public void Parse_HrefFormatSupported_ReturnsId() {
			var spotifyConfigurationFake = A.Fake<ISpotifyConfiguration>();
			A.CallTo(() => spotifyConfigurationFake.AlbumHrefFormat).Returns("abc:def:{0}");

			var hrefIdParser = new AlbumHrefIdParser(spotifyConfigurationFake);

			var albumId = hrefIdParser.Parse("abc:def:ghi");

			Assert.That(albumId, Is.EqualTo("ghi"));
		}
	}

}
