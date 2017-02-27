namespace MobileSDKIntegrationTest
{
  using System;
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
      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
          .Credentials(testData.Users.Admin)
          .BuildReadonlySession()
      )
      {
        var response = await session.AuthenticateAsync();
        Assert.True(response.IsSuccessful);
      }
    }

    [Test]
    public async void TestGetAuthenticationWithNotExistentUsername()
    {
      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
          .Credentials(testData.Users.NotExistent)
          .BuildReadonlySession()
      ) {
        var response = await session.AuthenticateAsync();
        Assert.False(response.IsSuccessful);
      }
    }

    [Test]
    public async void TestGetAuthenticationAstUserInExtraneDomainToWebsite()
    {
      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
          .Credentials(testData.Users.Creatorex)
          .BuildReadonlySession()
      )
      {
        var response = await session.AuthenticateAsync();
        Assert.True(response.IsSuccessful);
      }
    }

    [Test]
    public async void TestGetAuthenticationAsUserInSitecoreDomainToWebsite()
    {
      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
          .Credentials(testData.Users.SitecoreCreator)
          .BuildReadonlySession()
      )
      {
        var response = await session.AuthenticateAsync();
        Assert.True(response.IsSuccessful);
      }
    }

    [Test]
    public async void TestGetAuthenticationWithNotExistentPassword()
    {
      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
        .Credentials(new SSCCredentialsPOD(testData.Users.Admin.Username, "wrongpassword", "sitecore"))
        .BuildReadonlySession()
      )
      {
        var response = await session.AuthenticateAsync();
        Assert.False(response.IsSuccessful);
      }
    }

    [Test]
    public async void TestGetAuthenticationWithInvalidPassword()
    {
      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
        .Credentials(new SSCCredentialsPOD(testData.Users.Admin.Username, "Password $#%^&^*", "sitecore"))
        .BuildReadonlySession()
      )
      {
        var response = await session.AuthenticateAsync();
        Assert.False(response.IsSuccessful);
      }
    }

    [Test]
    public async void TestGetAuthenticationWithInvalidUsername()
    {
      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
          .Credentials(new SSCCredentialsPOD("Username $#%^&^*", testData.Users.Admin.Password, "sitecore"))
          .BuildReadonlySession()
      )
      {
        var response = await session.AuthenticateAsync();
        Assert.False(response.IsSuccessful);
      }
    }

    [Test]
    public void TestGetPublicKeyWithNotExistentUrl()
    {
      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost("http://mobilesdk-notexistent.com")
          .Credentials(testData.Users.Admin)
          .BuildReadonlySession()
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
      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost("\\m.dk%&^&*(.net")
        .Credentials(testData.Users.Admin)
        .BuildReadonlySession()
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
      var urlWithoutHttp = testData.InstanceUrl.Remove(0, 7);
      using
      (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(urlWithoutHttp)
          .Credentials(testData.Users.Admin)
          .BuildReadonlySession()
      )
      {
        var response = await session.AuthenticateAsync();
        Assert.True(response.IsSuccessful);
      }
    }
  }
}