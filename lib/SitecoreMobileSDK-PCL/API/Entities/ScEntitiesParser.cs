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

  public class ScEntitiesParser
  {
    private ScEntitiesParser()
    {
    }

    public static ScEntityResponse Parse(string responseString, CancellationToken cancelToken)
    {
      if (string.IsNullOrEmpty(responseString))
      {
        throw new ArgumentException("response cannot null or empty");
      }

      var response = JToken.Parse(responseString);

      var items = new List<ISitecoreEntity>();

      //FIXME: @igk to manny result variants, do we still need universal parser?

      JToken results = null;

      try
      {
        results = response.Value<JToken>("Results");
      }
      catch(Exception e)
      {
        
      }

      if ( results != null)
      {
        response = results;
      }

      if (response is JArray)
      {
        foreach (JObject item in response)
        {
          cancelToken.ThrowIfCancellationRequested();

          ScEntity newItem = ScEntitiesParser.ParseSource(item, cancelToken);
          items.Add(newItem);
        }
      }
      else if (response is JObject)
      {
        ScEntity newItem = ScEntitiesParser.ParseSource(response as JObject, cancelToken);
        items.Add(newItem);
      }

      return new ScEntityResponse(items);
    }

    public static ScEntity ParseSource(JObject item, CancellationToken cancelToken)
    {
      ScEntity newItem;

      try {

        List<IField> fields = ScFieldsParser.ParseFieldsData(item, cancelToken);
        var fieldsByName = new Dictionary<string, IField>(fields.Count);
        foreach (IField singleField in fields) {
          cancelToken.ThrowIfCancellationRequested();

          string lowercaseName = singleField.Name.ToLowerInvariant();
          fieldsByName[lowercaseName] = singleField;
        }

        newItem = new ScEntity(fieldsByName);
      } catch (Exception e) {
        OperationCanceledException cancel = e as OperationCanceledException;
        if (cancel != null) { 
          throw cancel; 
        }
          
        throw new ParserException(TaskFlowErrorMessages.PARSER_EXCEPTION_MESSAGE + item.ToString(), e);
      }

      return newItem;
    }

    private static T ParseOrFail<T>(JObject json, string path)
    {
      JToken objectToken = json.SelectToken(path);
      if (null == objectToken)
      {
        throw new JsonException("Invalid json");
      }

      return objectToken.Value<T>();
    }
  }
}