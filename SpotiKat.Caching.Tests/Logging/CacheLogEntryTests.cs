using System;

using NUnit.Framework;

using SpotiKat.Caching.Logging;

namespace SpotiKat.Caching.Tests.Logging {
	[TestFixture]
	public class CacheLogEntryTests {
		[TestCase(null)]
		[TestCase("")]
		[TestCase("  ")]
		public void CacheLogEntry_ProxyIsNullOrTrimEmpty_ThrowsArgumentException(string proxy) {
			Assert.Throws<ArgumentException>(() => new CacheLogEntry(proxy, "def"));
		}

		[TestCase(null)]
		[TestCase("")]
		[TestCase("  ")]
		public void CacheLogEntry_MethodIsNullOrTrimEmpty_ThrowsArgumentException(string method) {
			Assert.Throws<ArgumentException>(() => new CacheLogEntry("abc", method));
		}

		[Test]
		public void UniqueId_ProxyAndMethodIsNotNullOrTrimEmpty_ReturnsUniqueId() {
			var entry = new CacheLogEntry("abc", "def");

			Assert.That(entry.UniqueId, Is.EqualTo("abc def"));
		}

		[Test]
		public void Created_SetInConstructor_ReturnsDateTimeGreaterThanOrEqualToDateTimeNow() {
			var now = DateTime.Now;

			var entry = new CacheLogEntry("abc", "def");

			Assert.That(entry.Created, Is.GreaterThanOrEqualTo(now));
		}

		[TestCase(0, 0, Result = 0)]
		[TestCase(0, 1, Result = 0)]
		[TestCase(1, 0, Result = 0)]
		[TestCase(1, 1, Result = 100)]
		[TestCase(2, 1, Result = 50)]
		[TestCase(3, 1, Result = 33)]
		public int CachedCallsPercentage_ReturnsCachedCallsPercentage(long calls, long cachedCalls) {
			var entry = new CacheLogEntry("abc", "def");
			entry.Calls = calls;
			entry.CachedCalls = cachedCalls;
			return entry.CachedCallsPercentage;
		}
	}

}
