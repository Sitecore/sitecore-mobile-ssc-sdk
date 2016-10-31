
namespace Sitecore.MobileSDK.CrudTasks.Entity
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using System.Net.Http;
  using System.Text;
  using System.Threading;
  using System.Threading.Tasks;
  using Newtonsoft.Json;
  using Newtonsoft.Json.Linq;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.Entities;
  using Sitecore.MobileSDK.Items;
  using Sitecore.MobileSDK.TaskFlow;
  using Sitecore.MobileSDK.UrlBuilder.Entity;

  internal class CreateEntityhTask<T> : IRestApiCallTasks<T, HttpRequestMessage, string, ScCreateEntityResponse>
    where T : class, ICreateEntityRequest
  {
    private readonly EntityByPathUrlBuilder createEntityBuilder;
    private readonly HttpClient httpClient;

    public CreateEntityhTask(
      EntityByPathUrlBuilder createEntityBuilder,
      HttpClient httpClient)
    {
      this.createEntityBuilder = createEntityBuilder;
      this.httpClient = httpClient;

      this.Validate();
    }

    public HttpRequestMessage BuildRequestUrlForRequestAsync(T request, CancellationToken cancelToken)
    {
      var url = this.createEntityBuilder.GetUrlForRequest(request);

      HttpRequestMessage result = new HttpRequestMessage(HttpMethod.Post, url);

      string fieldsList = this.GetFieldsListString(request);
      StringContent bodycontent = new StringContent(fieldsList, Encoding.UTF8, "application/json");
      result.Content = bodycontent;

      return result;

    }

    public async Task<string> SendRequestForUrlAsync(HttpRequestMessage request, CancellationToken cancelToken)
    {
      //TODO: @igk debug request output, remove later
      Debug.WriteLine("REQUEST: " + request);
      var result = await this.httpClient.SendAsync(request, cancelToken);

      IEnumerable<string> headerValues = result.Headers.GetValues("Location");
      return headerValues.FirstOrDefault();
    }

    public async Task<ScCreateEntityResponse> ParseResponseDataAsync(string httpData, CancellationToken cancelToken)
    {
      Func<ScCreateEntityResponse> syncParseResponse = () =>
      {
        //TODO: @igk debug response output, remove later
        Debug.WriteLine("RESPONSE: " + httpData);
        return ScEntitiesParser.Parse(httpData, cancelToken);
      };
      return await Task.Factory.StartNew(syncParseResponse, cancelToken);
    }

    public string GetFieldsListString(ICreateItemByPathRequest request)
    {
      string result = string.Empty;

      JObject jsonObject = new JObject();

      bool fieldsAvailable = (null != request.FieldsRawValuesByName);
      if (fieldsAvailable) {
        fieldsAvailable = (request.FieldsRawValuesByName.Count > 0);
      }

      //TODO: IGK refactor this

      if (fieldsAvailable) {
        foreach (var fieldElem in request.FieldsRawValuesByName) {
          jsonObject.Add(fieldElem.Key, fieldElem.Value);
        }
      }

      //TODO: IGK check do we need some fields more. Documentation have no such content.
      jsonObject.Add("TemplateID", request.ItemTemplateId);
      jsonObject.Add("ItemName", request.ItemName);

      result = jsonObject.ToString(Formatting.None);

      return result;
    }

    private void Validate()
    {
      if (null == this.httpClient)
      {
        throw new ArgumentNullException("DeleteItemTasks.httpClient cannot be null");
      }

      if (null == this.createItemBuilder)
      {
        throw new ArgumentNullException("DeleteItemTasks.deleteItemsBuilder cannot be null");
      }
    }
  }
}
