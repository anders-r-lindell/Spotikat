using System;

using FakeItEasy;

using NUnit.Framework;
using ServiceStack;
using SpotiKat.Configuration;

namespace SpotiKat.Tests {
	[TestFixture]
	public class RestServiceClientTests {
		[Test]
		public void Get_ServiceCallThrowsForbiddenWebServiceException_RetriesServiceCallAndThrowsWebServiceException() {
			var restClientConfigurationFake = A.Fake<IRestClientConfiguration>();
			A.CallTo(() => restClientConfigurationFake.MaxNumberOfRetries).Returns(5);
			A.CallTo(() => restClientConfigurationFake.SlowDownFactor).Returns(10);
			var restClientFake = A.Fake<ServiceStack.IRestClient>();
			A.CallTo(() => restClientFake.Get<Foo>(A<string>.Ignored)).Throws(
				new WebServiceException("Forbidden"));

			var restClient = new RestServiceClient(restClientFake, restClientConfigurationFake);

			try {
				var foo = restClient.Get<Foo>("abc");
			}catch(Exception ex) {
				Assert.That(ex, Is.TypeOf<WebServiceException>());
			}

			A.CallTo(() => restClientFake.Get<Foo>("abc")).MustHaveHappened(Repeated.Exactly.Times(5));
		}

		[Test]
		public void Get_ServiceCallThrowsNotFoundWebServiceException_DoesNotRetriesServiceCallAndThrowsWebServiceException() {
			var restClientConfigurationFake = A.Fake<IRestClientConfiguration>();
			A.CallTo(() => restClientConfigurationFake.MaxNumberOfRetries).Returns(5);
			A.CallTo(() => restClientConfigurationFake.SlowDownFactor).Returns(10);
			var restClientFake = A.Fake<ServiceStack.IRestClient>();
			A.CallTo(() => restClientFake.Get<Foo>(A<string>.Ignored)).Throws(
				new WebServiceException("Not Found"));

			var restClient = new RestServiceClient(restClientFake, restClientConfigurationFake);

			try {
				var foo = restClient.Get<Foo>("abc");
			}
			catch (Exception ex) {
				Assert.That(ex, Is.TypeOf<WebServiceException>());
			}

			A.CallTo(() => restClientFake.Get<Foo>("abc")).MustHaveHappened(Repeated.Exactly.Times(1));
		}

		[Test]
		public void Get_ServiceCallThrowsException_DoesNotRetriesServiceCallAndThrowsException() {
			var restClientConfigurationFake = A.Fake<IRestClientConfiguration>();
			A.CallTo(() => restClientConfigurationFake.MaxNumberOfRetries).Returns(5);
			A.CallTo(() => restClientConfigurationFake.SlowDownFactor).Returns(10);
			var restClientFake = A.Fake<ServiceStack.IRestClient>();
			A.CallTo(() => restClientFake.Get<Foo>(A<string>.Ignored)).Throws(
				new Exception("abc"));

			var restClient = new RestServiceClient(restClientFake, restClientConfigurationFake);

			try {
				var foo = restClient.Get<Foo>("abc");
			}
			catch (Exception ex) {
				Assert.That(ex, Is.TypeOf<Exception>());
			}

			A.CallTo(() => restClientFake.Get<Foo>("abc")).MustHaveHappened(Repeated.Exactly.Times(1));
		}

		[Test]
		public void Get_ServiceCallIsSuccessfull_ReturnsServiceCallResponseObject() {
			var restClientConfigurationFake = A.Fake<IRestClientConfiguration>();
			A.CallTo(() => restClientConfigurationFake.MaxNumberOfRetries).Returns(5);
			A.CallTo(() => restClientConfigurationFake.SlowDownFactor).Returns(10);
			var restClientFake = A.Fake<ServiceStack.IRestClient>();
			A.CallTo(() => restClientFake.Get<Foo>(A<string>.Ignored)).Returns(new Foo { Bar = "abc" });

			var restClient = new RestServiceClient(restClientFake, restClientConfigurationFake);

			var foo = restClient.Get<Foo>("abc");

			Assert.That(foo, Is.Not.Null);
			Assert.That(foo.Bar, Is.EqualTo("abc"));
		}
	}

	public class Foo {
		public string Bar { get; set; }
	}
}
