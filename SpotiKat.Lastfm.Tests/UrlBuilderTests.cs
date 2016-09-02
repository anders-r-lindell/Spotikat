using System;

using FakeItEasy;

using NUnit.Framework;

using SpotiKat.Lastfm.Configuration;

namespace SpotiKat.Lastfm.Tests {
	[TestFixture]
	public class UrlBuilderTests {
		[Test]
		public void BuildArtistGetSimilatUrl_ArtistIsNull_ThrowsArgumentNullException() {
			var urlBuilder = new UrlBuilder(A.Fake<ILastfmConfiguration>());

			Assert.Throws<ArgumentNullException>(() => urlBuilder.BuildArtistGetSimilatUrl(null));
		}

		[Test]
		public void BuildArtistGetSimilatUrl_ArtistIsNotNull_ReturnsArtistGetSimilatUrl() {
			var lastfmConfigurationFake = A.Fake<ILastfmConfiguration>();
			A.CallTo(() => lastfmConfigurationFake.ApiKey).Returns("abc");
			A.CallTo(() => lastfmConfigurationFake.ApiBaseUrl).Returns("http://url.com/?foo={0}&bar={1}");
			A.CallTo(() => lastfmConfigurationFake.ArtistGetSimilarMethodUrlParameter).Returns("def&qux={0}");

			var urlBuilder = new UrlBuilder(lastfmConfigurationFake);

			var url = urlBuilder.BuildArtistGetSimilatUrl("ghi");

			Assert.That(url, Is.EqualTo("http://url.com/?foo=abc&bar=def&qux=ghi"));
		}
	}

}
