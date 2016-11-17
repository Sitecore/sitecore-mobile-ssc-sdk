namespace Sitecore.MobileSdkUnitTest
{
  using System;
  using System.Collections.Generic;
  using NUnit.Framework;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.API.Request.Parameters;
  using Sitecore.MobileSDK.MockObjects;
  using Sitecore.MobileSDK.SessionSettings;
  using Sitecore.MobileSDK.UrlBuilder.Entity;
  using Sitecore.MobileSDK.UrlBuilder.ItemByPath;
  using Sitecore.MobileSDK.UrlBuilder.QueryParameters;
  using Sitecore.MobileSDK.UrlBuilder.Rest;
  using Sitecore.MobileSDK.UrlBuilder.SSC;
  using Sitecore.MobileSDK.UserRequest;

  [TestFixture]
  public class GetEntitiesUrlBuilderTests
  {
    private GetEntitiesUrlBuilder<IBaseEntityRequest> getEntityBuilder;
    private ISessionConfig sessionConfig;

    [SetUp]
    public void SetUp()
    {
      IRestServiceGrammar restGrammar = RestServiceGrammar.ItemSSCV2Grammar();
      ISSCUrlParameters webApiGrammar = SSCUrlParameters.ItemSSCV2UrlParameters();

      this.getEntityBuilder = new GetEntitiesUrlBuilder<IBaseEntityRequest>(restGrammar, webApiGrammar);

      SessionConfigPOD mutableSession = new SessionConfigPOD();
      mutableSession.InstanceUrl = "http://mobiledev1ua1.dk.sitecore.net";
      this.sessionConfig = mutableSession;
    }

    [TearDown]
    public void TearDown()
    {
      this.getEntityBuilder = null;
      this.sessionConfig = null;
    }

    [Test]
    public void TestBuildWithValidPath()
    {
      MockReadEntitiesByPathParameters mutableParameters = new MockReadEntitiesByPathParameters();
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.EntitySource = new EntitySource("namespace", "controller", "id", "action");

      IBaseEntityRequest request = mutableParameters;

      string result = this.getEntityBuilder.GetUrlForRequest(request);
      string expected = "http://mobiledev1ua1.dk.sitecore.net/sitecore/api/ssc/namespace/controller/id/action";

      Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestBuildWithValidPathwithCustomParameters()
    {
      MockReadEntitiesByPathParameters mutableParameters = new MockReadEntitiesByPathParameters();
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.EntitySource = new EntitySource("namespace", "controller", "id", "action");
      mutableParameters.ParametersRawValuesByName = new Dictionary<string, string>(){{ "field1", "value1" }, { "field2", "value2"}};

      IBaseEntityRequest request = mutableParameters;

      string result = this.getEntityBuilder.GetUrlForRequest(request);
      string expected = "http://mobiledev1ua1.dk.sitecore.net/sitecore/api/ssc/namespace/controller/id/action?field1=value1&field2=value2";

      Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestBuildCustomParametersIsCaseSensitive()
    {
      MockReadEntitiesByPathParameters mutableParameters = new MockReadEntitiesByPathParameters();
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.EntitySource = new EntitySource("namespace", "controller", "id", "action");
      mutableParameters.ParametersRawValuesByName = new Dictionary<string, string>() { { "fIeLd1", "VaLuE1" }, { "FiElD2", "vAlUe2" } };

      IBaseEntityRequest request = mutableParameters;

      string result = this.getEntityBuilder.GetUrlForRequest(request);
      string expected = "http://mobiledev1ua1.dk.sitecore.net/sitecore/api/ssc/namespace/controller/id/action?fIeLd1=VaLuE1&FiElD2=vAlUe2";

      Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestBuildentityIdCanBeNull()
    {
      MockReadEntitiesByPathParameters mutableParameters = new MockReadEntitiesByPathParameters();
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.EntitySource = new EntitySource("namespace", "controller", null, "action");

      IBaseEntityRequest request = mutableParameters;

      string result = this.getEntityBuilder.GetUrlForRequest(request);
      string expected = "http://mobiledev1ua1.dk.sitecore.net/sitecore/api/ssc/namespace/controller/action";

      Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestBuildentityNamespaceCanNotBeNull()
    {
      MockReadEntitiesByPathParameters mutableParameters = new MockReadEntitiesByPathParameters();
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.EntitySource = new EntitySource(null, "controller", null, "action");

      IBaseEntityRequest request = mutableParameters;

      TestDelegate action = () => this.getEntityBuilder.GetUrlForRequest(request);
      Assert.Throws<ArgumentNullException>(action);
    }

    [Test]
    public void TestBuildentityControllerCanNotBeNull()
    {
      MockReadEntitiesByPathParameters mutableParameters = new MockReadEntitiesByPathParameters();
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.EntitySource = new EntitySource("Namespace", null, null, "action");

      IBaseEntityRequest request = mutableParameters;

      TestDelegate action = () => this.getEntityBuilder.GetUrlForRequest(request);
      Assert.Throws<ArgumentNullException>(action);
    }

    [Test]
    public void TestBuildentityActionCanNotBeNull()
    {
      MockReadEntitiesByPathParameters mutableParameters = new MockReadEntitiesByPathParameters();
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.EntitySource = new EntitySource("Namespace", null, "Controller", null);

      IBaseEntityRequest request = mutableParameters;

      TestDelegate action = () => this.getEntityBuilder.GetUrlForRequest(request);
      Assert.Throws<ArgumentNullException>(action);
    }

  }
}
