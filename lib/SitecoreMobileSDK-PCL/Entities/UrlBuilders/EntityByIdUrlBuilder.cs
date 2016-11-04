
namespace Sitecore.MobileSDK.UrlBuilder.Entity
{
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.UrlBuilder.Rest;
  using Sitecore.MobileSDK.UrlBuilder.SSC;

  public class EntityByIdUrlBuilder<T> : EntityByPathUrlBuilder<T>
  where T : IReadEntityByIdRequest
  {
    public EntityByIdUrlBuilder(IRestServiceGrammar restGrammar, ISSCUrlParameters sscGrammar)
      : base(restGrammar, sscGrammar)
    {
    }

    protected override string GetItemIdenticationForRequest(T request)
    {
      string basePath = base.GetItemIdenticationForRequest(request);

      string result = basePath + "('" + request.EntityID + "')";

      return result;
    }
  }
}
