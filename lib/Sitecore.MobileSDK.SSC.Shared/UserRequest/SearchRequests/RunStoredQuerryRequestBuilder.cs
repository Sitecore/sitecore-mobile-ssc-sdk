using Sitecore.MobileSDK.API.Request.Parameters;

namespace Sitecore.MobileSDK.UserRequest.SearchRequest
{
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.Items;
  using Sitecore.MobileSDK.Validators;

  public class RunStoredQuerryRequestBuilder : AbstractGetPagedItemRequestBuilder<ISitecoreStoredSearchRequest>
  {
    public RunStoredQuerryRequestBuilder(string itemId)
    {
      ItemIdValidator.ValidateItemId(itemId, this.GetType().Name + ".ItemId");

      this.itemId = itemId;
    }

    public override ISitecoreStoredSearchRequest Build()
    {
      IPagingParameters pagingSettings = this.AccumulatedPagingParameters;

      StoredSearchParameters result = new StoredSearchParameters(
        null,
        this.itemSourceAccumulator,
        this.queryParameters,
        this.AccumulatedPagingParameters,
        this.itemId,
        this.icludeStanderdTemplateFields,
        null);


      return result;
    }

    private readonly string itemId;
  }
}

