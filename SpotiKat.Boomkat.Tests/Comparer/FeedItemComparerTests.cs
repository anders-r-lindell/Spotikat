using NUnit.Framework;

using SpotiKat.Boomkat.Comparer;
using SpotiKat.Boomkat.Entities;

namespace SpotiKat.Boomkat.Tests.Comparer {
	[TestFixture]
	public class FeedItemComparerTests {
		[Test]
		public void Equals_ObjectXIsNull_ReturnsFalse() {
			var comparer = new FeedItemComparer();

			var isEqual = comparer.Equals(null, new FeedItem());

			Assert.That(isEqual, Is.False);
		}

		[Test]
		public void Equals_ObjectYIsNull_ReturnsFalse() {
			var comparer = new FeedItemComparer();

			var isEqual = comparer.Equals(new FeedItem(), null);

			Assert.That(isEqual, Is.False);
		}

		[Test]
		public void Equals_ObjectXAndYIsNull_ReturnsFalse() {
			var comparer = new FeedItemComparer();

			var isEqual = comparer.Equals(null, null);

			Assert.That(isEqual, Is.False);
		}

		[Test]
		public void Equals_ArtistDoesNotEquals_ReturnsFalse() {
			var comparer = new FeedItemComparer();

			var isEqual = comparer.Equals(new FeedItem { Artist = "abc", Album = "def" }, new FeedItem { Artist = "ghi", Album = "def" });

			Assert.That(isEqual, Is.False);
		}

		[Test]
		public void Equals_AlbumDoesNotEquals_ReturnsFalse() {
			var comparer = new FeedItemComparer();

			var isEqual = comparer.Equals(new FeedItem { Artist = "abc", Album = "def" }, new FeedItem { Artist = "abc", Album = "ghi" });

			Assert.That(isEqual, Is.False);
		}

		[Test]
		public void Equals_ArtistAndAlbumEquals_ReturnsTrue() {
			var comparer = new FeedItemComparer();

			var isEqual = comparer.Equals(new FeedItem { Artist = "abc", Album = "def" }, new FeedItem { Artist = "abc", Album = "def" });

			Assert.That(isEqual, Is.True);
		}

		[Test]
		public void GetHashCode_ReturnsHashCode() {
			var comparer = new FeedItemComparer();

			var hashCode = comparer.GetHashCode(new FeedItem { Artist = "abc", Album = "def" });

			Assert.That(hashCode, Is.EqualTo("abc".GetHashCode() + "def".GetHashCode()));
		}
	}
}
