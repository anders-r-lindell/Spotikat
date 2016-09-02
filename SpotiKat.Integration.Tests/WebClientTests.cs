using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using SpotiKat.Net.Http;
using FluentAssertions;
using SpotiKat.Interfaces.Logging;

namespace SpotiKat.Integration.Tests {
    [TestFixture]
    [Ignore]
    public class WebClientTests {
        [Test]
        public async Task GetAsync_ReturnsString() {
            var client = new WebClient(A.Fake<ILogFactory>()) {
                UserAgent =
                    "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.85 Safari/537.36"
            };

            var result = await client.GetAsync("http://www.dn.se");

            result.Should().NotBeEmpty();
        }
    }
}