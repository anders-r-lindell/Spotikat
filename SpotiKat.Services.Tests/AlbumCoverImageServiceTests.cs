using System;

using FakeItEasy;

using NUnit.Framework;

using SpotiKat.Services.Configuration;
using SpotiKat.Spotify.Configuration;
using SpotiKat.Spotify.Interfaces;

namespace SpotiKat.Services.Tests {
	[TestFixture]
	public class AlbumCoverImageServiceTests {
		[Test]
		public void GetVirtualUrl_AlbumHrefIsNull_ThrowsArgumentNullException() {
			var service = new AlbumCoverImageService(
				A.Fake<IAlbumHrefIdParser>(), 
				A.Fake<IAlbumCoverImageConfiguration>(), 
				A.Fake<ISpotifyConfiguration>());

			Assert.Throws<ArgumentNullException>(() => service.GetUrl(null));
		}

		[Test]
		public void GetVirtualUrl_HrefIdParserReturnsId_ReturnsUrlThatContainsParsedId() {
			var albumHrefIdParser = A.Fake<IAlbumHrefIdParser>();
			A.CallTo(() => albumHrefIdParser.Parse(A<string>.Ignored)).Returns("abc");
			var albumCoverImageConfigurationFake = A.Fake<IAlbumCoverImageConfiguration>();
			A.CallTo(() => albumCoverImageConfigurationFake.VirtualUrlFormat).Returns("/def/ghi/{0}.jkl");

			var service = new AlbumCoverImageService(
				albumHrefIdParser,
				albumCoverImageConfigurationFake,
				A.Fake<ISpotifyConfiguration>());

			var url = service.GetVirtualUrl("jkl:mno:abc");

			Assert.That(url, Is.EqualTo("/def/ghi/abc.jkl"));
		}

		[Test]
		public void GetAlbumHref_VirtualUrlIsNull_ThrowsArgumentNullException() {
			var service = new AlbumCoverImageService(
				A.Fake<IAlbumHrefIdParser>(),
				A.Fake<IAlbumCoverImageConfiguration>(),
				A.Fake<ISpotifyConfiguration>());

			Assert.Throws<ArgumentNullException>(() => service.GetAlbumHref(null));
		}

		[Test]
		public void GetAlbumHref_VirtualUrlSegmentsDoesNotMatchFormat_ThrowsNotSupportedException() {
			var albumHrefIdParser = A.Fake<IAlbumHrefIdParser>();
			A.CallTo(() => albumHrefIdParser.Parse("qwe:rty:uio")).Returns("uio");
			var albumCoverImageConfigurationFake = A.Fake<IAlbumCoverImageConfiguration>();
			A.CallTo(() => albumCoverImageConfigurationFake.VirtualUrlFormat).Returns("/abc/def/{0}.jkl");
			A.CallTo(() => albumCoverImageConfigurationFake.VirtualUrlFileExtension).Returns(".jkl");
			A.CallTo(() => albumCoverImageConfigurationFake.UrlFormat).Returns("/mno/{0}.stu");
			var spotifyConfigurationFake = A.Fake<ISpotifyConfiguration>();
			A.CallTo(() => spotifyConfigurationFake.AlbumHrefFormat).Returns("qwe:rty:{0}");

			var service = new AlbumCoverImageService(
				albumHrefIdParser,
				albumCoverImageConfigurationFake,
				spotifyConfigurationFake);

			Assert.Throws<NotSupportedException>(() => service.GetAlbumHref("/def/uio.jkl"));
			Assert.Throws<NotSupportedException>(() => service.GetAlbumHref("/qwe/def/uio.jkl"));
		}

		[Test]
		public void GetAlbumHref_VirtualUrlFileExtensionDoesNotMatchFormat_ThrowsNotSupportedException() {
			var albumHrefIdParser = A.Fake<IAlbumHrefIdParser>();
			A.CallTo(() => albumHrefIdParser.Parse("qwe:rty:uio")).Returns("uio");
			var albumCoverImageConfigurationFake = A.Fake<IAlbumCoverImageConfiguration>();
			A.CallTo(() => albumCoverImageConfigurationFake.VirtualUrlFormat).Returns("/abc/def/{0}.jkl");
			A.CallTo(() => albumCoverImageConfigurationFake.VirtualUrlFileExtension).Returns(".jkl");
			A.CallTo(() => albumCoverImageConfigurationFake.UrlFormat).Returns("/mno/{0}.stu");
			var spotifyConfigurationFake = A.Fake<ISpotifyConfiguration>();
			A.CallTo(() => spotifyConfigurationFake.AlbumHrefFormat).Returns("qwe:rty:{0}");

			var service = new AlbumCoverImageService(
				albumHrefIdParser,
				albumCoverImageConfigurationFake,
				spotifyConfigurationFake);

			Assert.Throws<NotSupportedException>(() => service.GetAlbumHref("/abc/def/uio.wer"));
		}

		[Test]
		public void GetAlbumHref_VirtualUrlDoesMatchFormat_ReturnsAlbumHref() {
			var albumHrefIdParser = A.Fake<IAlbumHrefIdParser>();
			A.CallTo(() => albumHrefIdParser.Parse("qwe:rty:uio")).Returns("uio");
			var albumCoverImageConfigurationFake = A.Fake<IAlbumCoverImageConfiguration>();
			A.CallTo(() => albumCoverImageConfigurationFake.VirtualUrlFormat).Returns("/abc/def/{0}.jkl");
			A.CallTo(() => albumCoverImageConfigurationFake.VirtualUrlFileExtension).Returns(".jkl");
			A.CallTo(() => albumCoverImageConfigurationFake.UrlFormat).Returns("/mno/{0}.stu");
			var spotifyConfigurationFake = A.Fake<ISpotifyConfiguration>();
			A.CallTo(() => spotifyConfigurationFake.AlbumHrefFormat).Returns("qwe:rty:{0}");

			var service = new AlbumCoverImageService(
				albumHrefIdParser,
				albumCoverImageConfigurationFake,
				spotifyConfigurationFake);

			var albumHref = service.GetAlbumHref("/abc/def/uio.jkl");

			Assert.That(albumHref, Is.EqualTo("qwe:rty:uio"));
		}

		[Test]
		public void GetUrl_AlbumIdIsNull_ThrowsArgumentNullException() {
			var service = new AlbumCoverImageService(
				A.Fake<IAlbumHrefIdParser>(),
				A.Fake<IAlbumCoverImageConfiguration>(),
				A.Fake<ISpotifyConfiguration>());

			Assert.Throws<ArgumentNullException>(() => service.GetUrl(null));
		}

		[Test]
		public void GetUrl_AlbumIdIsNotNull_ReturnsUrlThatContainsAlbumId() {
			var albumCoverImageConfigurationFake = A.Fake<IAlbumCoverImageConfiguration>();
			A.CallTo(() => albumCoverImageConfigurationFake.UrlFormat).Returns("/def/ghi/{0}.jkl");

			var service = new AlbumCoverImageService(
				A.Fake<IAlbumHrefIdParser>(),
				albumCoverImageConfigurationFake,
				A.Fake<ISpotifyConfiguration>());

			var url = service.GetUrl("zxc");

			Assert.That(url, Is.EqualTo("/def/ghi/zxc.jkl"));
		}
	}
}