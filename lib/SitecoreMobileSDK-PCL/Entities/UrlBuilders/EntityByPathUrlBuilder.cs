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

    protected override string GetItemIdenticationForRequest(T request)
    {
      string strItemPath = UrlBuilderUtils.EscapeDataString(request.EntitySource.Namespase)
                                  + restGrammar.PathComponentSeparator
                                  + UrlBuilderUtils.EscapeDataString(request.EntitySource.Controller)
                                  + restGrammar.PathComponentSeparator;
      if (request.EntitySource.Id != null) {
        strItemPath = strItemPath + UrlBuilderUtils.EscapeDataString(request.EntitySource.Id);
      }

      strItemPath = strItemPath + UrlBuilderUtils.EscapeDataString(request.EntitySource.Action);

      return strItemPath;
    }

    protected override void ValidateSpecificRequest(T request)
    {
       //TODO: @igk implement
    }
  }
}
