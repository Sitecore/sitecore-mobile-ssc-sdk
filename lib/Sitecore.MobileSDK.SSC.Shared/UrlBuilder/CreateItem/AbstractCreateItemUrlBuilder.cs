namespace Sitecore.MobileSDK.UrlBuilder.CreateItem
{
  using Sitecore.MobileSDK.Utils;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.UrlBuilder.ChangeItem;
  using Sitecore.MobileSDK.UrlBuilder.Rest;
  using Sitecore.MobileSDK.UrlBuilder.SSC;
  using Sitecore.MobileSDK.SessionSettings;
  using Sitecore.MobileSDK.Validators;

  public abstract class AbstractCreateItemUrlBuilder<TRequest> : ICreateItemUrlBuilder<TRequest>
    where TRequest : ICreateItemByPathRequest
  {
    private readonly SessionConfigUrlBuilder sessionConfigUrlBuilder;
    protected IRestServiceGrammar RestGrammar;
    protected ISSCUrlParameters SSCGrammar;


    public AbstractCreateItemUrlBuilder(IRestServiceGrammar restGrammar, ISSCUrlParameters sscGrammar)
    {
      this.RestGrammar = restGrammar;
      this.SSCGrammar = sscGrammar;
      this.sessionConfigUrlBuilder = new SessionConfigUrlBuilder(restGrammar, sscGrammar);
    }

    public abstract string GetUrlForRequest(TRequest request);

    protected void Validate(TRequest request)
    {
      BaseValidator.CheckNullAndThrow(request, this.GetType().Name + ".request");

      this.ValidateSpecificPart(request);
    }

    public virtual string GetBaseUrlForRequest(TRequest request)
    {
      string hostUrl = this.sessionConfigUrlBuilder.BuildUrlString(request.SessionSettings);
      hostUrl = hostUrl + this.SSCGrammar.ItemSSCItemsEndpoint;

      return hostUrl;
    }

    protected string GetParametersString(TRequest request)
    {
      var parametersString = "";

      var database = request.ItemSource.Database;

      if (!string.IsNullOrEmpty(database)) {
        if (!string.IsNullOrEmpty(parametersString)) {
          parametersString += this.RestGrammar.FieldSeparator;
        }
        parametersString += this.SSCGrammar.DatabaseParameterName + this.RestGrammar.KeyValuePairSeparator
          + database;
      }
      return parametersString;
    }

    public abstract void ValidateSpecificPart(TRequest request);


  }
}

