using System;
using System.Collections.Generic;

using FakeItEasy;

using NUnit.Framework;

using ServiceStack.Logging;

using SpotiKat.Abstractions.HtmlAgilityPack;
using SpotiKat.Boomkat.Entities;
using SpotiKat.Boomkat.Exceptions;
using SpotiKat.Boomkat.HtmlParser;
using SpotiKat.Boomkat.Interfaces;
using SpotiKat.Interfaces;

namespace SpotiKat.Boomkat.Tests {
	[TestFixture]
	public class FeedItemServiceTests {
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

			var feedItemService = new FeedItemService(
				A.Fake<IFeedItemHtmlParser>(),
				htmlWebFactoryFake, 
				A.Fake<IUrlBuilder>(),
				logFactoryFake);

			try {
				var feedItemsResult = feedItemService.GetFeedItems(1);
			}
			catch (Exception ex) {
				Assert.That(ex, Is.TypeOf<BoomkatServiceException>());
			}

			A.CallTo(() => logFake.ErrorFormat(A<string>.Ignored, A<object[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void GetFeedItems_FeedItemHtmlParserParseThrowsException_LogsErrorAndThrowsBoomkatServiceException() {
			var feedItemHtmlParserFake = A.Fake<IFeedItemHtmlParser>();
			var pages = (IList<string>)null;
			A.CallTo(() => feedItemHtmlParserFake.Parse(A<IHtmlDocument>.Ignored, out pages)).Throws(new Exception("foo"));
			var logFactoryFake = A.Fake<ILogFactory>();
			var logFake = A.Fake<ILog>();
			A.CallTo(() => logFactoryFake.GetLogger(A<Type>.Ignored)).Returns(logFake);

			var feedItemService = new FeedItemService(
				feedItemHtmlParserFake,
				A.Fake<IHtmlWebFactory>(),
				A.Fake<IUrlBuilder>(),
				logFactoryFake);

			try {
				var feedItemsResult = feedItemService.GetFeedItems(1);
			}
			catch (Exception ex) {
				Assert.That(ex, Is.TypeOf<BoomkatServiceException>());
			}

			A.CallTo(() => logFake.ErrorFormat(A<string>.Ignored, A<object[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void GetFeedItems_FeedItemHtmlParserParseReturnsFeedItems_ReturnsFeedItemsResultWithDistinctFeedItems() {
			var feedItemHtmlParserFake = A.Fake<IFeedItemHtmlParser>();
			var pages = (IList<string>)null;
			A.CallTo(() => feedItemHtmlParserFake.Parse(A<IHtmlDocument>.Ignored, out pages)).Returns(new List<IFeedItem> { new FeedItem { Album = "abc", Artist = "def", Url = "ghi" }, new FeedItem { Album = "abc", Artist = "def", Url = "ghi" } });

			var feedItemService = new FeedItemService(
				feedItemHtmlParserFake,
				A.Fake<IHtmlWebFactory>(),
				A.Fake<IUrlBuilder>(),
				A.Fake<ILogFactory>());

			var feedItemsResult = feedItemService.GetFeedItems(1);
			
			Assert.That(feedItemsResult, Is.Not.Null);
			Assert.That(feedItemsResult.Items.Count, Is.EqualTo(1));
		}

		[Test]
		public void GetFeedItemsByGenre_HtmlWebLoadThrowsException_LogsErrorAndThrowsBoomkatServiceException() {
			var htmlWebFake = A.Fake<IHtmlWeb>();
			A.CallTo(() => htmlWebFake.Load(A<string>.Ignored)).Throws(new Exception("foo"));
			var htmlWebFactoryFake = A.Fake<IHtmlWebFactory>();
			A.CallTo(() => htmlWebFactoryFake.Get()).Returns(htmlWebFake);
			A.CallTo(() => htmlWebFake.Load(A<string>.Ignored)).Throws(new Exception("abc"));
			var logFactoryFake = A.Fake<ILogFactory>();
			var logFake = A.Fake<ILog>();
			A.CallTo(() => logFactoryFake.GetLogger(A<Type>.Ignored)).Returns(logFake);

			var feedItemService = new FeedItemService(
				A.Fake<IFeedItemHtmlParser>(),
				htmlWebFactoryFake,
				A.Fake<IUrlBuilder>(),
				logFactoryFake);

			try {
				var feedItemsResult = feedItemService.GetFeedItemsByGenre(2, 1);
			}
			catch (Exception ex) {
				Assert.That(ex, Is.TypeOf<BoomkatServiceException>());
			}

			A.CallTo(() => logFake.ErrorFormat(A<string>.Ignored, A<object[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void GetFeedItemsByGenre_FeedItemHtmlParserParseThrowsException_LogsErrorAndThrowsBoomkatServiceException() {
			var feedItemHtmlParserFake = A.Fake<IFeedItemHtmlParser>();
			var pages = (IList<string>)null;
			A.CallTo(() => feedItemHtmlParserFake.Parse(A<IHtmlDocument>.Ignored, out pages)).Throws(new Exception("foo"));
			var logFactoryFake = A.Fake<ILogFactory>();
			var logFake = A.Fake<ILog>();
			A.CallTo(() => logFactoryFake.GetLogger(A<Type>.Ignored)).Returns(logFake);

			var feedItemService = new FeedItemService(
				feedItemHtmlParserFake,
				A.Fake<IHtmlWebFactory>(),
				A.Fake<IUrlBuilder>(),
				logFactoryFake);

			try {
				var feedItemsResult = feedItemService.GetFeedItemsByGenre(2, 1);
			}
			catch (Exception ex) {
				Assert.That(ex, Is.TypeOf<BoomkatServiceException>());
			}

			A.CallTo(() => logFake.ErrorFormat(A<string>.Ignored, A<object[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void GetFeedItemsByGenre_FeedItemHtmlParserParseReturnsFeedItems_ReturnsFeedItemsResultWithDistinctFeedItems() {
			var feedItemHtmlParserFake = A.Fake<IFeedItemHtmlParser>();
			var pages = (IList<string>)null;
			A.CallTo(() => feedItemHtmlParserFake.Parse(A<IHtmlDocument>.Ignored, out pages)).Returns(new List<IFeedItem> { new FeedItem { Album = "abc", Artist = "def", Url = "ghi" }, new FeedItem { Album = "abc", Artist = "def", Url = "ghi" } });

			var feedItemService = new FeedItemService(
				feedItemHtmlParserFake,
				A.Fake<IHtmlWebFactory>(),
				A.Fake<IUrlBuilder>(),
				A.Fake<ILogFactory>());

			var feedItemsResult = feedItemService.GetFeedItemsByGenre(2, 1);

			Assert.That(feedItemsResult, Is.Not.Null);
			Assert.That(feedItemsResult.Items.Count, Is.EqualTo(1));
		}
	}

}
