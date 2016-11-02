namespace Sitecore.MobileSDK.UrlBuilder.Entity
{
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.UrlBuilder.Rest;
  using Sitecore.MobileSDK.UrlBuilder.SSC;
  using Sitecore.MobileSDK.Utils;
  using Sitecore.MobileSDK.Validators;

  public class EntityByPathUrlBuilder : GetEntitiesUrlBuilder<IBaseEntityRequest>
  {
    public EntityByPathUrlBuilder(IRestServiceGrammar restGrammar, ISSCUrlParameters sscGrammar)
      : base(restGrammar, sscGrammar)
    {
    }

    protected override string GetHostUrlForRequest(IBaseEntityRequest request)
    {
      string hostUrl = base.GetHostUrlForRequest(request);     

      return hostUrl;
    }

    protected override string GetItemIdenticationForRequest(IBaseEntityRequest request)
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

    protected override void ValidateSpecificRequest(IBaseEntityRequest request)
    {
       //TODO: @igk implement
    }
  }
}
