namespace Sitecore.MobileSdkUnitTest
{
  using System;
  using NUnit.Framework;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.SessionSettings;
  using Sitecore.MobileSDK.UserRequest;

  [TestFixture]
  public class EntityRouteMergerTest
  {
    [Test]
    public void TestMergerRequiresDefaultValues()
    {
      Assert.Throws<ArgumentNullException>(() => new EntitySourceMerger(null));
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


  
  }
}

