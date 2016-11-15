
//FIXME: REFACTOR!!!

namespace Sitecore.MobileSDK.UrlBuilder
{
  using System;
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.SessionSettings;
  using Sitecore.MobileSDK.UrlBuilder.Rest;
  using Sitecore.MobileSDK.UrlBuilder.SSC;
  using Sitecore.MobileSDK.Utils;
  using Sitecore.MobileSDK.Validators;

  public abstract class AbstractGetEntityUrlBuilder<TRequest>
    where TRequest : IBaseEntityRequest
  {
    public AbstractGetEntityUrlBuilder(IRestServiceGrammar restGrammar, ISSCUrlParameters sscGrammar)
    {
      this.restGrammar = restGrammar;
      this.sscGrammar = sscGrammar;

      this.Validate();
    }

    #region Entry Point
    public virtual string GetUrlForRequest(TRequest request)
    {
      this.ValidateRequest(request);

      string requestUrl = this.GetHostUrlForRequest(request);

      string routePath = this.GetEntityRoutePath(request);

      if (!string.IsNullOrEmpty(routePath))
      {
        requestUrl =
          requestUrl +
          restGrammar.PathComponentSeparator +
          routePath;
      }

      string specificPart = this.GetSpecificPartForRequest(request);

      if (!string.IsNullOrEmpty(specificPart)) {
        requestUrl =
          requestUrl + specificPart;
      }

      string customParameters = this.GetParametersString(request);

      if (!string.IsNullOrEmpty(customParameters)) {
        requestUrl =
          requestUrl + restGrammar.HostAndArgsSeparator + customParameters;
      }

      return requestUrl;
    }

    private string GetParametersString(TRequest request)
    {
      if (request.ParametersRawValuesByName == null || request.ParametersRawValuesByName.Count == 0) {
        return "";
      }

      string parametersString = "";
      List<string> keysList = new List<string>(request.ParametersRawValuesByName.Keys);
      foreach (string key in keysList) {
        parametersString = parametersString +
                      key +
                      restGrammar.KeyValuePairSeparator +
                      request.ParametersRawValuesByName[key].ToString() +
                      restGrammar.FieldSeparator;
      }

      parametersString = parametersString.Remove(parametersString.Length - 1);

      return parametersString;
    }

    private string GetEntityRoutePath(TRequest request)
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

    protected virtual void ValidateRequest(TRequest request)
    {
      //FIXME: @igk turn on validation, commented in entities needs
      #warning turn on validation

      //this.ValidateCommonRequest(request);
      //this.ValidateSpecificRequest(request);
    }
    #endregion Entry Point

    #region Common Logic
    protected virtual string GetHostUrlForRequest(TRequest request)
    {
      SessionConfigUrlBuilder sessionBuilder = new SessionConfigUrlBuilder(this.restGrammar, this.sscGrammar);
      string hostUrl = sessionBuilder.BuildUrlString(request.SessionSettings);

      return hostUrl;
    }

    private string GetCommonPartForRequest(TRequest request)
    {
      return "";
    }

    private void ValidateCommonRequest(TRequest request)
    {
      if (null == request)
      {
        throw new ArgumentNullException("AbstractGetItemUrlBuilder.GetBaseUrlForRequest() : request cannot be null");
      }
      else if (null == request.SessionSettings)
      {
        throw new ArgumentNullException("AbstractGetItemUrlBuilder.GetBaseUrlForRequest() : request.SessionSettings cannot be null");
      }
      else if (null == request.SessionSettings.InstanceUrl)
      {
        throw new ArgumentNullException("AbstractGetItemUrlBuilder.GetBaseUrlForRequest() : request.SessionSettings.InstanceUrl cannot be null");
      }

      if (null != request.QueryParameters)
      {
        this.ValidateFields(request.QueryParameters.Fields);
      }
    }

    private void ValidateFields(IEnumerable<string> fields)
    {
      if (DuplicateEntryValidator.IsDuplicatedFieldsInTheList(fields))
      {
        throw new ArgumentException("AbstractGetItemUrlBuilder.GetBaseUrlForRequest() : request.QueryParameters.Fields must contain NO duplicates");
      }
    }

    private void Validate()
    {
      if (null == this.restGrammar)
      {
        throw new ArgumentNullException("[GetItemUrlBuilder] restGrammar cannot be null");
      }
      else if (null == this.sscGrammar)
      {
        throw new ArgumentNullException("[GetItemUrlBuilder] sscGrammar cannot be null");
      }
    }
    #endregion Common Logic

    #region Abstract Methods
    protected abstract string GetSpecificPartForRequest(TRequest request);

    protected abstract void ValidateSpecificRequest(TRequest request);
    #endregion Abstract Methods

    #region instance variables
    protected IRestServiceGrammar restGrammar;
    protected ISSCUrlParameters sscGrammar;
    #endregion instance variables
  }
}

