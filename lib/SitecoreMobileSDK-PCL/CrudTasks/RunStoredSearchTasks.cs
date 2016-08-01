
namespace Sitecore.MobileSDK.CrudTasks
{
  using System.Net.Http;
  using Sitecore.MobileSDK.PublicKey;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.UrlBuilder.Search;

  internal class RunStoredSearchTasks : AbstractGetItemTask<ISitecoreStoredSearchRequest>
  {
    public RunStoredSearchTasks(RunStoredSearchUrlBuilder urlBuilder, HttpClient httpClient)
      : base(httpClient)
    {
      this.urlBuilder = urlBuilder;
    }

    protected override string UrlToGetItemWithRequest(ISitecoreStoredSearchRequest request)
    {
      this.privateDb = request.ItemSource.Database;
      return this.urlBuilder.GetUrlForRequest(request);
    }

    public override string CurrentDb {
      get {
        return this.privateDb;
      }
    }

    private string privateDb = null;
    private readonly RunStoredSearchUrlBuilder urlBuilder;
  }
}

