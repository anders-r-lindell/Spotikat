using System;
using System.Collections.Generic;
using System.Web.Caching;

using FakeItEasy;

using NUnit.Framework;

using SpotiKat.Caching.Logging;

namespace SpotiKat.Caching.Tests {
	[TestFixture]
	public class CacheServiceTests {
		[Test]
		public void Get_CachedNullValueFoundInCache_ReturnsNull() {
			const string cacheKey = "abc";

			var cacheFake = A.Fake<ICache>();
			A.CallTo(() => cacheFake.Get(cacheKey)).Returns(new CachedNullValue());

			var cacheManager = new CacheService(cacheFake, A.Fake<ICacheServiceLogger>());
			var obj = cacheManager.Get(cacheKey, new TimeSpan(0, 10, 0), true, () => new CachedNullValue());

			Assert.That(obj, Is.Null);
		}

		[Test]
		public void Get_ObjectFoundInCache_ReturnsObject() {
			const string cacheKey = "abc";

			var cachedObj = new object();

			var cacheFake = A.Fake<ICache>();
			A.CallTo(() => cacheFake.Get(cacheKey)).Returns(cachedObj);

			var cacheManager = new CacheService(cacheFake, A.Fake<ICacheServiceLogger>());
			var obj = cacheManager.Get(cacheKey, new TimeSpan(0, 10, 0), true, () => new object());

			Assert.That(obj, Is.SameAs(cachedObj));
		}

