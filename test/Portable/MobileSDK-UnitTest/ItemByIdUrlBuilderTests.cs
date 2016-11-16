namespace Sitecore.MobileSdkUnitTest
{
  using System;
  using NUnit.Framework;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Request.Parameters;
  using Sitecore.MobileSDK.MockObjects;
  using Sitecore.MobileSDK.SessionSettings;
  using Sitecore.MobileSDK.UrlBuilder.ItemById;
  using Sitecore.MobileSDK.UrlBuilder.QueryParameters;
  using Sitecore.MobileSDK.UrlBuilder.Rest;
  using Sitecore.MobileSDK.UrlBuilder.SSC;
  using Sitecore.MobileSDK.UserRequest;

  [TestFixture]
  public class ItemByIdUrlBuilderTests
  {
    private ItemByIdUrlBuilder builder;
    private ISessionConfig sessionConfig;
    private ISessionConfig sitecoreShellConfig;
    private QueryParameters payload;

    [SetUp]
    public void SetUp()
    {
      IRestServiceGrammar restGrammar = RestServiceGrammar.ItemSSCV2Grammar();
      ISSCUrlParameters webApiGrammar = SSCUrlParameters.ItemSSCV2UrlParameters();

      this.builder = new ItemByIdUrlBuilder(restGrammar, webApiGrammar);

      SessionConfigPOD mutableSessionConfig = new SessionConfigPOD();
      mutableSessionConfig.InstanceUrl = "sitecore.net";
      this.sessionConfig = mutableSessionConfig;


      mutableSessionConfig = new SessionConfigPOD();
      mutableSessionConfig.InstanceUrl = "mobiledev1ua1.dk.sitecore.net:7119";
      this.sitecoreShellConfig = mutableSessionConfig;

      this.payload = new QueryParameters(null);
    }

    [TearDown]
    public void TearDown()
    {
      this.builder = null;
      this.sessionConfig = null;
    }

    [Test]
    public void TestInvaliItemIdCausesArgumentException()
    {
      MockGetItemsByIdParameters mutableParameters = new MockGetItemsByIdParameters();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.ItemId = "someInvalidItemId";
      mutableParameters.QueryParameters = this.payload;

      IReadItemsByIdRequest parameters = mutableParameters;

      TestDelegate action = () => this.builder.GetUrlForRequest(parameters);
      Assert.Throws<ArgumentException>(action);
    }

    [Test]
    public void TestUrlBuilderExcapesArgs()
    {
      MockGetItemsByIdParameters mutableParameters = new MockGetItemsByIdParameters();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.ItemId = "110d559f-dea5-42ea-9c1c-8a5df7e70ef9";
      mutableParameters.QueryParameters = this.payload;

      IReadItemsByIdRequest parameters = mutableParameters;

      string result = this.builder.GetUrlForRequest(parameters);
      string expected = "http://sitecore.net/sitecore/api/ssc/item/110d559f-dea5-42ea-9c1c-8a5df7e70ef9?database=web&language=en";

      Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestUrlBuilderAddsItemSource()
    {
      MockGetItemsByIdParameters mutableParameters = new MockGetItemsByIdParameters();
      mutableParameters.SessionSettings = this.sitecoreShellConfig;
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.ItemId = "110d559f-dea5-42ea-9c1c-8a5df7e70ef9";
      mutableParameters.QueryParameters = this.payload;

      IReadItemsByIdRequest parameters = mutableParameters;

      string result = this.builder.GetUrlForRequest(parameters);
      string expected = "http://mobiledev1ua1.dk.sitecore.net:7119/sitecore/api/ssc/item/110d559f-dea5-42ea-9c1c-8a5df7e70ef9?database=web&language=en";

      Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestUrlBuilderThrowsExceptionForNullItemId()
    {
      MockGetItemsByIdParameters mutableParameters = new MockGetItemsByIdParameters();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.ItemId = null;
      mutableParameters.QueryParameters = this.payload;

      IReadItemsByIdRequest parameters = mutableParameters;

      TestDelegate action = () => this.builder.GetUrlForRequest(parameters);

      Assert.Throws<ArgumentNullException>(action);
    }

    [Test]
    public void TestBracesIdWithoutTextIsInvalid()
    {
      MockGetItemsByIdParameters mutableParameters = new MockGetItemsByIdParameters();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.ItemId = "{}";
      mutableParameters.QueryParameters = this.payload;

      IReadItemsByIdRequest parameters = mutableParameters;

      TestDelegate action = () => this.builder.GetUrlForRequest(parameters);
      Assert.Throws<ArgumentException>(action);
    }

    [Test]
    public void TestOptionalSourceInSessionAndUserRequest()
    {
      var connection = new SessionConfig("localhost");

      var request = ItemSSCRequestBuilder.ReadItemsRequestWithId("110d559f-dea5-42ea-9c1c-8a5df7e70ef9").Build();
      var requestMerger = new UserRequestMerger(connection, null, null);
      var mergedRequest = requestMerger.FillReadItemByIdGaps(request);

      var urlBuilder = new ItemByIdUrlBuilder(RestServiceGrammar.ItemSSCV2Grammar(), SSCUrlParameters.ItemSSCV2UrlParameters());

      string result = urlBuilder.GetUrlForRequest(mergedRequest);
      string expected = "http://localhost/sitecore/api/ssc/item/110d559f-dea5-42ea-9c1c-8a5df7e70ef9";

      Assert.AreEqual(expected, result);
    }


    [Test]
    public void TestExplicitDatabase()
    {
      var connection = new SessionConfig("localhost");

      var request = ItemSSCRequestBuilder.ReadItemsRequestWithId("110d559f-dea5-42ea-9c1c-8a5df7e70ef9")
        .Database("master")
        .Build();
      var requestMerger = new UserRequestMerger(connection, null, null);
      var mergedRequest = requestMerger.FillReadItemByIdGaps(request);

      var urlBuilder = new ItemByIdUrlBuilder(RestServiceGrammar.ItemSSCV2Grammar(), SSCUrlParameters.ItemSSCV2UrlParameters());

      string result = urlBuilder.GetUrlForRequest(mergedRequest);
      string expected = "http://localhost/sitecore/api/ssc/item/110d559f-dea5-42ea-9c1c-8a5df7e70ef9?database=master";

      Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestDatabaseAndExplicitLanguage()
    {
      var connection = new SessionConfig("localhost");

      var request = ItemSSCRequestBuilder.ReadItemsRequestWithId("110d559f-dea5-42ea-9c1c-8a5df7e70ef9")
        .Language("da")
        .Build();
      var requestMerger = new UserRequestMerger(connection, null, null);
      var mergedRequest = requestMerger.FillReadItemByIdGaps(request);

      var urlBuilder = new ItemByIdUrlBuilder(RestServiceGrammar.ItemSSCV2Grammar(), SSCUrlParameters.ItemSSCV2UrlParameters());

      string result = urlBuilder.GetUrlForRequest(mergedRequest);
      string expected = "http://localhost/sitecore/api/ssc/item/110d559f-dea5-42ea-9c1c-8a5df7e70ef9?language=da";

      Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestDuplicateFieldsCauseException()
    {
      MockGetItemsByIdParameters mutableParameters = new MockGetItemsByIdParameters();
      mutableParameters.SessionSettings = this.sitecoreShellConfig;
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.ItemId = "110d559f-dea5-42ea-9c1c-8a5df7e70ef9";

      string[] fields = { "x", "y", "x" };
      IQueryParameters duplicatedFields = new QueryParameters(fields);
      mutableParameters.QueryParameters = duplicatedFields;


      IReadItemsByIdRequest parameters = mutableParameters;
      Assert.Throws<ArgumentException>(() => this.builder.GetUrlForRequest(parameters));
    }


    [Test]
    public void TestDuplicateFieldsWithDifferentCaseCauseException()
    {
      MockGetItemsByIdParameters mutableParameters = new MockGetItemsByIdParameters();
      mutableParameters.SessionSettings = this.sitecoreShellConfig;
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.ItemId = "110d559f-dea5-42ea-9c1c-8a5df7e70ef9";

      string[] fields = { "x", "y", "X" };
      IQueryParameters duplicatedFields = new QueryParameters(fields);
      mutableParameters.QueryParameters = duplicatedFields;


      IReadItemsByIdRequest parameters = mutableParameters;
      Assert.Throws<ArgumentException>(() => this.builder.GetUrlForRequest(parameters));
    }

    [Test]
    public void TestNullFieldEntriesCaseCauseException()
    {
      MockGetItemsByIdParameters mutableParameters = new MockGetItemsByIdParameters();
      mutableParameters.SessionSettings = this.sitecoreShellConfig;
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.ItemId = "{   xxx   }";

      string[] fields = { "x", "y", null };
      IQueryParameters duplicatedFields = new QueryParameters(fields);
      mutableParameters.QueryParameters = duplicatedFields;


      IReadItemsByIdRequest parameters = mutableParameters;
      Assert.Throws<ArgumentException>(() => this.builder.GetUrlForRequest(parameters));
    }

    [Test]
    public void TestEmptyFieldEntriesCaseCauseException()
    {
      MockGetItemsByIdParameters mutableParameters = new MockGetItemsByIdParameters();
      mutableParameters.SessionSettings = this.sitecoreShellConfig;
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.ItemId = "{   xxx   }";

      string[] fields = { "x", "y", "" };
      IQueryParameters duplicatedFields = new QueryParameters(fields);
      mutableParameters.QueryParameters = duplicatedFields;


      IReadItemsByIdRequest parameters = mutableParameters;
      Assert.Throws<ArgumentException>(() => this.builder.GetUrlForRequest(parameters));
    }
  }
}
