namespace Sitecore.MobileSdkUnitTest
{
  using System;
  using System.Collections.Generic;
  using NUnit.Framework;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.Items;
  using Sitecore.MobileSDK.MockObjects;
  using Sitecore.MobileSDK.SessionSettings;
  using Sitecore.MobileSDK.UrlBuilder.Rest;
  using Sitecore.MobileSDK.UrlBuilder.UpdateItem;
  using Sitecore.MobileSDK.UrlBuilder.SSC;
  using Sitecore.MobileSDK.UserRequest;

  [TestFixture]
  public class UpdateItemByIdUrlBuilderTests
  {
    private UpdateItemByIdUrlBuilder builder;
    private UserRequestMerger requestMerger;

    [SetUp]
    public void SetUp()
    {
      IRestServiceGrammar restGrammar = RestServiceGrammar.ItemSSCV2Grammar();
      ISSCUrlParameters webApiGrammar = SSCUrlParameters.ItemSSCV2UrlParameters();

      this.builder = new UpdateItemByIdUrlBuilder(restGrammar, webApiGrammar);

      SessionConfigPOD mutableSessionConfig = new SessionConfigPOD();
      mutableSessionConfig.InstanceUrl = "mobiledev1ua1.dk.sitecore.net:7119";

      ItemSource source = LegacyConstants.DefaultSource();
      this.requestMerger = new UserRequestMerger(mutableSessionConfig, source, null);

    }

    [TearDown]
    public void TearDown()
    {
      this.builder = null;
    }

    [Test]
    public void TestCorrectParamsWithoutFields()
    {
      IUpdateItemByIdRequest request = ItemSSCRequestBuilder.UpdateItemRequestWithId("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}")
        .Build();

      IUpdateItemByIdRequest autocompletedRequest = this.requestMerger.FillUpdateItemByIdGaps(request);

      string result = this.builder.GetUrlForRequest(autocompletedRequest);
      string expected = "http://mobiledev1ua1.dk.sitecore.net:7119/sitecore/api/ssc/item/%7B110d559f-dea5-42ea-9c1c-8a5df7e70ef9%7D?database=web&language=en";

      string fieldsResult = this.builder.GetFieldValuesList(autocompletedRequest);
      string expectedFieldsResult = "";

      Assert.AreEqual(expected, result);
      Assert.AreEqual(expectedFieldsResult, fieldsResult);
    }

    [Test]
    public void TestCorrectParamsWithFields()
    {
      IUpdateItemByIdRequest request = ItemSSCRequestBuilder.UpdateItemRequestWithId("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}")
        .Version(1)
        .AddFieldsRawValuesByNameToSet("field1", "value1")
        .AddFieldsRawValuesByNameToSet("field2", "value2")
        .Build();

      IUpdateItemByIdRequest autocompletedRequest = this.requestMerger.FillUpdateItemByIdGaps(request);

      string result = this.builder.GetUrlForRequest(autocompletedRequest);
      string expected = "http://mobiledev1ua1.dk.sitecore.net:7119/sitecore/api/ssc/item/%7B110d559f-dea5-42ea-9c1c-8a5df7e70ef9%7D?database=web&language=en&version=1";

      string fieldsResult = this.builder.GetFieldValuesList(autocompletedRequest);
      string expectedFieldsResult = "field1=value1&field2=value2";

      Assert.AreEqual(expected, result);
      Assert.AreEqual(expectedFieldsResult, fieldsResult);
    }

