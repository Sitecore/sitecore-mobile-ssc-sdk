namespace Sitecore.MobileSdkUnitTest
{
  using System.Collections.Generic;
  using System.Threading;
  using NUnit.Framework;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.Entities;

  [TestFixture]
  public class GetEntitiesParserTest
  {
    const string VALID_RESPONSE = "{\n  \"@odata.context\": \"http://cms82u1.test24dk1.dk.sitecore.net/sitecore/api/ssc/aggregate/admin/$metadata#Todo\",\n  \"value\": [\n    {\n      \"Title\": \"First Task\",\n      \"Completed\": false,\n      \"Index\": 4,\n      \"Id\": \"1\",\n      \"Url\": null\n    },\n    {\n      \"Title\": \"First Task\",\n      \"Completed\": false,\n      \"Index\": 4,\n      \"Id\": \"2\",\n      \"Url\": null\n    }\n  ]\n}";


    [Test]
    public void TestParseValidResponse()
    {
      string rawResponse = VALID_RESPONSE;
      ScEntityResponse response = ScReadEntitiesParser.Parse(rawResponse, 200, CancellationToken.None);

      Assert.AreEqual(2, response.ResultCount);

      ISitecoreEntity entity = response[0];

      Assert.AreEqual("First Task", entity["Title"].RawValue);
      Assert.AreEqual("False", entity["Completed"].RawValue);
      Assert.AreEqual("4", entity["Index"].RawValue);

      Assert.AreEqual(5, entity.FieldsCount);
    }

    [Test]
    public void TestParseBoolValuesConversionToString()
    {
      string rawResponse = VALID_RESPONSE;
      ScEntityResponse response = ScReadEntitiesParser.Parse(rawResponse, 200, CancellationToken.None);

      Assert.AreEqual(2, response.ResultCount);

      ISitecoreEntity entity = response[0];

      Assert.AreEqual("False", entity["Completed"].RawValue);

      Assert.AreEqual(5, entity.FieldsCount);
    }

    [Test]
    public void TestParseIntValuesConversionToString()
    {
      string rawResponse = VALID_RESPONSE;
      ScEntityResponse response = ScReadEntitiesParser.Parse(rawResponse, 200, CancellationToken.None);

      Assert.AreEqual(2, response.ResultCount);

      ISitecoreEntity entity = response[0];

      Assert.AreEqual("4", entity["Index"].RawValue);

      Assert.AreEqual(5, entity.FieldsCount);
    }

    [Test]
    public void TestParseFieldNamesIsCaseSensitive()
    {
      string rawResponse = VALID_RESPONSE;
      ScEntityResponse response = ScReadEntitiesParser.Parse(rawResponse, 200, CancellationToken.None);

      Assert.AreEqual(2, response.ResultCount);

      ISitecoreEntity entity = response[0];

      Assert.AreEqual("First Task", entity["Title"].RawValue);

      string result;

      TestDelegate action = () => result = entity["title"].RawValue;
      Assert.Throws<KeyNotFoundException>(action);

      Assert.AreEqual(5, entity.FieldsCount);
    }

    [Test]
    public void TestParseNullValueConversion()
    {
      string rawResponse = VALID_RESPONSE;
      ScEntityResponse response = ScReadEntitiesParser.Parse(rawResponse, 200, CancellationToken.None);

      Assert.AreEqual(2, response.ResultCount);

      ISitecoreEntity entity = response[0];

      Assert.AreEqual(string.Empty, entity["Url"].RawValue);

      Assert.AreEqual(5, entity.FieldsCount);
    }
   
  }
}