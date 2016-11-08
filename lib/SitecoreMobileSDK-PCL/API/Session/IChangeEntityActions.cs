namespace Sitecore.MobileSDK.API.Session
{
  using System.Threading;
  using System.Threading.Tasks;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request.Entity;

  public interface IChangeEntityActions
  {
    Task<ScCreateEntityResponse> CreateEntityAsync(ICreateEntityRequest request, CancellationToken cancelToken = default(CancellationToken));
    Task<ScUpdateEntityResponse> UpdateEntityAsync(IUpdateEntityRequest request, CancellationToken cancelToken = default(CancellationToken));
    Task<ScDeleteEntityResponse> DeleteEntityAsync(IDeleteEntityRequest request, CancellationToken cancelToken = default(CancellationToken));
  }

}
