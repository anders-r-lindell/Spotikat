using FakeItEasy;

using NUnit.Framework;

using ServiceStack.Logging;

using SpotiKat.Boomkat.Configuration;
using SpotiKat.Boomkat.HtmlParser;
using SpotiKat.Configuration;

namespace SpotiKat.Boomkat.Integration.Tests {
	[TestFixture]
	[Ignore]
	public class RelatedFeedItemServiceTests {
		[Test]
		public void GetFeedItems_ReturnsFeedItems() {
			var boomkatConfigurationFake = A.Fake<IBoomkatConfiguration>();
			A.CallTo(() => boomkatConfigurationFake.SiteUrl).Returns("http://boomkat.com");

			var relatedFeedItemService = new RelatedFeedItemService(
				new RelatedFeedItemHtmlParser(boomkatConfigurationFake), 
				new HtmlWebFactory(A.Fake<IHtmlWebConfiguration>()),
				A.Fake<ILogFactory>());

			var feedItems = relatedFeedItemService.GetFeedItems("http://boomkat.com/downloads/568054-the-xx-coexist");

			Assert.That(feedItems, Is.Not.Empty);
			Assert.That(feedItems[0].Artist, Is.Not.Empty);
			Assert.That(feedItems[0].Album, Is.Not.Empty);
			Assert.That(feedItems[0].Url, Is.Empty);
		}
	}
}
