namespace Sitecore.MobileSDK.Items
{
  using System;
  using System.Collections.Generic;
  using System.Threading;
  using Newtonsoft.Json;
  using Newtonsoft.Json.Linq;
  using Sitecore.MobileSDK.API.Exceptions;
  using Sitecore.MobileSDK.API.Fields;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.Items.Fields;
  using Sitecore.MobileSDK.Session;

  public class ScItemsParser
  {
    private ScItemsParser()
    {
    }

    public static ScItemsResponse Parse(string responseString, string db, CancellationToken cancelToken)
    {
      return ScItemsParser.Parse(responseString, db, 0, cancelToken);
    }

    public static ScItemsResponse Parse(string responseString, string db, int responseCode, CancellationToken cancelToken)
    {
      if (string.IsNullOrEmpty(responseString))
      {
        return new ScItemsResponse(null, responseCode);
      }

      var response = JToken.Parse(responseString);

      var items = new List<ISitecoreItem>();

      //FIXME: @igk to manny result variants, do we still need universal parser?

      JToken results = null;

      try
      {
        results = response.Value<JToken>("Results");
      }
      catch(Exception)
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

          ScItem newItem = ScItemsParser.ParseSource(item, db, cancelToken);
          items.Add(newItem);
        }
      }
      else if (response is JObject)
      {
        ScItem newItem = ScItemsParser.ParseSource(response as JObject, db, cancelToken);
        items.Add(newItem);
      }

      return new ScItemsResponse(items, responseCode);
    }

    public static ScItem ParseSource(JObject item, string db, CancellationToken cancelToken)
    {
      ScItem newItem;

      try {
        var source = ParseItemSource(item, db);

        List<IField> fields = ScFieldsParser.ParseFieldsData(item, cancelToken);
        var fieldsByName = new Dictionary<string, IField>(fields.Count);
        foreach (IField singleField in fields) {
          cancelToken.ThrowIfCancellationRequested();

          string lowercaseName = singleField.Name.ToLowerInvariant();
          fieldsByName[lowercaseName] = singleField;
        }

        newItem = new ScItem(source, fieldsByName);
      } catch (Exception e) {
        OperationCanceledException cancel = e as OperationCanceledException;
        if (cancel != null) { 
          throw cancel; 
        }
          
        throw new ParserException(TaskFlowErrorMessages.PARSER_EXCEPTION_MESSAGE + item.ToString(), e);
      }

      return newItem;
    }

    private static ItemSource ParseItemSource(JObject json, string db)
    {
      var language = (string)json.GetValue("ItemLanguage");
      var version = (int)json.GetValue("ItemVersion");

      //TODO: DB nont available in responce, use value from request if possible, shpuld be fixed on server side
      return new ItemSource(db, language, version);
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