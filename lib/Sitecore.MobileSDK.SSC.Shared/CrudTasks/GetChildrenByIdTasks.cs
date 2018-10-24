namespace Sitecore.MobileSDK.CrudTasks
{
  using System.Net.Http;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.UrlBuilder.Children;
  using Sitecore.MobileSDK.API.Items;

  internal class GetChildrenByIdTasks : AbstractGetItemTask<IReadItemsByIdRequest, ScItemsResponse>
  {
    public GetChildrenByIdTasks(ChildrenByIdUrlBuilder urlBuilder, HttpClient httpClient)
      : base(httpClient)
    {
      this.urlBuilder = urlBuilder;
    }

    protected override string UrlToGetItemWithRequest(IReadItemsByIdRequest request)
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
    private readonly ChildrenByIdUrlBuilder urlBuilder;
  }
}

