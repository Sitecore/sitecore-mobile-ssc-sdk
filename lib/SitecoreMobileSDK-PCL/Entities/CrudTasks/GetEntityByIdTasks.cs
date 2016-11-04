
namespace Sitecore.MobileSDK.CrudTasks.Entity
{
  using System.Net.Http;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.UrlBuilder.Entity;

  internal class GetEntityByIdTasks : AbstractGetEntityTask<IReadEntityByIdRequest, ScEntityResponse>
  {
    public GetEntityByIdTasks(EntityByIdUrlBuilder<IReadEntityByIdRequest> urlBuilder, HttpClient httpClient)
      : base(httpClient)
    {
      this.urlBuilder = urlBuilder;
    }

    protected override string UrlToGetEntityWithRequest(IReadEntityByIdRequest request)
    {
      return this.urlBuilder.GetUrlForRequest(request);
    }

    private readonly EntityByIdUrlBuilder<IReadEntityByIdRequest> urlBuilder;

  }
}
