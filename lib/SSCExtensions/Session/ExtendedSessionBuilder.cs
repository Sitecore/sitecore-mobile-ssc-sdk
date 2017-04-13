using System;
using Sitecore.MobileSDK.API.Session;

namespace SSCExtensions
{
  public static class ExtendedSessionBuilder
  {
      public static ISitecoreExtendedSessionBuilder ExtendedSessionWith(ISitecoreSSCSession session)
      {
        var result = new SitecoreExtendedSessionBuilder(session);
        return result;
      }
  }
}
