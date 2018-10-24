namespace Sitecore.MobileSDK.Items
{
  using System;
  using System.Collections.Generic;
  using System.Threading;
  using Newtonsoft.Json;
  using Newtonsoft.Json.Linq;
  using Sitecore.MobileSDK.API.Exceptions;
  using Sitecore.MobileSDK.API.Items;

  public class DeleteItemsResponseParser
  {
    public static ScDeleteItemsResponse ParseResponse(string response, CancellationToken token)
    {
      token.ThrowIfCancellationRequested();

      if (string.IsNullOrEmpty(response)) {
        throw new ArgumentException("response shouldn't be empty or null");
      }

      return new ScDeleteItemsResponse(response);
    }

  }
}