    [Test]
    public void TestFieldsValuesIsCaseInsensitive()
    {
      IUpdateItemByIdRequest request = ItemSSCRequestBuilder.UpdateItemRequestWithId("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}")
        .AddFieldsRawValuesByNameToSet("field1", "VaLuE1")
        .AddFieldsRawValuesByNameToSet("field2", "VaLuE2")
        .Build();

      IUpdateItemByIdRequest autocompletedRequest = this.requestMerger.FillUpdateItemByIdGaps(request);

      string result = this.builder.GetUrlForRequest(autocompletedRequest);
      string expected = "http://mobiledev1ua1.dk.sitecore.net:7119/sitecore/api/ssc/item/%7B110d559f-dea5-42ea-9c1c-8a5df7e70ef9%7D?database=web&language=en";

      string fieldsResult = this.builder.GetFieldValuesList(autocompletedRequest);
      string expectedFieldsResult = "field1=VaLuE1&field2=VaLuE2";

      Assert.AreEqual(expected, result);
      Assert.AreEqual(expectedFieldsResult, fieldsResult);
    }

    [Test]
    public void TestItemNameIsCaseInsensitive()
    {
      IUpdateItemByIdRequest request = ItemSSCRequestBuilder.UpdateItemRequestWithId("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}")
        .AddFieldsRawValuesByNameToSet("field1", "VaLuE1")
        .AddFieldsRawValuesByNameToSet("field2", "VaLuE2")
        .Build();

      IUpdateItemByIdRequest autocompletedRequest = this.requestMerger.FillUpdateItemByIdGaps(request);

      string result = this.builder.GetUrlForRequest(autocompletedRequest);
      string expected = "http://mobiledev1ua1.dk.sitecore.net:7119/sitecore/api/ssc/item/%7B110d559f-dea5-42ea-9c1c-8a5df7e70ef9%7D?database=web&language=en";

      string fieldsResult = this.builder.GetFieldValuesList(autocompletedRequest);
      string expectedFieldsResult = "field1=VaLuE1&field2=VaLuE2";

      Assert.AreEqual(expected, result);
      Assert.AreEqual(expectedFieldsResult, fieldsResult);
    }

    [Test]
    public void TestFieldWithDuplicatedKeyWillCrash()
    {
      var requestBuilder = ItemSSCRequestBuilder.UpdateItemRequestWithId("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}")
        .AddFieldsRawValuesByNameToSet("field1", "VaLuE1")
        .AddFieldsRawValuesByNameToSet("field2", "VaLuE2");

      TestDelegate action = () => requestBuilder.AddFieldsRawValuesByNameToSet("field1", "VaLuE3");
      Assert.Throws<InvalidOperationException>(action);
    }

    [Test]
    public void TestAppendingFields()
    {
      var requestBuilder = ItemSSCRequestBuilder.UpdateItemRequestWithId("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}")
        .AddFieldsRawValuesByNameToSet("field1", "VaLuE1")
        .AddFieldsRawValuesByNameToSet("field2", "VaLuE2");

      TestDelegate action = () => requestBuilder.AddFieldsRawValuesByNameToSet("field1", "VaLuE3");
      Assert.Throws<InvalidOperationException>(action);
    }

    [Test]
    public void TestFieldsAppending()
    {
      Dictionary<string, string> fields = new Dictionary<string, string>();
      fields.Add("field1", "VaLuE1");
      fields.Add("field2", "VaLuE2");

      IUpdateItemByIdRequest request = ItemSSCRequestBuilder.UpdateItemRequestWithId("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}")
        .Database("db")
        .Language("lg")
        .AddFieldsRawValuesByNameToSet(fields)
        .AddFieldsRawValuesByNameToSet("field3", "VaLuE3")
        .Build();

      IUpdateItemByIdRequest autocompletedRequest = this.requestMerger.FillUpdateItemByIdGaps(request);

      string result = this.builder.GetUrlForRequest(autocompletedRequest);
      string expected =
        "http://mobiledev1ua1.dk.sitecore.net:7119/sitecore/api/ssc/item/%7B110d559f-dea5-42ea-9c1c-8a5df7e70ef9%7D?database=db&language=lg";

      string fieldsResult = this.builder.GetFieldValuesList(autocompletedRequest);
      string expectedFieldsResult = "field1=VaLuE1&field2=VaLuE2&field3=VaLuE3";

      Assert.AreEqual(expected, result);
      Assert.AreEqual(expectedFieldsResult, fieldsResult);
    }
  }
}
