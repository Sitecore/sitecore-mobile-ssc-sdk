namespace Sitecore.MobileSdkUnitTest
{
  using System;
  using NUnit.Framework;
  using Sitecore.MobileSDK.SessionSettings;
  using Sitecore.MobileSDK.UrlBuilder.Rest;
  using Sitecore.MobileSDK.UrlBuilder.SSC;

  [TestFixture]
  public class SessionConfigUrlBuilderTest
  {
    [Test]
    public void TestBuildBaseUrlWithSite()
    {
      SessionConfigUrlBuilder builder = new SessionConfigUrlBuilder(RestServiceGrammar.ItemSSCV2Grammar(), SSCUrlParameters.ItemSSCV2UrlParameters());
      SessionConfigPOD mockConfig = new SessionConfigPOD();
      mockConfig.InstanceUrl = "localhost";

      string result = builder.BuildUrlString(mockConfig);
      string expected = "http://localhost/sitecore/api/ssc";

      Assert.AreEqual(expected, result);
    }

    public void TestBuildBaseUrlWithoutSite()
    {
      SessionConfigUrlBuilder builder = new SessionConfigUrlBuilder(RestServiceGrammar.ItemSSCV2Grammar(), SSCUrlParameters.ItemSSCV2UrlParameters());
      SessionConfigPOD mockConfig = new SessionConfigPOD();
      mockConfig.InstanceUrl = "localhost";

      string result = builder.BuildUrlString(mockConfig);
      string expected = "http://localhost/-/item/v1";

      Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestBuildNullSessionConfig()
    {
      SessionConfigUrlBuilder builder = new SessionConfigUrlBuilder(RestServiceGrammar.ItemSSCV2Grammar(), SSCUrlParameters.ItemSSCV2UrlParameters());
      TestDelegate action = () => builder.BuildUrlString(null);

      Assert.Throws<ArgumentNullException>(action);
    }

  }
}

