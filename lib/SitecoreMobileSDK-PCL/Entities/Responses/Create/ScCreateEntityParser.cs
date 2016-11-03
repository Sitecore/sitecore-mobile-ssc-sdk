namespace Sitecore.MobileSDK.Entities
{
  using System;
  using System.Collections.Generic;
  using System.Threading;
  using Newtonsoft.Json;
  using Newtonsoft.Json.Linq;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Exceptions;
  using Sitecore.MobileSDK.API.Fields;
  using Sitecore.MobileSDK.Items.Fields;
  using Sitecore.MobileSDK.Session;

  public class ScCreateEntityParser
  {
    private ScCreateEntityParser()
    {
    }

    public static ScCreateEntityResponse Parse(string responseString, int statusCode, CancellationToken cancelToken)
    {
      if (string.IsNullOrEmpty(responseString))
      {
        throw new ArgumentException("response cannot null or empty");
      }

      var response = JToken.Parse(responseString);


      ScEntity newItem = ScEntityParser.ParseSource(response as JObject, cancelToken);

      return new ScCreateEntityResponse(newItem, statusCode);
    }



  }
}