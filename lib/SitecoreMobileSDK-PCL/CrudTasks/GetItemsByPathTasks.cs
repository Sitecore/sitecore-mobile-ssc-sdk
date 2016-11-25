namespace Sitecore.MobileSDK.CrudTasks
{
  using System.Net.Http;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.UrlBuilder.ItemByPath;
  using Sitecore.MobileSDK.API.Items;

  internal class GetItemsByPathTasks : AbstractGetItemTask<IReadItemsByPathRequest, ScItemsResponse>
  {
    public GetItemsByPathTasks(ItemByPathUrlBuilder urlBuilder, HttpClient httpClient)
      : base(httpClient)
    {
      this.urlBuilder = urlBuilder;
    }
    protected override string UrlToGetItemWithRequest(IReadItemsByPathRequest request)
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
    private readonly ItemByPathUrlBuilder urlBuilder;
  }
}
