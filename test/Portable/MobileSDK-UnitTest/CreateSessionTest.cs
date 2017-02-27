namespace Sitecore.MobileSdkUnitTest
{
  using System;
  using System.Net.Http;
  using NUnit.Framework;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Session;
  using Sitecore.MobileSDK.MockObjects;
  using Sitecore.MobileSDK.PasswordProvider.Interface;
  using Sitecore.MobileSDK.SessionSettings;

  [TestFixture]
  public class CreateSessionTest
  {
    private IScCredentials adminCredentials = new SSCCredentialsPOD("admin", "b", "sitecore");

    #region Explicit Construction
    [Test]
    public void TestSessionConfigForAuthenticatedSession()
    {
      var sessionSettings = new SessionConfig("localhost");
      var credentials = new SSCCredentialsPOD("root", "pass", "sitecore");

      Assert.IsNotNull(sessionSettings);
      Assert.IsNotNull(credentials);

      Assert.AreEqual("localhost", sessionSettings.InstanceUrl);
      Assert.AreEqual("root", credentials.Username);
      Assert.AreEqual("pass", credentials.Password);
      Assert.AreEqual("sitecore", credentials.Domain);
    }

    [Test]
    public void TestSessionConfigAllowsBothNullForAuthenticatedSession()
    {
      var sessionSettings = new SessionConfig("localhost");
      var credentials = new SSCCredentialsPOD(null, null, "sitecore");

      Assert.IsNotNull(sessionSettings);
      Assert.IsNotNull(credentials);

      Assert.AreEqual("localhost", sessionSettings.InstanceUrl);
      Assert.IsNull(credentials.Username);
      Assert.IsNull(credentials.Password);
      Assert.AreEqual("sitecore", credentials.Domain);
    }


    [Test]
    public void TestSessionConfigAllowsNullUsernameForAuthenticatedSession()
    {
      var sessionSettings = new SessionConfig("localhost");
      var credentials = new SSCCredentialsPOD(null, "pass", "sitecore");

      Assert.IsNotNull(sessionSettings);
      Assert.IsNotNull(credentials);

      Assert.AreEqual("localhost", sessionSettings.InstanceUrl);
      Assert.IsNull(credentials.Username);
      Assert.AreEqual("pass", credentials.Password);
      Assert.AreEqual("sitecore", credentials.Domain);
    }
    #endregion Explicit Construction

    #region Builder Interface
    [Test]
    public void TestAnonymousSessionShouldBeCreatedByTheBuilder()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      var builder =
        SitecoreSSCSessionBuilder.AnonymousSessionWithHost("sitecore.net")
          .DefaultDatabase("web")
          .DefaultLanguage("en")
          .MediaLibraryRoot("/sitecore/media library")
          .DefaultMediaResourceExtension("ashx")
          .MediaPrefix("~/media");

      var session = builder.BuildSession(handler, httpClient);
      Assert.IsNotNull(session);

      var roSession = builder.BuildReadonlySession(handler, httpClient);
      Assert.IsNotNull(roSession);
    }

    [Test]
    public void TestAuthenticatedSessionShouldBeCreatedByTheBuilder()
    {
      IScCredentials credentials = this.adminCredentials;

      var builder = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost("sitecore.net")
        .Credentials(credentials)
        .DefaultDatabase("web")
        .DefaultLanguage("en")
        .MediaLibraryRoot("/sitecore/media library")
        .DefaultMediaResourceExtension("ashx");

      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      ISitecoreSSCSession session = builder.BuildSession(handler, httpClient);
      Assert.IsNotNull(session);

      var roSession = builder.BuildReadonlySession(handler, httpClient);
      Assert.IsNotNull(roSession);
    }
    #endregion Builder Interface

    #region Write Once


    [Test]
    public void TestDatabaseIsWriteOnce()
    {
      Assert.Throws<InvalidOperationException>(() =>
        SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost("sitecore.net")
        .Credentials(this.adminCredentials)
        .DefaultDatabase("web")
        .DefaultDatabase("web")
      );

      Assert.Throws<InvalidOperationException>(() =>
        SitecoreSSCSessionBuilder.AnonymousSessionWithHost("sitecore.net")
        .DefaultDatabase("master")
        .DefaultDatabase("core")
      );
    }

    [Test]
    public void TestLanguageIsWriteOnce()
    {
      Assert.Throws<InvalidOperationException>(() =>
        SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost("sitecore.net")
        .Credentials(this.adminCredentials)
        .DefaultLanguage("en")
        .DefaultLanguage("es")
      );

      Assert.Throws<InvalidOperationException>(() =>
        SitecoreSSCSessionBuilder.AnonymousSessionWithHost("sitecore.net")
        .DefaultLanguage("en")
        .DefaultLanguage("en")
      );
    }

    [Test]
    public void TestMediaRootIsWriteOnce()
    {
      Assert.Throws<InvalidOperationException>(() =>
        SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost("sitecore.net")
        .Credentials(this.adminCredentials)
        .MediaLibraryRoot("/sitecore/media library")
        .MediaLibraryRoot("/sitecore/other media library")
      );

      Assert.Throws<InvalidOperationException>(() =>
        SitecoreSSCSessionBuilder.AnonymousSessionWithHost("sitecore.net")
        .MediaLibraryRoot("/dev/null")
        .MediaLibraryRoot("/sitecore/media library")
      );
    }


    [Test]
    public void TestMediaExtIsWriteOnce()
    {
      Assert.Throws<InvalidOperationException>(() =>
        SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost("sitecore.net")
        .Credentials(this.adminCredentials)
        .DefaultMediaResourceExtension("ashx")
        .DefaultMediaResourceExtension("pdf")
      );

      Assert.Throws<InvalidOperationException>(() =>
        SitecoreSSCSessionBuilder.AnonymousSessionWithHost("sitecore.net")
        .DefaultMediaResourceExtension("jpeg")
        .DefaultMediaResourceExtension("jpg")
      );
    }
    #endregion Write Once

    #region Validate Null
    [Test]
    public void TestSSCVersionThrowsExceptionForNullInput()
    {
      Assert.Throws<ArgumentNullException>(() =>
        SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost("sitecore.net")
        .Credentials(this.adminCredentials)
        .SSCVersion(null)
      );

      Assert.Throws<ArgumentNullException>(() =>
        SitecoreSSCSessionBuilder.AnonymousSessionWithHost("sitecore.net")
        .SSCVersion(null)
      );
    }

    [Test]
    public void TestDatabaseDoNotThrowsExceptionForNullInput()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
        (
        var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost("sitecore.net")
        .Credentials(this.adminCredentials)
        .DefaultDatabase(null)
          .BuildSession(handler, httpClient)
        )
      {
        Assert.IsNotNull(session);
      }
    }

    [Test]
    public void TestLanguageDoNotThrowsExceptionForNullInput()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
        (
          var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost("sitecore.net")
          .Credentials(this.adminCredentials)
          .DefaultLanguage(null)
          .BuildSession(handler, httpClient)
        )
      {
        Assert.IsNotNull(session);
      }
    }

    [Test]
    public void TestSiteDoNotThrowsExceptionForNullInput()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
        (
          var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost("sitecore.net")
          .Credentials(this.adminCredentials)
          .BuildSession(handler, httpClient)
        )
      {
        Assert.IsNotNull(session);
      }
    }


    [Test]
    public void TestMediaDoNotThrowsExceptionForNullInput()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
        (
          var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost("sitecore.net")
          .Credentials(this.adminCredentials)
          .MediaLibraryRoot(null)
          .BuildSession(handler, httpClient)
        )
      {
        Assert.IsNotNull(session);
      }
    }


    [Test]
    public void TestMediaExtDonotThrowsExceptionForNullInput()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      using
        (
          var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost("sitecore.net")
          .Credentials(this.adminCredentials)
          .DefaultMediaResourceExtension(null)
          .BuildSession(handler, httpClient)
        )
      {
        Assert.IsNotNull(session);
      }
    }
    #endregion Validate Null

   
  }
}

