namespace Sitecore.MobileSDK.UrlBuilder.Entity
{
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.UrlBuilder.Rest;
  using Sitecore.MobileSDK.UrlBuilder.SSC;


  public class GetEntitiesUrlBuilder<TRequest> : AbstractGetEntityUrlBuilder<TRequest>
    where TRequest : IBaseEntityRequest
  {
    public GetEntitiesUrlBuilder(IRestServiceGrammar restGrammar, ISSCUrlParameters sscGrammar) : 
    base(restGrammar, sscGrammar)
    {
    }

    protected override string GetSpecificPartForRequest(TRequest request)
    {
      return this.GetItemIdentificationForRequest(request);
    }

    protected virtual string GetItemIdentificationForRequest(TRequest request)
    {
      return "";
    }

    protected override void ValidateSpecificRequest(TRequest request)
    { 
    }
  }
}

