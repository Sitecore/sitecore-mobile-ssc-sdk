﻿using Sitecore.MobileSDK.API.Request.Parameters;

namespace Sitecore.MobileSDK.UserRequest.ReadRequest
{
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.Items;
  using Sitecore.MobileSDK.Validators;

  public class ReadChildrenByIdRequestBuilder : AbstractGetVersionedItemRequestBuilder<IReadItemsByIdRequest>
  {
    public ReadChildrenByIdRequestBuilder(string itemId)
    {
      ItemIdValidator.ValidateItemId(itemId, this.GetType().Name + ".ItemId");

      this.itemId = itemId;
    }

    public override IReadItemsByIdRequest Build()
    {
      IPagingParameters pagingSettings = this.AccumulatedPagingParameters;

      ReadItemsByIdParameters result = new ReadItemsByIdParameters(
        null, 
        this.itemSourceAccumulator, 
        this.queryParameters, 
        this.resultIncludeStandardTemplateFields,
        this.itemId);

      return result;
    }

    private readonly string itemId;
  }
}

