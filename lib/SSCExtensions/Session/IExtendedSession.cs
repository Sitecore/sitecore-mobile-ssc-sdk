using System.Threading;
using System.Threading.Tasks;
using Sitecore.MobileSDK.API.Items;
using Sitecore.MobileSDK.Items;

namespace SSCExtensions
{
  public interface IExtendedSession
  {
    Task<ScItemsResponse> SearchBySitecoreQueryAsync(SitecoreSearchParameters querySearchRequest, CancellationToken cancelToken = default(CancellationToken));
    Task<ScDeleteItemsListResponse> DeleteItemsAsync(IDeleteItemsListRequest deleteItemsListRequest, CancellationToken cancelToken = default(CancellationToken));
    Task<ScDeleteItemsListResponse> DeleteItemsAsync(IDeleteItemsBySitecoreQueryRequest deleteItemsBySitecoreQueryRequest, CancellationToken cancelToken = default(CancellationToken));
  }
}