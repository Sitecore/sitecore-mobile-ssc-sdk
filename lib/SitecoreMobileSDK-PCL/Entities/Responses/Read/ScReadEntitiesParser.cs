
namespace Sitecore.MobileSDK.Entities
{
  using System.Collections.Generic;
  using System.Threading;
  using Newtonsoft.Json.Linq;
  using Sitecore.MobileSDK.API.Entities;

  public static class ScReadEntitiesParser
  {

    public static ScEntityResponse Parse(string responseString, int statusCode, CancellationToken cancelToken)
    {
      if (string.IsNullOrEmpty(responseString))
      {
        return new ScEntityResponse(new List<ISitecoreEntity>(), statusCode);
      }

      var response = JToken.Parse(responseString);

      var items = new List<ISitecoreEntity>();

      JToken results = null;

      try
      {
        results = response.Value<JToken>("value");


        if (results != null) {
          response = results;
        }

        if (response is JArray) {
          foreach (JObject item in response) {
            cancelToken.ThrowIfCancellationRequested();

            ScEntity newItem = ScEntityParser.ParseSource(item, cancelToken);
            items.Add(newItem);
          }
        } else if (response is JObject) {
          ScEntity newItem = ScEntityParser.ParseSource(response as JObject, cancelToken);
          items.Add(newItem);
        }
      }
      catch {
        //ScEntityResponse will be created any way
      }

      return new ScEntityResponse(items, statusCode);
    }

  }
}