		[Test]
		public void Get_ObjectNotFoundInCache_AddsObjectToCacheAndReturnsObject() {
			const string cacheKey = "abc";

			var objectToAdd = new object();

			var cacheFake = A.Fake<ICache>();
			A.CallTo(() => cacheFake.Get(cacheKey)).Returns((object)null);

			var cacheManager = new CacheService(cacheFake, A.Fake<ICacheServiceLogger>());
			var obj = cacheManager.Get(cacheKey, new TimeSpan(0, 10, 0), true, () => objectToAdd);

			A.CallTo(() => cacheFake.Add(cacheKey, objectToAdd, A<DateTime>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

			Assert.That(obj, Is.SameAs(objectToAdd));
		}

		[Test]
		public void Get_ObjectNotFoundInCache_AddsCachedNullValueToCacheAndReturnsNull() {
			const string cacheKey = "abc";

			var objectToAdd = new CachedNullValue();

			var cacheFake = A.Fake<ICache>();
			A.CallTo(() => cacheFake.Get(cacheKey)).Returns((object)null);

			var cacheManager = new CacheService(cacheFake, A.Fake<ICacheServiceLogger>());
			var obj = cacheManager.Get(cacheKey, new TimeSpan(0, 10, 0), true, () => objectToAdd);

			A.CallTo(() => cacheFake.Add(cacheKey, objectToAdd, A<DateTime>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

			Assert.That(obj, Is.Null);
		}

		[Test]
		public void Get_ObjectFoundInCache_LogsFoundInCacheMethodCall() {
			const string cacheKey = "abc";

			var cachedObj = new object();

			var cacheFake = A.Fake<ICache>();
			A.CallTo(() => cacheFake.Get(cacheKey)).Returns(cachedObj);
			var cacheServiceLoggerFake = A.Fake<ICacheServiceLogger>();

			var cacheManager = new CacheService(cacheFake, cacheServiceLoggerFake);
			Func<object> delegateFunction = () => new object();
			var obj = cacheManager.Get(cacheKey, new TimeSpan(0, 10, 0), true, delegateFunction);

			A.CallTo(() => cacheServiceLoggerFake.LogMethodCall(true, delegateFunction)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Get_ObjectNotFoundInCache_LogsNotFoundInCacheMethodCall() {
			const string cacheKey = "abc";

			var objectToAdd = new CachedNullValue();

			var cacheFake = A.Fake<ICache>();
			A.CallTo(() => cacheFake.Get(cacheKey)).Returns((object)null);
			var cacheServiceLoggerFake = A.Fake<ICacheServiceLogger>();

			var cacheManager = new CacheService(cacheFake, cacheServiceLoggerFake);
			Func<CachedNullValue> delegateFunction = () => objectToAdd;
			var obj = cacheManager.Get(cacheKey, new TimeSpan(0, 10, 0), true, delegateFunction);

			A.CallTo(() => cacheServiceLoggerFake.LogMethodCall(false, delegateFunction)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Get_CacheNullAndEmptyCollectionIsFalse_DoesNotAddsCachedNullValueToCacheAndReturnsNull() {
			const string cacheKey = "abc";

			var objectToAdd = new CachedNullValue();

			var cacheFake = A.Fake<ICache>();
			A.CallTo(() => cacheFake.Get(cacheKey)).Returns((object)null);

			var cacheManager = new CacheService(cacheFake, A.Fake<ICacheServiceLogger>());
			var obj = cacheManager.Get(cacheKey, new TimeSpan(0, 10, 0), false, () => objectToAdd);

			A.CallTo(() => cacheFake.Add(cacheKey, objectToAdd, A<DateTime>.Ignored)).MustNotHaveHappened();
		}

		[Test]
		public void Get_CacheNullAndEmptyCollectionIsTrue_AddsCachedNullValueToCacheAndReturnsNull() {
			const string cacheKey = "abc";

			var objectToAdd = new CachedNullValue();

			var cacheFake = A.Fake<ICache>();
			A.CallTo(() => cacheFake.Get(cacheKey)).Returns((object)null);

			var cacheManager = new CacheService(cacheFake, A.Fake<ICacheServiceLogger>());
			var obj = cacheManager.Get(cacheKey, new TimeSpan(0, 10, 0), true, () => objectToAdd);

			A.CallTo(() => cacheFake.Add(cacheKey, objectToAdd, A<DateTime>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Get_CacheNullAndEmptyCollectionIsFalse_AddsNotNullValueToCacheAndReturnsNull() {
			const string cacheKey = "abc";

			var objectToAdd = new object();

			var cacheFake = A.Fake<ICache>();
			A.CallTo(() => cacheFake.Get(cacheKey)).Returns((object)null);

			var cacheManager = new CacheService(cacheFake, A.Fake<ICacheServiceLogger>());
			var obj = cacheManager.Get(cacheKey, new TimeSpan(0, 10, 0), false, () => objectToAdd);

			A.CallTo(() => cacheFake.Add(cacheKey, objectToAdd, A<DateTime>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Get_CacheNullAndEmptyCollectionIsFalse_DoesNotAddsEmptyCollectionToCacheAndReturnsEmptyCollection() {
			const string cacheKey = "abc";

			var objectToAdd = new string[] {};

			var cacheFake = A.Fake<ICache>();
			A.CallTo(() => cacheFake.Get(cacheKey)).Returns((object)null);

			var cacheManager = new CacheService(cacheFake, A.Fake<ICacheServiceLogger>());
			var obj = cacheManager.Get(cacheKey, new TimeSpan(0, 10, 0), false, () => objectToAdd);

			A.CallTo(() => cacheFake.Add(cacheKey, objectToAdd, A<DateTime>.Ignored)).MustNotHaveHappened();
		}

		[Test]
		public void Get_CacheNullAndEmptyCollectionIsTrue_AddsEmptyCollectionToCacheAndReturnsEmptyCollection() {
			const string cacheKey = "abc";

			var objectToAdd = new string[] { };

			var cacheFake = A.Fake<ICache>();
			A.CallTo(() => cacheFake.Get(cacheKey)).Returns((object)null);

			var cacheManager = new CacheService(cacheFake, A.Fake<ICacheServiceLogger>());
			var obj = cacheManager.Get(cacheKey, new TimeSpan(0, 10, 0), true, () => objectToAdd);

			A.CallTo(() => cacheFake.Add(cacheKey, objectToAdd, A<DateTime>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Get_CacheNullAndEmptyCollectionIsFalse_AddsNotEmptyCollectionToCacheAndReturnsEmptyCollection() {
			const string cacheKey = "abc";

			var objectToAdd = new string[] { "1", "2" };

			var cacheFake = A.Fake<ICache>();
			A.CallTo(() => cacheFake.Get(cacheKey)).Returns((object)null);

			var cacheManager = new CacheService(cacheFake, A.Fake<ICacheServiceLogger>());
			var obj = cacheManager.Get(cacheKey, new TimeSpan(0, 10, 0), false, () => objectToAdd);

			A.CallTo(() => cacheFake.Add(cacheKey, objectToAdd, A<DateTime>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
		}
	}
}
