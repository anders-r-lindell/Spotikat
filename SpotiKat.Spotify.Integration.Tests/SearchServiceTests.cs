using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using SpotiKat.Interfaces.Configuration;
using SpotiKat.Interfaces.Logging;
using SpotiKat.Net.Http;
using SpotiKat.Spotify.Interfaces;

namespace SpotiKat.Spotify.Integration.Tests {
    [TestFixture]
    [Ignore]
    public class SearchServiceTests {
        [Test]
        public async Task AlbumSearchAsync_AlbumFound_ReturnsAlbumSearchResultWithNotEmptyAlbumList() {
            var urlBuilderFake = A.Fake<IUrlBuilder>();
            A.CallTo(() => urlBuilderFake.BuildAlbumSearchUrl(A<string>.Ignored, A<string>.Ignored))
                .Returns(
                    "https://api.spotify.com/v1/search?q=artist:Helena+Hauff+AND+album:Discreet+Desires&type=album&limit=50");
            var restClientConfigurationFake = A.Fake<IJsonServiceClientConfiguration>();
            A.CallTo(() => restClientConfigurationFake.MaxNumberOfRetries).Returns(5);
            A.CallTo(() => restClientConfigurationFake.SlowDownFactor).Returns(10);
            var searchService = new SearchService(urlBuilderFake,
                new JsonServiceClient(new WebClient(A.Fake<ILogFactory>()), restClientConfigurationFake, A.Fake<ILogFactory>()),
                A.Fake<ILogFactory>());

            var searchResult = await searchService.AlbumSearchAsync("foo", "bar");

            searchResult.Should().NotBeNull();
            searchResult.Albums.Total.Should().Be(2);
            searchResult.Albums.Items.Count.Should().Be(2);
            searchResult.Albums.Items[0].Id.Should().Be("0bQyK8wR0FFKE0rJyQH9pQ");
            searchResult.Albums.Items[0].Type.Should().Be("album");
            searchResult.Albums.Items[0].Uri.Should().Be("spotify:album:0bQyK8wR0FFKE0rJyQH9pQ");
            searchResult.Albums.Items[0].Name.Should().Be("Discreet Desires");
            searchResult.Albums.Items[0].AvailableMarkets.Count.Should().BeGreaterThan(0);
            searchResult.Albums.Items[0].Type.Count().Should().BeGreaterThan(0);
            searchResult.Albums.Items[0].Images.Count().Should().Be(3);
            searchResult.Albums.Items[0].Images[0].Url.Should()
                .Be("https://i.scdn.co/image/19b59c2c0dfd17ce8127dcc8228e484acdd8fc46");
            searchResult.Albums.Items[0].Images[0].Width.Should().Be(640);
            searchResult.Albums.Items[0].Images[0].Height.Should().Be(640);
        }

        [Test]
        public async Task AlbumSearchAsync_AlbumNotFound_ReturnsAlbumSearchResultWithEmptyAlbumList() {
            var urlBuilderFake = A.Fake<IUrlBuilder>();
            A.CallTo(() => urlBuilderFake.BuildAlbumSearchUrl(A<string>.Ignored, A<string>.Ignored))
                .Returns(
                    "https://api.spotify.com/v1/search?q=artist:asdoieroijdsfoisdjfoi+AND+album:wqireuoaisfdoaisoisahf&type=album&limit=50");
            var restClientConfigurationFake = A.Fake<IJsonServiceClientConfiguration>();
            A.CallTo(() => restClientConfigurationFake.MaxNumberOfRetries).Returns(5);
            A.CallTo(() => restClientConfigurationFake.SlowDownFactor).Returns(10);
            var searchService = new SearchService(urlBuilderFake,
                new JsonServiceClient(new WebClient(A.Fake<ILogFactory>()), restClientConfigurationFake, A.Fake<ILogFactory>()),
                A.Fake<ILogFactory>());

            var searchResult = await searchService.AlbumSearchAsync("foo", "bar");

            searchResult.Should().NotBeNull();
            searchResult.Albums.Total.Should().Be(0);
            searchResult.Albums.Items.Count.Should().Be(0);
        }
    }
}