using FluentAssertions;
using NUnit.Framework;

namespace SpotiKat.Spotify.Integration.Tests {
    [TestFixture]
    public class DummyTests {
        [Test]
        public void Foo() {
            var _true = true;
            _true.Should().BeTrue();
        }
    }
}