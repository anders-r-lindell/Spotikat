using System;

using FakeItEasy;

using NUnit.Framework;

using SpotiKat.Boomkat.Configuration;

namespace SpotiKat.Boomkat.Tests {
	[TestFixture]
	public class UrlBuilderTests {
		[Test]
		public void BuildFeedItemUrl_ReturnsUrl() {
			var boomkatConfigurationFake = A.Fake<IBoomkatConfiguration>();
			A.CallTo(() => boomkatConfigurationFake.FeedItemUrlFormat).Returns("http://boomkat.com/summary/page/{0}");

			var urlBuilder = new UrlBuilder(boomkatConfigurationFake);

			var url = urlBuilder.BuildFeedItemUrl(1);

			Assert.That(url, Is.EqualTo("http://boomkat.com/summary/page/1"));
		}

		[Test]
		public void BuildFeedItemByGenreUrl_ReturnsUrl() {
			var boomkatConfigurationFake = A.Fake<IBoomkatConfiguration>();
			A.CallTo(() => boomkatConfigurationFake.FeedItemByGenreUrlFormat).Returns("http://boomkat.com/genres/{1}/summary/page/{0}");

			var urlBuilder = new UrlBuilder(boomkatConfigurationFake);

			var url = urlBuilder.BuildFeedItemByGenreUrl(2, 1);

			Assert.That(url, Is.EqualTo("http://boomkat.com/genres/2/summary/page/1"));
		}
	}
}
