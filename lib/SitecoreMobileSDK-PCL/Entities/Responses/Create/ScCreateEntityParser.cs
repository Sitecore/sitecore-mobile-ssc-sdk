namespace Sitecore.MobileSDK.Entities
{
  using System.Threading;
  using Newtonsoft.Json.Linq;
  using Sitecore.MobileSDK.API.Entities;


  public class ScCreateEntityParser
  {
    private ScCreateEntityParser()
    {
    }

    public static ScCreateEntityResponse Parse(string responseString, int statusCode, CancellationToken cancelToken)
    {
      if (string.IsNullOrEmpty(responseString))
      {
        return new ScCreateEntityResponse(null, statusCode);
      }

      var response = JToken.Parse(responseString);


      ScEntity newItem = ScEntityParser.ParseSource(response as JObject, cancelToken);

      return new ScCreateEntityResponse(newItem, statusCode);
    }

  }
}