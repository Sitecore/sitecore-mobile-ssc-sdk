
namespace Sitecore.MobileSDK.API.Session
{
  using System.Threading;
  using System.Threading.Tasks;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request.Entity;

  public interface IReadEntityActions
  {
    Task<ScEntityResponse> ReadEntityAsync(IReadEntitiesByPathRequest request, CancellationToken cancelToken = default(CancellationToken));
    Task<ScEntityResponse> ReadEntityAsync(IReadEntityByIdRequest request, CancellationToken cancelToken = default(CancellationToken));

  }
}

