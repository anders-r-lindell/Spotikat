using NUnit.Framework;

using SpotiKat.Logging;

namespace SpotiKat.Tests.Logging {
	[TestFixture]
	public class InMemoryLoggerlogEntryReaderTests {
		[SetUp]
		public void Init() {
			InMemoryLogger.ClearLog();
		}

		[TearDown]
		public void Dispose() {
			InMemoryLogger.ClearLog();
		}

		[Test]
		public void GetLogEntries_InMemoryLogIsEmpty_ReturnsEmptyResult() {
			InMemoryLogger.ClearLog();

			var logEntryReader = new InMemoryLoggerLogEntryReader();

			var result = logEntryReader.GetLogEntries(1, 10);

			Assert.That(result.PageCount, Is.EqualTo(0));
			Assert.That(result.LogEntries, Is.Empty);
		}

		[Test]
		public void GetLogEntries_InMemoryLogIsNotEmpty_ReturnsNotEmptyResult() {
			InMemoryLogger.ClearLog();

			var logger = new InMemoryLogger();
			logger.ErrorFormat("abc {0}", 1);

			var logEntryReader = new InMemoryLoggerLogEntryReader();

			var result = logEntryReader.GetLogEntries(1, 10);

			Assert.That(result.PageCount, Is.EqualTo(1));
			Assert.That(result.LogEntries, Is.Not.Empty);
		}

		[TestCase(10, 0, Result = 0)]
		[TestCase(10, 1, Result = 1)]
		[TestCase(10, 9, Result = 1)]
		[TestCase(10, 10, Result = 1)]
		[TestCase(10, 11, Result = 2)]
		[TestCase(10, 19, Result = 2)]
		[TestCase(10, 20, Result = 2)]
		[TestCase(10, 21, Result = 3)]
		public int GetLogEntries_SetsPageCount(int pageSize, int noOfLogEntries) {
			InMemoryLogger.ClearLog();

			var logger = new InMemoryLogger();
			for (var i = 0; i < noOfLogEntries; i++) {
				logger.ErrorFormat("abc {0}", i);
			}

			var result = new InMemoryLoggerLogEntryReader().GetLogEntries(1, pageSize);

			return result.PageCount;
		}

		[TestCase(10, 0, Result = 0)]
		[TestCase(10, 1, Result = 1)]
		[TestCase(10, 9, Result = 9)]
		[TestCase(10, 10, Result = 10)]
		[TestCase(10, 11, Result = 10)]
		[TestCase(10, 19, Result = 10)]
		[TestCase(20, 20, Result = 20)]
		[TestCase(20, 21, Result = 20)]
		public int GetLogEntries_SetsLogEntries(int pageSize, int noOfLogEntries) {
			InMemoryLogger.ClearLog();

			var logger = new InMemoryLogger();
			for (var i = 0; i < noOfLogEntries; i++) {
				logger.ErrorFormat("abc {0}", i);
			}

			var result = new InMemoryLoggerLogEntryReader().GetLogEntries(1, pageSize);

			return result.LogEntries.Count;
		}
	}
}
