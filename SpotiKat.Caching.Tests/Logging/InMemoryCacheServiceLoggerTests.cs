using NUnit.Framework;

using SpotiKat.Caching.Logging;

namespace SpotiKat.Caching.Tests.Logging {

	[TestFixture]
	public class InMemoryCacheServiceLoggerTests {
		[SetUp]
		public void Init() {
			InMemoryCacheServiceLogger.ClearLog();
		}

		[TearDown]
		public void Dispose() {
			InMemoryCacheServiceLogger.ClearLog();
		}

		[Test]
		public void Log_NoMethodCallsLogged_ReturnsEmptyList() {
			InMemoryCacheServiceLogger.ClearLog();

			var logger = new InMemoryCacheServiceLogger();

			Assert.That(InMemoryCacheServiceLogger.Log, Is.Empty);
		}

		[Test]
		public void LogMethodCall_CachedLogEntryNotFoundInLog_AddsNewCachedLogEntryToLog() {
			InMemoryCacheServiceLogger.ClearLog();

			var logger = new InMemoryCacheServiceLogger();
			var service = new FooService();
			logger.LogMethodCall(false, () => service.GetFoo());

			Assert.That(InMemoryCacheServiceLogger.Log.Count, Is.EqualTo(1));
			Assert.That(InMemoryCacheServiceLogger.Log[0].UniqueId, Is.EqualTo("SpotiKat.Caching.Tests.Logging.InMemoryCacheServiceLoggerTests LogMethodCall_CachedLogEntryNotFoundInLog_AddsNewCachedLogEntryToLog"));
			Assert.That(InMemoryCacheServiceLogger.Log[0].Calls, Is.EqualTo(1));
		}

		[Test]
		public void LogMethodCall_CachedLogEntryFoundInLog_ModifiesCachedLogEntryInLog() {
			InMemoryCacheServiceLogger.ClearLog();

			var logger = new InMemoryCacheServiceLogger();
			var service = new FooService();
			logger.LogMethodCall(false, () => service.GetFoo());
			logger.LogMethodCall(true, () => service.GetFoo());

			Assert.That(InMemoryCacheServiceLogger.Log.Count, Is.EqualTo(1));
			Assert.That(InMemoryCacheServiceLogger.Log[0].Calls, Is.EqualTo(2));
			Assert.That(InMemoryCacheServiceLogger.Log[0].CachedCalls, Is.EqualTo(1));
		}
	}

	public class FooService {
		public Foo GetFoo() {
			return new Foo();
		}
	}

	public class Foo {}
}
