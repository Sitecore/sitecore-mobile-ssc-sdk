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

    public static ScCreateEntityResponse Parse(string responseString, CancellationToken cancelToken)
    {
      if (string.IsNullOrEmpty(responseString))
      {
        throw new ArgumentException("response cannot null or empty");
      }

      var response = JToken.Parse(responseString);


      ScEntity newItem = ScEntitiesParser.ParseSource(response as JObject, cancelToken);

      return new ScCreateEntityResponse(newItem);
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

  }
}