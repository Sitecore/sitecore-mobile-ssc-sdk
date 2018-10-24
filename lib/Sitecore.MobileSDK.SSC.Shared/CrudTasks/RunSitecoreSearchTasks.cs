
namespace Sitecore.MobileSDK.CrudTasks
{
  using System.Net.Http;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.UrlBuilder.Search;
  using Sitecore.MobileSDK.API.Items;

  internal class RunSitecoreSearchTasks : AbstractGetItemTask<ISitecoreSearchRequest, ScItemsResponse>
  {
    public RunSitecoreSearchTasks(RunSitecoreSearchUrlBuilder urlBuilder, HttpClient httpClient)
      : base(httpClient)
    {
      this.urlBuilder = urlBuilder;
    }

    protected override string UrlToGetItemWithRequest(ISitecoreSearchRequest request)
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
    private readonly RunSitecoreSearchUrlBuilder urlBuilder;
  }
}

