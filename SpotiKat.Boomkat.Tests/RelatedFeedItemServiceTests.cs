using System;
using System.Collections.Generic;

using FakeItEasy;

using NUnit.Framework;

using ServiceStack.Logging;

using SpotiKat.Abstractions.HtmlAgilityPack;
using SpotiKat.Boomkat.Configuration;
using SpotiKat.Boomkat.Entities;
using SpotiKat.Boomkat.Exceptions;
using SpotiKat.Boomkat.HtmlParser;
using SpotiKat.Boomkat.Interfaces;
using SpotiKat.Interfaces;

namespace SpotiKat.Boomkat.Tests {
	[TestFixture]
	public class RelatedFeedItemServiceTests {
		[Test]
		public void GetFeedItems_UrlIsNull_ThrowsArgumentNullException() {
			var relatedFeedItemService = new RelatedFeedItemService(
				A.Fake<IRelatedFeedItemHtmlParser>(),
				A.Fake<IHtmlWebFactory>(),
				A.Fake<ILogFactory>());

			Assert.Throws<ArgumentNullException>(() => relatedFeedItemService.GetFeedItems(null));
		}

		[Test]
		public void GetFeedItems_HtmlWebLoadThrowsException_LogsErrorAndThrowsBoomkatServiceException() {
			var htmlWebFake = A.Fake<IHtmlWeb>();
			A.CallTo(() => htmlWebFake.Load(A<string>.Ignored)).Throws(new Exception("foo"));
			var htmlWebFactoryFake = A.Fake<IHtmlWebFactory>();
			A.CallTo(() => htmlWebFactoryFake.Get()).Returns(htmlWebFake);
			A.CallTo(() => htmlWebFake.Load(A<string>.Ignored)).Throws(new Exception("abc"));
			var logFactoryFake = A.Fake<ILogFactory>();
			var logFake = A.Fake<ILog>();
			A.CallTo(() => logFactoryFake.GetLogger(A<Type>.Ignored)).Returns(logFake);

			var relatedFeedItemService = new RelatedFeedItemService(
				A.Fake<IRelatedFeedItemHtmlParser>(),
				htmlWebFactoryFake,
				logFactoryFake);

			try {
				var feedItemsResult = relatedFeedItemService.GetFeedItems("abc");
			} catch(Exception ex) {
				Assert.That(ex, Is.TypeOf<BoomkatServiceException>());
			}

			A.CallTo(() => logFake.ErrorFormat(A<string>.Ignored, A<object[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void GetFeedItems_RelatedFeedItemHtmlParserParseThrowsException_LogsErrorAndThrowsBoomkatServiceException() {
			var relatedFeedItemHtmlParserFake = A.Fake<IRelatedFeedItemHtmlParser>();
			var pages = (IList<string>)null;
			A.CallTo(() => relatedFeedItemHtmlParserFake.Parse(A<IHtmlDocument>.Ignored)).Throws(new Exception("foo"));
			var logFactoryFake = A.Fake<ILogFactory>();
			var logFake = A.Fake<ILog>();
			A.CallTo(() => logFactoryFake.GetLogger(A<Type>.Ignored)).Returns(logFake);

			var relatedFeedItemService = new RelatedFeedItemService(
				relatedFeedItemHtmlParserFake,
				A.Fake<IHtmlWebFactory>(),
				logFactoryFake);

			try {
				var feedItemsResult = relatedFeedItemService.GetFeedItems("abc");
			} catch(Exception ex) {
				Assert.That(ex, Is.TypeOf<BoomkatServiceException>());
			}

			A.CallTo(() => logFake.ErrorFormat(A<string>.Ignored, A<object[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void GetFeedItems_RelatedFeedItemHtmlParserParseReturnsFeedItems_ReturnsDistinctFeedItems() {
			var relatedFeedItemHtmlParserFake = A.Fake<IRelatedFeedItemHtmlParser>();
			var pages = (IList<string>)null;
			A.CallTo(() => relatedFeedItemHtmlParserFake.Parse(A<IHtmlDocument>.Ignored)).Returns(new List<IFeedItem> { new FeedItem { Album = "abc", Artist = "def", Url = "ghi" }, new FeedItem { Album = "abc", Artist = "def", Url = "ghi" } });

			var relatedFeedItemService = new RelatedFeedItemService(
				relatedFeedItemHtmlParserFake,
				A.Fake<IHtmlWebFactory>(),
				A.Fake<ILogFactory>());

			var feedItems = relatedFeedItemService.GetFeedItems("abc");

			Assert.That(feedItems.Count, Is.EqualTo(1));
		}
	}
}