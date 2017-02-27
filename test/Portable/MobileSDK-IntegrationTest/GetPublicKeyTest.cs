namespace MobileSDKIntegrationTest
{
  using System;
  using System.Net.Http;
  using NUnit.Framework;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Exceptions;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.MockObjects;

  [TestFixture]
  public class GetPublicKeyTest
  {
    private TestEnvironment testData;

    IReadItemsByIdRequest requestWithItemId;

    [SetUp]
    public void Setup()
    {
      testData = TestEnvironment.DefaultTestEnvironment();
      this.requestWithItemId = ItemSSCRequestBuilder.ReadItemsRequestWithId(this.testData.Items.Home.Id).Build();
    }

    [TearDown]
    public void TearDown()
    {
      this.testData = null;
    }

    [Test]
    public async void TestGetItemAsAuthenticatedUser()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(this.testData.InstanceUrl)
          .Credentials(this.testData.Users.Admin)
          .BuildReadonlySession(handler, httpClient)
      )
      {
        var response = await session.ReadItemAsync(requestWithItemId);
        testData.AssertItemsCount(1, response);
        Assert.AreEqual(testData.Items.Home.DisplayName, response[0].DisplayName);
      }
    }

    [Test]
    public async void TestMissingHttpIsAutocompletedDuringAuthentication()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      var urlWithoutHttp = testData.InstanceUrl.Remove(0, 7);
      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(urlWithoutHttp)
          .Credentials(this.testData.Users.Admin)
          .BuildReadonlySession(handler, httpClient)
      )
      {
        var certrificate = await session.ReadItemAsync(this.requestWithItemId);
        Assert.IsNotNull(certrificate);
      }
    }

    [Test]
    public async void TestAuthenticateWithSlashInTheEnd()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      string urlWithSlahInTheEnd = testData.InstanceUrl + '/';
      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(urlWithSlahInTheEnd)
          .Credentials(this.testData.Users.Admin)
          .BuildReadonlySession(handler, httpClient)
      )
      {
        var response = await session.ReadItemAsync(requestWithItemId);
        testData.AssertItemsCount(1, response);
        Assert.AreEqual(testData.Items.Home.DisplayName, response[0].DisplayName);
      }
    }

    [Test]
    public void TestGetItemsWithNotExistentInstanceUrlReturnsError()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost("http://mobiledev1ua1.dddk.sitecore.net")
          .Credentials(this.testData.Users.Admin)
          .BuildReadonlySession(handler, httpClient)
      )
      {
        TestDelegate testCode = async () =>
        {
          var task = session.ReadItemAsync(this.requestWithItemId);
          await task;
        };
        Exception exception = Assert.Throws<RsaHandshakeException>(testCode);

        Assert.AreEqual("Sitecore.MobileSDK.API.Exceptions.RsaHandshakeException", exception.GetType().ToString());
        Assert.AreEqual("[Sitecore Mobile SDK] ASPXAUTH not received properly", exception.Message);
      }
    }

    [Test]
    public void TestGetItemWithNotExistentUserReturnsError()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
          .Credentials(this.testData.Users.NotExistent)
          .DefaultDatabase("web")
          .DefaultLanguage("en")
          .BuildSession(handler, httpClient)
      )
      {
        TestDelegate testCode = async () =>
        {
          var task = session.ReadItemAsync(this.requestWithItemId);
          await task;
        };
        Exception exception = Assert.Throws<RsaHandshakeException>(testCode);

        Assert.AreEqual("Sitecore.MobileSDK.API.Exceptions.RsaHandshakeException", exception.GetType().ToString());
        Assert.AreEqual("[Sitecore Mobile SDK] ASPXAUTH not received properly", exception.Message);
      }
    }

    [Test]
    public void TestGetItemWithInvalidUsernameAndPasswordReturnsError()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
          .Credentials(new SSCCredentialsPOD("/?*not#valid@username", "*not_valid ^ pwd", "fdfdfd"))
          .BuildSession(handler, httpClient)
      )
      {
        TestDelegate testCode = async () =>
        {
          var task = session.ReadItemAsync(this.requestWithItemId);
          await task;
        };
        Exception exception = Assert.Throws<RsaHandshakeException>(testCode);

        Assert.AreEqual("Sitecore.MobileSDK.API.Exceptions.RsaHandshakeException", exception.GetType().ToString());
        Assert.AreEqual("[Sitecore Mobile SDK] ASPXAUTH not received properly", exception.Message);
      }
    }

    [Test]
    public void TestGetItemAsAnonymousWithoutReadAccessReturnsError()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
      (
        var session = SitecoreSSCSessionBuilder.AnonymousSessionWithHost(testData.InstanceUrl)
        .DefaultDatabase("web")
        .DefaultLanguage("en")
        .BuildReadonlySession(handler, httpClient)
      )
      {
        TestDelegate testCode = async () =>
        {
          var result = await session.ReadItemAsync(this.requestWithItemId);
          Console.WriteLine(result[0].DisplayName);
        };
        Exception exception = Assert.Throws<ParserException>(testCode);

        Assert.AreEqual("[Sitecore Mobile SDK] Data from the internet has unexpected format", exception.Message);
        Assert.AreEqual("Sitecore.MobileSDK.API.Exceptions.SSCJsonErrorException", exception.InnerException.GetType().ToString());
        Assert.True(exception.InnerException.Message.Contains("Access to site is not granted."));
      }
    }
  }
}
