namespace Sitecore.MobileSDK.API.Session
{
  using System;
  using System.Threading.Tasks;
  using Items;
  using Request;

  /// <summary>
  /// Interface represents session to work with Sitecore Mobile SDK.
  /// </summary>
  public interface ISitecoreSSCSession :
    IDisposable,
    ISitecoreSSCSessionState,
    ISitecoreSSCReadonlySession,
    ISitecoreSSCSessionActions
  {
    
  }
}

