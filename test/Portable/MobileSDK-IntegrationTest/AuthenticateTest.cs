namespace MobileSDKIntegrationTest
{
  using System;
  using System.Net.Http;
  using NUnit.Framework;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Exceptions;
  using Sitecore.MobileSDK.MockObjects;

  [TestFixture]
  public class AuthenticateTest
  {
    private TestEnvironment testData;

    [SetUp]
    public void Setup()
    {
      testData = TestEnvironment.DefaultTestEnvironment();
    }

    [TearDown]
    public void TearDown()
    {
      this.testData = null;
    }

    [Test]
    public async void TestCheckValidCredentials()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
          .Credentials(testData.Users.Admin)
          .BuildReadonlySession(handler, httpClient)
      )
      {
        var response = await session.AuthenticateAsync();
        Assert.True(response.IsSuccessful);
      }
    }

    [Test]
    public async void TestGetAuthenticationWithNotExistentUsername()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
          .Credentials(testData.Users.NotExistent)
          .BuildReadonlySession(handler, httpClient)
      ) {
        var response = await session.AuthenticateAsync();
        Assert.False(response.IsSuccessful);
      }
    }

    [Test]
    public async void TestGetAuthenticationAstUserInExtraneDomainToWebsite()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
          .Credentials(testData.Users.Creatorex)
          .BuildReadonlySession(handler, httpClient)
      )
      {
        var response = await session.AuthenticateAsync();
        Assert.True(response.IsSuccessful);
      }
    }

    [Test]
    public async void TestGetAuthenticationAsUserInSitecoreDomainToWebsite()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
          .Credentials(testData.Users.SitecoreCreator)
          .BuildReadonlySession(handler, httpClient)
      )
      {
        var response = await session.AuthenticateAsync();
        Assert.True(response.IsSuccessful);
      }
    }

    [Test]
    public async void TestGetAuthenticationWithNotExistentPassword()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
        .Credentials(new SSCCredentialsPOD(testData.Users.Admin.Username, "wrongpassword", "sitecore"))
        .BuildReadonlySession(handler, httpClient)
      )
      {
        var response = await session.AuthenticateAsync();
        Assert.False(response.IsSuccessful);
      }
    }

    [Test]
    public async void TestGetAuthenticationWithInvalidPassword()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
        .Credentials(new SSCCredentialsPOD(testData.Users.Admin.Username, "Password $#%^&^*", "sitecore"))
        .BuildReadonlySession(handler, httpClient)
      )
      {
        var response = await session.AuthenticateAsync();
        Assert.False(response.IsSuccessful);
      }
    }

    [Test]
    public async void TestGetAuthenticationWithInvalidUsername()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
          .Credentials(new SSCCredentialsPOD("Username $#%^&^*", testData.Users.Admin.Password, "sitecore"))
          .BuildReadonlySession(handler, httpClient)
      )
      {
        var response = await session.AuthenticateAsync();
        Assert.False(response.IsSuccessful);
      }
    }

    [Test]
    public void TestGetPublicKeyWithNotExistentUrl()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost("http://mobilesdk-notexistent.com")
          .Credentials(testData.Users.Admin)
          .BuildReadonlySession(handler, httpClient)
      )
      {
        TestDelegate testCode = async () =>
        {
          await session.AuthenticateAsync();
        };
        Exception exception = Assert.Throws<RsaHandshakeException>(testCode);
        Assert.True(exception.Message.Contains("ASPXAUTH not received properly"));


        // TODO : create platform specific files for this test case
        // Windows : System.Net.Http.HttpRequestException
        // iOS : System.Net.WebException

        //Assert.AreEqual("System.Net.Http.HttpRequestException", exception.InnerException.GetType().ToString());
        bool testCorrect = exception.InnerException.GetType().ToString().Equals("System.Net.Http.HttpRequestException");
        testCorrect = testCorrect || exception.InnerException.GetType().ToString().Equals("System.Net.WebException");
        Assert.IsTrue(testCorrect, "exception.InnerException is wrong");

        bool messageCorrect = exception.InnerException.Message.Contains("An error occurred while sending the request");
        messageCorrect = messageCorrect || exception.InnerException.Message.Contains("NameResolutionFailure");
        Assert.IsTrue(messageCorrect, "exception message is not correct");
      }
    }

    [Test]
    public void TestGetAuthenticationWithInvalidUrl()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost("\\m.dk%&^&*(.net")
        .Credentials(testData.Users.Admin)
        .BuildReadonlySession(handler, httpClient)
      )
      {
        TestDelegate testCode = async () =>
        {
          await session.AuthenticateAsync();
        };
        Exception exception = Assert.Throws<UriFormatException>(testCode);

        Assert.AreEqual("System.UriFormatException", exception.GetType().ToString());
        Assert.True(exception.Message.Contains("Invalid URI: The hostname could not be parsed"));
      }
    }

    [Test]
    public async void TestGetAuthenticationForUrlWithoutHttp()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      var urlWithoutHttp = testData.InstanceUrl.Remove(0, 7);
      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(urlWithoutHttp)
          .Credentials(testData.Users.Admin)
          .BuildReadonlySession(handler, httpClient)
      )
      {
        var response = await session.AuthenticateAsync();
        Assert.True(response.IsSuccessful);
      }
    }
  }
}