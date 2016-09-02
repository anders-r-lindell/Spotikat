using NUnit.Framework;

using SpotiKat.Logging;

namespace SpotiKat.Tests.Logging {
	[TestFixture]
	public class InMemoryLoggerTests {
		[SetUp]
		public void Init() {
			InMemoryLogger.ClearLog();
		}

		[TearDown]
		public void Dispose() {
			InMemoryLogger.ClearLog();
		}

		[Test]
		public void Log_NoErrorLogged_ReturnsEmptyList() {
			InMemoryLogger.ClearLog();

			var log = new InMemoryLogger();

			Assert.That(InMemoryLogger.Log, Is.Empty);
		}

		[Test]
		public void ErrorFormat_LogMaxNumberOfLogEntries_EntriesEnqueuedToLog() {
			InMemoryLogger.ClearLog();

			var log = new InMemoryLogger();

			for (var i = 0; i <= 1001; i++) {
				log.ErrorFormat("abc: {0}", i);
			}

			Assert.That(InMemoryLogger.Log.Count, Is.EqualTo(1000));
			Assert.That(InMemoryLogger.Log[0].Message, Is.EqualTo("abc: 1001"));
			Assert.That(InMemoryLogger.Log[999].Message, Is.EqualTo("abc: 2"));
		}
	}
}
