namespace Sitecore.MobileSdkUnitTest
{
  using System;
  using System.Collections.Generic;
  using System.Threading;
  using NUnit.Framework;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.Entities;

  [TestFixture]
  public class CreateEntitiesParserTest
  {
    const string VALID_RESPONSE = "{\n  \"@odata.context\": \"http://cms82u1.test24dk1.dk.sitecore.net/sitecore/api/ssc/aggregate/admin/$metadata#Todo/$entity\",\n  \"Title\": \"First Task\",\n  \"Completed\": false,\n  \"Index\": 4,\n  \"Id\": \"3\",\n  \"Url\": null\n}";

    [Test]
    public void TestParseValidResponse()
    {
      string rawResponse = VALID_RESPONSE;
      ScCreateEntityResponse response = ScCreateEntityParser.Parse(rawResponse, 202, CancellationToken.None);


      ISitecoreEntity entity = response.CreatedEntity;

      Assert.AreEqual("First Task", entity["Title"].RawValue);
      Assert.AreEqual("False", entity["Completed"].RawValue);
      Assert.AreEqual("4", entity["Index"].RawValue);

      Assert.IsTrue(response.Created);
    }

    [Test]
    public void TestParseResponceCode()
    {
      string rawResponse = VALID_RESPONSE;
      ScCreateEntityResponse response = ScCreateEntityParser.Parse(rawResponse, 444, CancellationToken.None);

      Assert.AreEqual(444, response.StatusCode);
    }

  }
}