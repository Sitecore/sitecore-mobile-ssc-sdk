
namespace Sitecore.MobileSDK.API.Session
{
  using System.IO;
  using System.Threading;
  using System.Threading.Tasks;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.API.Request;

  public interface ISearchActions
  {

    Task<ScItemsResponse> RunStoredQuerryAsync(ISitecoreStoredSearchRequest request, CancellationToken cancelToken = default(CancellationToken));

    Task<ScItemsResponse> RunSearchAsync(ISitecoreSearchRequest request, CancellationToken cancelToken = default(CancellationToken));

    Task<ScItemsResponse> RunSearchAsync(ISitecoreStoredSearchRequest request, CancellationToken cancelToken = default(CancellationToken));

  }
}

