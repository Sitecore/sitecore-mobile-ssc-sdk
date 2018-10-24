
namespace Sitecore.MobileSDK.CrudTasks.Entity
{
  using System;
  using System.Net.Http;
  using System.Threading;
  using System.Threading.Tasks;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.UrlBuilder.Entity;

  internal class DeleteEntityTask : AbstractGetEntityTask<IDeleteEntityRequest, ScDeleteEntityResponse>
  {
    public DeleteEntityTask(EntityByIdUrlBuilder<IDeleteEntityRequest> urlBuilder, HttpClient httpClient)
      : base(httpClient)
    {
      this.urlBuilder = urlBuilder;
    }

    public override HttpRequestMessage BuildRequestUrlForRequestAsync(IDeleteEntityRequest request, CancellationToken cancelToken)
    {
      string url = this.UrlToGetEntityWithRequest(request);
      HttpRequestMessage result = new HttpRequestMessage(HttpMethod.Delete, url);

      return result;
    }

    protected override string UrlToGetEntityWithRequest(IDeleteEntityRequest request)
    {
      return this.urlBuilder.GetUrlForRequest(request);
    }

    public override async Task<ScDeleteEntityResponse> ParseResponseDataAsync(string data, CancellationToken cancelToken)
    {
      Func<ScDeleteEntityResponse> syncParseResponse = () => {
        return new ScDeleteEntityResponse(this.statusCode);
      };
      return await Task.Factory.StartNew(syncParseResponse, cancelToken);
    }

    private readonly EntityByIdUrlBuilder<IDeleteEntityRequest> urlBuilder;

  }
}
