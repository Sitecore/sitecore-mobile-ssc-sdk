namespace Sitecore.MobileSdkUnitTest
{
  using System;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  // @adk.review : waybe we should we wrap this? 
  using Newtonsoft.Json;
  using NUnit.Framework;
  using Sitecore.MobileSDK.API.Exceptions;
  using Sitecore.MobileSDK.API.Fields;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.Items;

  [TestFixture]
  public class GetItemsParserTest
  {
    const string VALID_RESPONSE =
      "{\"ItemID\":\"110d559f-dea5-42ea-9c1c-8a5df7e70ef9\",\"ItemName\":\"Home\",\"ItemPath\":\"/sitecore/content/Home\",\"ParentID\":\"0de95ae4-41ab-4d01-9eb0-67441b7c2450\",\"TemplateID\":\"76036f5e-cbce-46d1-af0a-4143f9b557aa\",\"TemplateName\":\"Sample Item\",\"CloneSource\":null,\"ItemLanguage\":\"en\",\"ItemVersion\":\"1\",\"DisplayName\":\"Home\",\"HasChildren\":\"True\",\"ItemIcon\":\"/temp/IconCache/Network/16x16/home.png\",\"ItemMedialUrl\":\"/temp/IconCache/Network/48x48/home.png\",\"ItemUrl\":\"~/link.aspx?_id=110D559FDEA542EA9C1C8A5DF7E70EF9&amp;_z=z\",\"Title\":\"Sitecore\",\"Text\":\"<div>Welcome to Sitecore!</div>\\n<div><br />\\n</div>\\n<a href=\\\"~/link.aspx?_id=A2EE64D5BD7A4567A27E708440CAA9CD&amp;_z=z\\\">Accelerometer</a>\"}";
    [Test]
    public void TestParseValidResponse()
    {
      string rawResponse = VALID_RESPONSE;
      ScItemsResponse response = ScItemsParser.Parse(rawResponse, "web", CancellationToken.None);
      Assert.AreEqual(1, response.ResultCount);

      ISitecoreItem item1 = response[0];

      Assert.AreEqual("Home", item1.DisplayName);
      Assert.AreEqual("web", item1.Source.Database);
      Assert.AreEqual(true, item1.HasChildren);
      Assert.AreEqual("en", item1.Source.Language);
      Assert.AreEqual("110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9", item1.Id.ToUpperInvariant());

      Assert.AreEqual("/sitecore/content/Home", item1.Path);
      Assert.AreEqual(1, item1.Source.VersionNumber);

      Assert.AreEqual(16, item1.FieldsCount);
    }

    [Test]
    public void TestParseEmptyResponse()
    {
      TestDelegate action = () => ScItemsParser.Parse(string.Empty, "web", CancellationToken.None);
      Assert.Throws<ArgumentException>(action, "cannot parse empty response");
    }

    [Test]
    public void TestParseNullResponse()
    {
      TestDelegate action = () => ScItemsParser.Parse(null, "web", CancellationToken.None);
      Assert.Throws<ArgumentException>(action, "cannot parse null response");
    }


    [Test]
    public void TestParseInvalidResponse()
    {
      string rawResponse = "bla";
      TestDelegate action = () => ScItemsParser.Parse(rawResponse, "web", CancellationToken.None);
      JsonReaderException exception = Assert.Throws<JsonReaderException>(action, "JsonReaderException should be here");

      Assert.AreEqual("Unexpected character encountered while parsing value: b. Path '', line 0, position 0.", exception.Message);
    }

    [Test]
    public void TestParseErrorResponse()
    {
      string rawResponse =
        "{\n\"Message\": \"An error has occurred.\"\n}";
      TestDelegate action = () => ScItemsParser.Parse(rawResponse, "web", CancellationToken.None);
      ParserException exception = Assert.Throws<ParserException>(action, "ParserException should be here");

      Assert.AreEqual("[Sitecore Mobile SDK] Data from the internet has unexpected format{\n  \"Message\": \"An error has occurred.\"\n}", exception.Message);
    }


    [Test]
    public void TestCancellationCausesOperationCanceledException()
    {
      TestDelegate testAction = async () =>
      {
        var cancel = new CancellationTokenSource();

        Task<ScItemsResponse> action = Task.Factory.StartNew(() =>
        {
          int millisecondTimeout = 10;
          Thread.Sleep(millisecondTimeout);

          return ScItemsParser.Parse(VALID_RESPONSE, "web", cancel.Token);
        });
        cancel.Cancel();
        await action;
      };
      Assert.Catch<OperationCanceledException>(testAction);
    }
  }
}