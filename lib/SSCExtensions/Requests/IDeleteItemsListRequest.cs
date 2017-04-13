using System;
using System.Collections.Generic;
using Sitecore.MobileSDK.API.Items;
using Sitecore.MobileSDK.API.Request;

namespace SSCExtensions
{
  public interface IDeleteItemsListRequest : IBaseDeleteItemRequest
  {
    IDeleteItemsListRequest DeepCopyDeleteItemListRequest();

    IEnumerable<ISitecoreItem> ItemsList{ get; }
  }
}
