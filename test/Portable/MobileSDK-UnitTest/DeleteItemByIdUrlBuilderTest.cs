namespace Sitecore.MobileSdkUnitTest
{
    using System;
    using NUnit.Framework;
    using Sitecore.MobileSDK.API.Request;
    using Sitecore.MobileSDK.Items.Delete;
    using Sitecore.MobileSDK.MockObjects;
    using Sitecore.MobileSDK.SessionSettings;
    using Sitecore.MobileSDK.UrlBuilder.DeleteItem;
    using Sitecore.MobileSDK.UrlBuilder.Rest;
    using Sitecore.MobileSDK.UrlBuilder.SSC;

    [TestFixture]
    public class DeleteItemByIdUrlBuilderTest
    {
        private SessionConfig sessionConfig;

        private string id;
        private string database;

        private IDeleteItemsUrlBuilder<IDeleteItemsByIdRequest> builder;

        [SetUp]
        public void Setup()
        {
            this.sessionConfig = new MutableSessionConfig("http://testurl");

            this.id = "{B0ED4777-1F5D-478D-AF47-145CCA9E4311}";
            this.database = "master";

            this.builder = new DeleteItemByIdUrlBuilder(RestServiceGrammar.ItemSSCV2Grammar(), SSCUrlParameters.ItemSSCV2UrlParameters());
        }

        [TearDown]
        public void TearDown()
        {
            this.sessionConfig = null;

            this.id = null;
            this.database = null;

            this.builder = null;
        }

        [Test]
        public void TestNullRequest()
        {
            TestDelegate action = () => this.builder.GetUrlForRequest(null);

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void TestNullSessionInParams()
        {
            TestDelegate action = () =>
            {
                var parameters = new DeleteItemByIdParameters(null, this.database, this.id);

                this.builder.GetUrlForRequest(parameters);
            };

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void TestNullId()
        {
            TestDelegate action = () =>
            {
                var parameters = new DeleteItemByIdParameters(this.sessionConfig, this.database, null);

                this.builder.GetUrlForRequest(parameters);
            };

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void TestCorrectId()
        {
            var parameters = new DeleteItemByIdParameters(this.sessionConfig, null, this.id);

            var url = this.builder.GetUrlForRequest(parameters);

            Assert.AreEqual("https://testurl/sitecore/api/ssc/item/%7bb0ed4777-1f5d-478d-af47-145cca9e4311%7d", url);
        }

        [Test]
        public void TestCorrectIdWithDatabase()
        {
            var parameters = new DeleteItemByIdParameters(this.sessionConfig, this.database, this.id);

            var url = this.builder.GetUrlForRequest(parameters);

            Assert.AreEqual("https://testurl/sitecore/api/ssc/item/%7bb0ed4777-1f5d-478d-af47-145cca9e4311%7d?database=master", url);
        }

        [Test]
        public void TestForceHttpsProtocol()
        {
            var localSessionConfig = new MutableSessionConfig("http://testurl");

            var parameters = new DeleteItemByIdParameters(localSessionConfig, this.database, this.id);

            var url = this.builder.GetUrlForRequest(parameters);

            Assert.AreEqual("https://testurl/sitecore/api/ssc/item/%7bb0ed4777-1f5d-478d-af47-145cca9e4311%7d?database=master", url);
        }

        [Test]
        public void TestHTTPSProtocolWillNotBeChanged()
        {
            var localSessionConfig = new MutableSessionConfig("https://testurl");

            var parameters = new DeleteItemByIdParameters(localSessionConfig, this.database, this.id);

            var url = this.builder.GetUrlForRequest(parameters);

            Assert.AreEqual("https://testurl/sitecore/api/ssc/item/%7bb0ed4777-1f5d-478d-af47-145cca9e4311%7d?database=master", url);
        }

    }
}

