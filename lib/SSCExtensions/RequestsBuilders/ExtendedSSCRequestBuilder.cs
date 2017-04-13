using System;
using System.Collections.Generic;
using Sitecore.MobileSDK.API.Items;
using Sitecore.MobileSDK.API.Request;
using Sitecore.MobileSDK.Items;
using Sitecore.MobileSDK.UserRequest.SearchRequest;

namespace SSCExtensions
{
  public static class ExtendedSSCRequestBuilder
  {

    public static SitecoreQueryRequestBuilder SitecoreQueryRequest(string sitecoreQuery)
    {
      return new SitecoreQueryRequestBuilder(sitecoreQuery);
    }

    public static IDeleteItemRequestBuilder<IDeleteItemsListRequest> DeleteItemsListRequest(IEnumerable<ISitecoreItem> itemsList)
    {
      return new DeleteItemsListRequestBuilder(itemsList);
    }

  }
}
