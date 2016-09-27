namespace Sitecore.MobileSDK.Items
{
  using System;
  using System.Collections.Generic;
  using System.Threading;
  using Newtonsoft.Json;
  using Newtonsoft.Json.Linq;
  using Sitecore.MobileSDK.API.Exceptions;
  using Sitecore.MobileSDK.API.Items;

  public class CreateItemResponseParser
  {
    public static ScCreateItemResponse ParseResponse(string response, CancellationToken token)
    {
      token.ThrowIfCancellationRequested();

      if (string.IsNullOrEmpty(response)) {
        throw new ArgumentException("response shouldn't be empty or null");
      }
      //response example
      //http://cms80u2.test24dk1.dk.sitecore.net/sitecore/api/ssc/item/a62679f1-3948-48bd-ae69-c90adbf8f8d0?Database=web

      string beforeSubstring = "item/";
      string afterSubstring = "?";

      int pFrom = response.IndexOf(beforeSubstring, StringComparison.CurrentCultureIgnoreCase) + beforeSubstring.Length;
      int pTo = response.LastIndexOf(afterSubstring, StringComparison.CurrentCultureIgnoreCase);

      string result = response.Substring(pFrom, pTo - pFrom);

      return new ScCreateItemResponse(result);
    }
  }
}