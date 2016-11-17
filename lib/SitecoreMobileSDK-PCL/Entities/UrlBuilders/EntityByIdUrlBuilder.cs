
namespace Sitecore.MobileSDK.UrlBuilder.Entity
{
  using System;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.UrlBuilder.Rest;
  using Sitecore.MobileSDK.UrlBuilder.SSC;

  public class EntityByIdUrlBuilder<T> : GetEntitiesUrlBuilder<T>
  where T : IReadEntityByIdRequest
  {
    public EntityByIdUrlBuilder(IRestServiceGrammar restGrammar, ISSCUrlParameters sscGrammar)
      : base(restGrammar, sscGrammar)
    {
    }

    protected override string GetItemIdentificationForRequest(T request)
    {
      string result = "('" + request.EntityID + "')";

      return result;
    }

    protected override void ValidateSpecificRequest(T request)
    {
      base.ValidateSpecificRequest(request);

      if (null == request.EntityID) {
        throw new ArgumentNullException("EntityByIdUrlBuilder.GetBaseUrlForRequest() : EntityID cannot be null");
      }
    }
  }
}
