namespace Sitecore.MobileSDK.CrudTasks
{
  using System;
  using System.Threading.Tasks;
  using System.Net.Http;
  using System.Threading;
  using System.Text;
  using Sitecore.MobileSDK.API.Items;
  using System.Diagnostics;

  internal abstract class AbstractUpdateItemTask<TRequest> : AbstractGetItemTask<TRequest, ScUpdateItemResponse>
    where TRequest : class
  {
    public AbstractUpdateItemTask(HttpClient httpClient)
      : base(httpClient)
    {

    }

    public override HttpRequestMessage BuildRequestUrlForRequestAsync(TRequest request, CancellationToken cancelToken)
    {
      string url = this.UrlToGetItemWithRequest(request);
      HttpRequestMessage result = new HttpRequestMessage(new HttpMethod("PATCH"), url);

      string fieldsList = this.GetFieldsListString(request);
      StringContent bodycontent = new StringContent(fieldsList, Encoding.UTF8, "application/json");
      result.Content = bodycontent;

      return result;
    }

    public override async Task<string> SendRequestForUrlAsync(HttpRequestMessage requestUrl, CancellationToken cancelToken)
    {
      //TODO: @igk debug request output, remove later
      Debug.WriteLine("REQUEST: " + requestUrl);

      HttpResponseMessage httpResponse = await this.httpClient.SendAsync(requestUrl, cancelToken);
      int code = (int)httpResponse.StatusCode;

      return code.ToString();
    }

    public override async Task<ScUpdateItemResponse> ParseResponseDataAsync(string data, CancellationToken cancelToken)
    {
      Func<ScUpdateItemResponse> syncParseResponse = () => {
        return new ScUpdateItemResponse(data);
      };
      return await Task.Factory.StartNew(syncParseResponse, cancelToken);
    }

    public abstract string GetFieldsListString(TRequest request);

  }
}

