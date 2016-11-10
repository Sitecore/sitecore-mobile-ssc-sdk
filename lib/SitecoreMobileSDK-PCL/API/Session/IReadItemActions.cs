
namespace Sitecore.MobileSDK.API.Session
{
  using System.IO;
  using System.Threading;
  using System.Threading.Tasks;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.API.Request;

  /// <summary>
  /// Interface represents read actions that retrieve items.
  /// </summary>
  public interface IReadItemActions
  {
    Task<ScItemsResponse> ReadItemAsync(IReadItemsByIdRequest request, CancellationToken cancelToken = default(CancellationToken));
    Task<ScItemsResponse> ReadChildrenAsync(IReadItemsByIdRequest request, CancellationToken cancelToken = default(CancellationToken));
    Task<ScItemsResponse> ReadItemAsync(IReadItemsByPathRequest request, CancellationToken cancelToken = default(CancellationToken));

   }
}

