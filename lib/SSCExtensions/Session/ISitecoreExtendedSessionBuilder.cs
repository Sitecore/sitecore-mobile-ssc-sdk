using System;
using Sitecore.MobileSDK.API.Session;

namespace SSCExtensions
{
 public interface ISitecoreExtendedSessionBuilder
  {

    IExtendedSession Build();

    ISitecoreExtendedSessionBuilder PathForTemporaryItems(string sscVersion);

    ISitecoreExtendedSessionBuilder DefaultTemporaryItemName(string defaultDatabase);

  }
}
