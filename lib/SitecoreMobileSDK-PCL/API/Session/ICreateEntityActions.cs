namespace Sitecore.MobileSDK.API.Session
{
  using System.Threading;
  using System.Threading.Tasks;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request.Entity;

  public interface ICreateEntityActions
  {
    Task<ScCreateEntityResponse> CreateEntityAsync(ICreateEntityRequest request, CancellationToken cancelToken = default(CancellationToken));
  }

}
