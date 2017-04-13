using Sitecore.MobileSDK.API.Request.Parameters;

namespace Sitecore.MobileSDK.UserRequest.SearchRequest
{
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.Items;
  using Sitecore.MobileSDK.Validators;

  public class SitecoreQueryRequestBuilder : AbstractGetVersionedItemRequestBuilder<SitecoreSearchParameters>
  {
    public SitecoreQueryRequestBuilder(string sitecoreQuery)
    {
      this.sitecoreQuery = sitecoreQuery;
    }

    public override SitecoreSearchParameters Build()
    {
      IPagingParameters pagingSettings = this.AccumulatedPagingParameters;
      //
      SitecoreSearchParameters result = new SitecoreSearchParameters(
        null,
        this.itemSourceAccumulator,
        this.queryParameters,
        pagingSettings,
        null,
        this.icludeStanderdTemplateFields,
        this.sitecoreQuery);

      return result;
    }


    private string sitecoreQuery;
  }
}

