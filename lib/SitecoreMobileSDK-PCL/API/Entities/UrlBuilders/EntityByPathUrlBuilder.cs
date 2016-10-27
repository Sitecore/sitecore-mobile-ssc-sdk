namespace Sitecore.MobileSDK.UrlBuilder.Entity
{
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.UrlBuilder.Rest;
  using Sitecore.MobileSDK.UrlBuilder.SSC;
  using Sitecore.MobileSDK.Utils;
  using Sitecore.MobileSDK.Validators;

  public class EntityByPathUrlBuilder : GetItemsUrlBuilder<IReadEntitiesByPathRequest>
  {
    public EntityByPathUrlBuilder(IRestServiceGrammar restGrammar, ISSCUrlParameters sscGrammar)
      : base(restGrammar, sscGrammar)
    {
    }

    protected override string GetHostUrlForRequest(IReadEntitiesByPathRequest request)
    {
      string hostUrl = base.GetHostUrlForRequest(request);
      string result = hostUrl;

      return result;
    }

    protected override string GetItemIdenticationForRequest(IReadEntitiesByPathRequest request)
    {
      //string escapedPath = UrlBuilderUtils.EscapeDataString(request.ItemPath.ToLowerInvariant());
      string strItemPath = request.EntitySource.Namespase
                                  + restGrammar.PathComponentSeparator
                                  + request.EntitySource.Controller
                                  + restGrammar.PathComponentSeparator;
      if (request.EntitySource.Id != null) {
        strItemPath = strItemPath + request.EntitySource.Id;
      }

      strItemPath = strItemPath + request.EntitySource.Action;
      string escapedPath = UrlBuilderUtils.EscapeDataString(strItemPath.ToLowerInvariant());

      return escapedPath;
    }

    protected override void ValidateSpecificRequest(IReadEntitiesByPathRequest request)
    {
       //TODO: @igk implement
    }
  }
}
