
namespace Sitecore.MobileSDK.CrudTasks.Entity
{
  using System;
  using System.Net.Http;
  using System.Diagnostics;
  using System.Threading;
  using System.Threading.Tasks;

  using Sitecore.MobileSDK.TaskFlow;
  using Sitecore.MobileSDK.Entities;
  using Sitecore.MobileSDK.API.Entities;

  internal abstract class AbstractGetEntityTask<TRequest, TResponse> : IRestApiCallTasks<TRequest, HttpRequestMessage, string, TResponse>
      where TRequest : class
      where TResponse : class
  {

    private AbstractGetEntityTask()
    {
    }

    public AbstractGetEntityTask(HttpClient httpClient)
    {
      this.httpClient = httpClient;

      this.Validate();
    }

    #region  IRestApiCallTasks

    public virtual HttpRequestMessage BuildRequestUrlForRequestAsync(TRequest request, CancellationToken cancelToken)
    {
      string url = this.UrlToGetEntityWithRequest(request);
      HttpRequestMessage result = new HttpRequestMessage(HttpMethod.Get, url);

      return result;
    }

    public virtual async Task<string> SendRequestForUrlAsync(HttpRequestMessage requestUrl, CancellationToken cancelToken)
    {
      //TODO: @igk debug request output, remove later
      Debug.WriteLine("REQUEST: " + requestUrl);
      HttpResponseMessage httpResponse = await this.httpClient.SendAsync(requestUrl, cancelToken);

      this.statusCode = (int)httpResponse.StatusCode;

      return await httpResponse.Content.ReadAsStringAsync();
    }

    public virtual async Task<TResponse> ParseResponseDataAsync(string data, CancellationToken cancelToken)
    {
      Func<ScEntityResponse> syncParseResponse = () =>
      {
        //TODO: @igk debug response output, remove later
        //Debug.WriteLine("RESPONSE: " + data);

        return ScReadEntitiesParser.Parse(data, this.statusCode, cancelToken);
      };
      return await Task.Factory.StartNew(syncParseResponse, cancelToken) as TResponse;
    }

    #endregion IRestApiCallTasks

    private void Validate()
    {
      if (null == this.httpClient)
      {
        throw new ArgumentNullException("AbstractGetEntityTask.httpClient cannot be null");
      }

    }

    protected int statusCode = 0;

    protected abstract string UrlToGetEntityWithRequest(TRequest request);

    protected HttpClient httpClient;
  }
}




