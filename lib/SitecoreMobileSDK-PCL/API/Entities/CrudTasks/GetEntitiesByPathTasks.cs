
namespace Sitecore.MobileSDK.CrudTasks.Entity
{
  using System.Net.Http;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.UrlBuilder.Entity;

  internal class GetEntitiesByPathTasks : AbstractGetEntityTask<IReadEntitiesByPathRequest, ScEntityResponse>
  {
    public GetEntitiesByPathTasks(EntityByPathUrlBuilder urlBuilder, HttpClient httpClient)
      : base(httpClient)
    {
      this.urlBuilder = urlBuilder;
    }
    protected override string UrlToGetEntityWithRequest(IReadEntitiesByPathRequest request)
    {
      return this.urlBuilder.GetUrlForRequest(request);
    }

    private readonly EntityByPathUrlBuilder urlBuilder;

  }
}
