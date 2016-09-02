using FakeItEasy;

using NUnit.Framework;

using ServiceStack.Logging;

using SpotiKat.Boomkat.Configuration;
using SpotiKat.Boomkat.HtmlParser;
using SpotiKat.Boomkat.Interfaces;
using SpotiKat.Configuration;

namespace SpotiKat.Boomkat.Integration.Tests {
	[TestFixture]
	[Ignore]
	public class FeedItemServiceTests {
		[Test]
		public void GetFeedItems_ReturnsFeedItemsResult() {
			var urlBuilderFake = A.Fake<IUrlBuilder>();
			A.CallTo(() => urlBuilderFake.BuildFeedItemUrl(A<int>.Ignored)).Returns("http://boomkat.com/summary/page/1");
			var boomkatConfigurationFake = A.Fake<IBoomkatConfiguration>();
			A.CallTo(() => boomkatConfigurationFake.SiteUrl).Returns("http://boomkat.com");

			var feedItemService = new FeedItemService(
				new FeedItemHtmlParser(boomkatConfigurationFake),
				new HtmlWebFactory(A.Fake<IHtmlWebConfiguration>()), 
				urlBuilderFake,
				A.Fake<ILogFactory>());

			var feedItemsResult = feedItemService.GetFeedItems(1);

			Assert.That(feedItemsResult, Is.Not.Null);
			Assert.That(feedItemsResult.Items, Is.Not.Empty);
			Assert.That(feedItemsResult.Items[0].Artist, Is.Not.Empty);
			Assert.That(feedItemsResult.Items[0].Album, Is.Not.Empty);
			Assert.That(feedItemsResult.Items[0].Url, Is.Not.Empty);
			Assert.That(feedItemsResult.Pages, Is.Not.Empty);
		}

		[Test]
		public void FeedItemsByGenre_ReturnsFeedItemsResult() {
			var urlBuilderFake = A.Fake<IUrlBuilder>();
			A.CallTo(() => urlBuilderFake.BuildFeedItemByGenreUrl(A<int>.Ignored, A<int>.Ignored)).Returns("http://boomkat.com/genres/133/summary/page/1");
			var boomkatConfigurationFake = A.Fake<IBoomkatConfiguration>();
			A.CallTo(() => boomkatConfigurationFake.SiteUrl).Returns("http://boomkat.com");

			var feedItemService = new FeedItemService(
				new FeedItemHtmlParser(boomkatConfigurationFake),
				new HtmlWebFactory(A.Fake<IHtmlWebConfiguration>()),
				urlBuilderFake,
				A.Fake<ILogFactory>());

			var feedItemsResult = feedItemService.GetFeedItemsByGenre(133, 1);

			Assert.That(feedItemsResult, Is.Not.Null);
			Assert.That(feedItemsResult.Items, Is.Not.Empty);
			Assert.That(feedItemsResult.Items[0].Artist, Is.Not.Empty);
			Assert.That(feedItemsResult.Items[0].Album, Is.Not.Empty);
			Assert.That(feedItemsResult.Items[0].Url, Is.Not.Empty);
			Assert.That(feedItemsResult.Pages, Is.Not.Empty);
		}
	}
}
