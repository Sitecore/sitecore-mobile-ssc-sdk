namespace Sitecore.MobileSdkUnitTest
{
  using System;
  using NUnit.Framework;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.SessionSettings;
  using Sitecore.MobileSDK.UserRequest;

  [TestFixture]
  public class SessionConfigMergerTest
  {
    [Test]
    public void TestMergerRequiresDefaultValues()
    {
      Assert.Throws<ArgumentNullException>(() => new SessionConfigMerger(null));
    }

    [Test]
    public void TestUrlMustBeSetOnDefaultConfig()
    {
      var noInstanceUrl = new SessionConfigPOD();
      noInstanceUrl.InstanceUrl = null;

      Assert.Throws<ArgumentNullException>(() => new SessionConfigMerger(noInstanceUrl));
    }

    [Test]
    public void TestMergerReturnsDefaultSourceForNilInput()
    {
      var defaultConfig = new SessionConfigPOD();
      defaultConfig.InstanceUrl = "sitecore.net";


      var merger = new SessionConfigMerger(defaultConfig);
      ISessionConfig result = merger.FillSessionConfigGaps(null);

      Assert.AreSame(defaultConfig, result);
    }


    [Test]
    public void TestUserFieldsHaveHigherPriority()
    {
      var defaultConfig = new SessionConfigPOD();
      defaultConfig.InstanceUrl = "sitecore.net";

      var userConfig = new SessionConfigPOD();
      userConfig.InstanceUrl = "http://localhost:80";

      var merger = new SessionConfigMerger(defaultConfig);
      ISessionConfig result = merger.FillSessionConfigGaps(userConfig);

      Assert.AreEqual(userConfig, result);
      Assert.AreNotSame(userConfig, result);
    }

    [Test]
    public void TestNullUserFieldsAreAutocompleted()
    {
      var defaultConfig = new SessionConfigPOD();
      defaultConfig.InstanceUrl = "sitecore.net";

      var userConfig = new SessionConfigPOD();
      userConfig.InstanceUrl = null;

      var merger = new SessionConfigMerger(defaultConfig);
      ISessionConfig result = merger.FillSessionConfigGaps(userConfig);

      Assert.AreEqual(defaultConfig, result);
      Assert.AreNotSame(defaultConfig, result);
    }
  }
}

