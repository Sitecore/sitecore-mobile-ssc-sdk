namespace Sitecore.MobileSDK.SessionSettings
{
  using System;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.MediaItem;

  public class SessionConfigPOD : ISessionConfig, IMediaLibrarySettings
  {
    private SessionConfigPOD SessionConfigPODCopy()
    {
      SessionConfigPOD result = new SessionConfigPOD();
      result.InstanceUrl = this.InstanceUrl;
      result.MediaLibraryRoot = this.MediaLibraryRoot;
      result.DefaultMediaResourceExtension = this.DefaultMediaResourceExtension;
      result.MediaPrefix = this.MediaPrefix;

      return result;
    }

    public virtual ISessionConfig SessionConfigShallowCopy()
    {
      return this.SessionConfigPODCopy();
    }

    public virtual IMediaLibrarySettings MediaSettingsShallowCopy()
    {
      return this.SessionConfigPODCopy();
    }

    #region ISessionConfig
    public string InstanceUrl { get; set; }

    public string MediaLibraryRoot { get; set; }

    public string DefaultMediaResourceExtension
    {
      get;
      set;
    }

    public string MediaPrefix
    {
      get;
      set;
    }

    #endregion ISessionConfig


    #region Comparator
    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals(this, obj))
      {
        return true;
      }

      SessionConfigPOD other = (SessionConfigPOD)obj;
      if (null == other)
      {
        return false;
      }


      bool isUrlEqual = object.Equals(this.InstanceUrl, other.InstanceUrl);

      return isUrlEqual;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() + this.InstanceUrl.GetHashCode();
    }
    #endregion Comparator
  }
}
