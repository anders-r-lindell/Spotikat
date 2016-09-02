using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using SpotiKat.Interfaces.Logging;
using SpotiKat.Net.Http;
using SpotiKat.Sbwr.HtmlParser;
using SpotiKat.Sbwr.interfaces.Configuration;
using SpotiKat.Sbwr.Interfaces;

namespace SpotiKat.Sbwr.Integration.Tests {
    [TestFixture]
    [Ignore]
    public class FeedItemServiceTests {
        [Test]
        public async Task GetFeedItemsByGenreAsync_ReturnsFeedItemsResult() {
            var sbwrConfigurationFake = A.Fake<ISbwrConfiguration>();
            A.CallTo(() => sbwrConfigurationFake.WebClientUserAgent).Returns("Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.85");

            var urlBuilderFake = A.Fake<IUrlBuilder>();
            A.CallTo(() => urlBuilderFake.BuildFeedItemByGenreUrl(A<string>.Ignored, A<int>.Ignored)).Returns("http://www.soundsbetterwithreverb.com/category/shoegaze/page/3/");

            var feedItemService = new SbwrFeedItemService(new FeedItemHtmlParser(), urlBuilderFake, A.Fake<ILogFactory>(), new WebClient(A.Fake<ILogFactory>()), sbwrConfigurationFake);

            var feedItemsResult = await feedItemService.GetFeedItemsByGenreAsync("foo", 1);

            feedItemsResult.Items.Count.Should().Be(6);
        }
    }
}