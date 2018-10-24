namespace Sitecore.MobileSDK.UrlBuilder
{
  using System;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.UrlBuilder.Rest;
  using Sitecore.MobileSDK.UrlBuilder.SSC;


  public abstract class GetItemsUrlBuilder<TRequest> : AbstractGetItemUrlBuilder<TRequest>
    where TRequest : IBaseItemRequest
  {
    public GetItemsUrlBuilder(IRestServiceGrammar restGrammar, ISSCUrlParameters sscGrammar) : 
    base(restGrammar, sscGrammar)
    {
    }

    protected override string GetSpecificPartForRequest(TRequest request)
    {
      return this.GetItemIdenticationForRequest(request);

    }
 
    protected abstract string GetItemIdenticationForRequest(TRequest request);

  }
}

