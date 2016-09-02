using System;

using FakeItEasy;

using NUnit.Framework;

using ServiceStack.Logging;

using SpotiKat.Abstractions.HtmlAgilityPack;
using SpotiKat.Interfaces;
using SpotiKat.Spotify.Exceptions;
using SpotiKat.Spotify.HtmlParser;
using SpotiKat.Spotify.Interfaces;

namespace SpotiKat.Spotify.Tests {
	[TestFixture]
	public class AlbumCoverServiceTests {
		[Test]
		public void GetAlbumCover_HtmlWebLoadThrowsException_LogsErrorAndThrowsSpotifyServiceException() {
			var htmlWebFake = A.Fake<IHtmlWeb>();
			A.CallTo(() => htmlWebFake.Load(A<string>.Ignored)).Throws(new Exception("foo"));
			var htmlWebFactoryFake = A.Fake<IHtmlWebFactory>();
			A.CallTo(() => htmlWebFactoryFake.Get()).Returns(htmlWebFake);
			A.CallTo(() => htmlWebFake.Load(A<string>.Ignored)).Throws(new Exception("abc"));
			var logFactoryFake = A.Fake<ILogFactory>();
			var logFake = A.Fake<ILog>();
			A.CallTo(() => logFactoryFake.GetLogger(A<Type>.Ignored)).Returns(logFake);

			var albumCoverService = new AlbumCoverService(A.Fake<IAlbumHrefIdParser>(), A.Fake<IAlbumCoverHtmlParser>(), htmlWebFactoryFake, A.Fake<IUrlBuilder>(), logFactoryFake);

			try {
				var albumCover = albumCoverService.GetAlbumCover("abc:def:ghi");
			}
			catch (Exception ex) {
				Assert.That(ex, Is.TypeOf<SpotifyServiceException>());
			}

			A.CallTo(() => logFake.ErrorFormat(A<string>.Ignored, A<object[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
		}


		[Test]
		public void GetAlbumCover_AlbumCoverHtmlParserThrowsException_LogsErrorAndThrowsSpotifyServiceException() {
			var albumCoverHtmlParserFake = A.Fake<IAlbumCoverHtmlParser>();
			A.CallTo(() => albumCoverHtmlParserFake.Parse(A<IHtmlDocument>.Ignored)).Throws(new Exception("abc"));
			var logFactoryFake = A.Fake<ILogFactory>();
			var logFake = A.Fake<ILog>();
			A.CallTo(() => logFactoryFake.GetLogger(A<Type>.Ignored)).Returns(logFake);

			var albumCoverService = new AlbumCoverService(A.Fake<IAlbumHrefIdParser>(), albumCoverHtmlParserFake, A.Fake<IHtmlWebFactory>(), A.Fake<IUrlBuilder>(), logFactoryFake);

			try {
				var albumCover = albumCoverService.GetAlbumCover("abc:def:ghi");
			} catch(Exception ex) {
				Assert.That(ex, Is.TypeOf<SpotifyServiceException>());
			}

			A.CallTo(() => logFake.ErrorFormat(A<string>.Ignored, A<object[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void GetAlbumCover_AlbumCoverHtmlParserReturnsImageUrl_ReturnsAlbumCover() {
			var albumCoverHtmlParserFake = A.Fake<IAlbumCoverHtmlParser>();
			A.CallTo(() => albumCoverHtmlParserFake.Parse(A<IHtmlDocument>.Ignored)).Returns("jkl");
			var logFactoryFake = A.Fake<ILogFactory>();
			var logFake = A.Fake<ILog>();
			A.CallTo(() => logFactoryFake.GetLogger(A<Type>.Ignored)).Returns(logFake);

			var albumCoverService = new AlbumCoverService(A.Fake<IAlbumHrefIdParser>(), albumCoverHtmlParserFake, A.Fake<IHtmlWebFactory>(), A.Fake<IUrlBuilder>(), logFactoryFake);

			var albumCover = albumCoverService.GetAlbumCover("abc:def:ghi");

			Assert.That(albumCover, Is.Not.Null);
			Assert.That(albumCover.AlbumHref, Is.EqualTo("abc:def:ghi"));
			Assert.That(albumCover.ImageUrl, Is.EqualTo("jkl"));
		}
	}
}