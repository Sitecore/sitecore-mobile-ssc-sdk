namespace Sitecore.MobileSDK.UrlBuilder.Entity
{
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.UrlBuilder.Rest;
  using Sitecore.MobileSDK.UrlBuilder.SSC;
  using Sitecore.MobileSDK.Utils;

  public class EntityByPathUrlBuilder<T> : GetEntitiesUrlBuilder<T>
  where T : IBaseEntityRequest
  {
    public EntityByPathUrlBuilder(IRestServiceGrammar restGrammar, ISSCUrlParameters sscGrammar)
      : base(restGrammar, sscGrammar)
    {
    }

    protected override string GetHostUrlForRequest(T request)
    {
      string hostUrl = base.GetHostUrlForRequest(request);     

      return hostUrl;
    }

    protected override string GetItemIdentificationForRequest(T request)
    {
      string strItemPath = UrlBuilderUtils.EscapeDataString(request.EntitySource.EntityNamespace)
                                  + restGrammar.PathComponentSeparator
                                  + UrlBuilderUtils.EscapeDataString(request.EntitySource.EntityController)
                                  + restGrammar.PathComponentSeparator;
      if (request.EntitySource.EntityId != null) {
        strItemPath = strItemPath + UrlBuilderUtils.EscapeDataString(request.EntitySource.EntityId) + restGrammar.PathComponentSeparator;
      }

      strItemPath = strItemPath + UrlBuilderUtils.EscapeDataString(request.EntitySource.EntityAction);

      return strItemPath;
    }

    protected override void ValidateSpecificRequest(T request)
    {
       //TODO: @igk implement
    }
  }
}
