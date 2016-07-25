namespace Sitecore.MobileSDK.MockObjects
{
  using Sitecore.MobileSDK.SessionSettings;

  public class MutableSessionConfig : SessionConfig
  {
    public MutableSessionConfig(
      string instanceUrl)
    : base(instanceUrl)
    {
    }

    public override SessionConfig ShallowCopy()
    {
      var result = new MutableSessionConfig(
        "mock instance");

      // @adk : skipping validation
      result.SetInstanceUrl(this.InstanceUrl);

      return result;
    }

    public void SetInstanceUrl(string value)
    {
      this.InstanceUrl = value;
    }

  }
}

