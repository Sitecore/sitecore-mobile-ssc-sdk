﻿namespace Sitecore.MobileSDK.UrlBuilder.UpdateItem
{
  using Sitecore.MobileSDK.Utils;
  using Sitecore.MobileSDK.UrlBuilder.Rest;
  using Sitecore.MobileSDK.UrlBuilder.SSC;
  using Sitecore.MobileSDK.Validators;
  using Sitecore.MobileSDK.API.Request;

  public class UpdateItemByIdUrlBuilder : AbstractUpdateItemUrlBuilder<IUpdateItemByIdRequest>
  {
    public UpdateItemByIdUrlBuilder(IRestServiceGrammar restGrammar, ISSCUrlParameters sscGrammar)
      : base(restGrammar, sscGrammar)
    {
    }

    protected override string GetHostUrlForRequest(IUpdateItemByIdRequest request)
    {
      string hostUrl = base.GetHostUrlForRequest(request);
      string itemId = UrlBuilderUtils.EscapeDataString(request.ItemId.ToLowerInvariant());

      string result = hostUrl + this.restGrammar.PathComponentSeparator + itemId;
      return result;
    }

    protected override string GetSpecificPartForRequest(IUpdateItemByIdRequest request)
    {
      return string.Empty;
    }

    protected override void ValidateSpecificRequest(IUpdateItemByIdRequest request)
    {
      ItemIdValidator.ValidateItemId(request.ItemId, this.GetType().Name + ".ItemId");
    }
  }
}

