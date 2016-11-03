namespace Sitecore.MobileSDK.CrudTasks
{
  using System;
  using System.Net.Http;
  using System.Diagnostics;
  using System.Threading;
  using System.Threading.Tasks;

  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.Items;
  using Sitecore.MobileSDK.TaskFlow;
  using Sitecore.MobileSDK.PublicKey;

  internal abstract class AbstractGetItemTask<TRequest, TResponse> : IRestApiCallTasks<TRequest, HttpRequestMessage, string, TResponse>
      where TRequest : class
      where TResponse : class
  {

    private AbstractGetItemTask()
    {
    }

    public AbstractGetItemTask(HttpClient httpClient)
    {
      this.httpClient = httpClient;

      this.Validate();
    }

    #region  IRestApiCallTasks

    public virtual HttpRequestMessage BuildRequestUrlForRequestAsync(TRequest request, CancellationToken cancelToken)
    {
      string url = this.UrlToGetItemWithRequest(request);
      HttpRequestMessage result = new HttpRequestMessage(HttpMethod.Get, url);

      return result;
    }

    public virtual async Task<string> SendRequestForUrlAsync(HttpRequestMessage requestUrl, CancellationToken cancelToken)
    {
      //TODO: @igk debug request output, remove later
      Debug.WriteLine("REQUEST: " + requestUrl);
      HttpResponseMessage httpResponse = await this.httpClient.SendAsync(requestUrl, cancelToken);
      return await httpResponse.Content.ReadAsStringAsync();
    }

    public virtual async Task<TResponse> ParseResponseDataAsync(string data, CancellationToken cancelToken)
    {
      Func<ScItemsResponse> syncParseResponse = () =>
      {
        //TODO: @igk debug response output, remove later
        Debug.WriteLine("RESPONSE: " + data);
        return ScItemsParser.Parse(data, this.CurrentDb, cancelToken);
      };
      return await Task.Factory.StartNew(syncParseResponse, cancelToken) as TResponse;
    }

    #endregion IRestApiCallTasks

    private void Validate()
    {
      if (null == this.httpClient)
      {
        throw new ArgumentNullException("AbstractGetItemTask.httpClient cannot be null");
      }

    }

    public int HttpResponseStatusCode()
    {
      return this.statusCode;
    }

    private int statusCode = 0;

    public abstract string CurrentDb { get; }

    protected abstract string UrlToGetItemWithRequest(TRequest request);

    protected HttpClient httpClient;
  }
}




