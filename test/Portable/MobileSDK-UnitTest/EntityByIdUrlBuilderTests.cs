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
  public class EntityByIdUrlBuilderTests
  {
    private EntityByIdUrlBuilder<IReadEntityByIdRequest> entitybyIdBuilder;
    private ISessionConfig sessionConfig;

    [SetUp]
    public void SetUp()
    {
      IRestServiceGrammar restGrammar = RestServiceGrammar.ItemSSCV2Grammar();
      ISSCUrlParameters webApiGrammar = SSCUrlParameters.ItemSSCV2UrlParameters();

      this.entitybyIdBuilder = new EntityByIdUrlBuilder<IReadEntityByIdRequest>(restGrammar, webApiGrammar);

      SessionConfigPOD mutableSession = new SessionConfigPOD();
      mutableSession.InstanceUrl = "http://mobiledev1ua1.dk.sitecore.net";
      this.sessionConfig = mutableSession;
    }

    [TearDown]
    public void TearDown()
    {
      this.entitybyIdBuilder = null;
      this.sessionConfig = null;
    }

    [Test]
    public void TestBuildWithValidPath()
    {
      MockReadEntityByIdParameters mutableParameters = new MockReadEntityByIdParameters();
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.EntitySource = new EntitySource("namespace", "controller", "id", "action");
      mutableParameters.EntityID = "bla";

      IReadEntityByIdRequest request = mutableParameters;

      string result = this.entitybyIdBuilder.GetUrlForRequest(request);
      string expected = "http://mobiledev1ua1.dk.sitecore.net/sitecore/api/ssc/namespace/controller/id/action('bla')";

      Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestBuildWithValidPathwithCustomParameters()
    {
      MockReadEntityByIdParameters mutableParameters = new MockReadEntityByIdParameters();
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.EntitySource = new EntitySource("namespace", "controller", "id", "action");
      mutableParameters.ParametersRawValuesByName = new Dictionary<string, string>(){{ "field1", "value1" }, { "field2", "value2"}};
      mutableParameters.EntityID = "bla";

      IReadEntityByIdRequest request = mutableParameters;

      string result = this.entitybyIdBuilder.GetUrlForRequest(request);
      string expected = "http://mobiledev1ua1.dk.sitecore.net/sitecore/api/ssc/namespace/controller/id/action('bla')?field1=value1&field2=value2";

      Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestBuildCustomParametersIsCaseSensitive()
    {
      MockReadEntityByIdParameters mutableParameters = new MockReadEntityByIdParameters();
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.EntitySource = new EntitySource("namespace", "controller", "id", "action");
      mutableParameters.ParametersRawValuesByName = new Dictionary<string, string>() { { "fIeLd1", "VaLuE1" }, { "FiElD2", "vAlUe2" } };
      mutableParameters.EntityID = "bla";

      IReadEntityByIdRequest request = mutableParameters;

      string result = this.entitybyIdBuilder.GetUrlForRequest(request);
      string expected = "http://mobiledev1ua1.dk.sitecore.net/sitecore/api/ssc/namespace/controller/id/action('bla')?fIeLd1=VaLuE1&FiElD2=vAlUe2";

      Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestBuildEntityIdIsCaseSensitive()
    {
      MockReadEntityByIdParameters mutableParameters = new MockReadEntityByIdParameters();
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.EntitySource = new EntitySource("namespace", "controller", "id", "action");
      mutableParameters.ParametersRawValuesByName = new Dictionary<string, string>() { { "fIeLd1", "VaLuE1" }, { "FiElD2", "vAlUe2" } };
      mutableParameters.EntityID = "BuGaGa";

      IReadEntityByIdRequest request = mutableParameters;

      string result = this.entitybyIdBuilder.GetUrlForRequest(request);
      string expected = "http://mobiledev1ua1.dk.sitecore.net/sitecore/api/ssc/namespace/controller/id/action('BuGaGa')?fIeLd1=VaLuE1&FiElD2=vAlUe2";

      Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestBuildentityIdCanBeNull()
    {
      MockReadEntityByIdParameters mutableParameters = new MockReadEntityByIdParameters();
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.EntitySource = new EntitySource("namespace", "controller", null, "action");
      mutableParameters.EntityID = "bla";

      IReadEntityByIdRequest request = mutableParameters;

      string result = this.entitybyIdBuilder.GetUrlForRequest(request);
      string expected = "http://mobiledev1ua1.dk.sitecore.net/sitecore/api/ssc/namespace/controller/action('bla')";

      Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestBuildentityNamespaceCanNotBeNull()
    {
      MockReadEntityByIdParameters mutableParameters = new MockReadEntityByIdParameters();
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.EntitySource = new EntitySource(null, "controller", null, "action");
      mutableParameters.EntityID = "bla";

      IReadEntityByIdRequest request = mutableParameters;

      TestDelegate action = () => this.entitybyIdBuilder.GetUrlForRequest(request);
      Assert.Throws<ArgumentNullException>(action);
    }

    [Test]
    public void TestBuildEntityIdIsRequired()
    {
      MockReadEntityByIdParameters mutableParameters = new MockReadEntityByIdParameters();
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.EntitySource = new EntitySource(null, "controller", null, "action");
      mutableParameters.EntityID = null;

      IReadEntityByIdRequest request = mutableParameters;

      TestDelegate action = () => this.entitybyIdBuilder.GetUrlForRequest(request);
      Assert.Throws<ArgumentNullException>(action);
    }

    [Test]
    public void TestBuildentityControllerCanNotBeNull()
    {
      MockReadEntityByIdParameters mutableParameters = new MockReadEntityByIdParameters();
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.EntitySource = new EntitySource("Namespace", null, null, "action");
      mutableParameters.EntityID = "bla";

      IReadEntityByIdRequest request = mutableParameters;

      TestDelegate action = () => this.entitybyIdBuilder.GetUrlForRequest(request);
      Assert.Throws<ArgumentNullException>(action);
    }

    [Test]
    public void TestBuildentityActionCanNotBeNull()
    {
      MockReadEntityByIdParameters mutableParameters = new MockReadEntityByIdParameters();
      mutableParameters.ItemSource = LegacyConstants.DefaultSource();
      mutableParameters.SessionSettings = this.sessionConfig;
      mutableParameters.EntitySource = new EntitySource("Namespace", null, "Controller", null);
      mutableParameters.EntityID = "bla";

      IReadEntityByIdRequest request = mutableParameters;

      TestDelegate action = () => this.entitybyIdBuilder.GetUrlForRequest(request);
      Assert.Throws<ArgumentNullException>(action);
    }

  }
}
