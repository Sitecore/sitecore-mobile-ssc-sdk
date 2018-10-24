namespace Sitecore.MobileSDK.UserRequest.UpdateRequest
{
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Request.Parameters;
  using Sitecore.MobileSDK.Items;
  using Sitecore.MobileSDK.UserRequest.ChangeRequest;
  using Sitecore.MobileSDK.Validators;

  public abstract class AbstractUpdateItemRequestBuilder<T> : AbstractChangeItemRequestBuilder<T>, 
    IUpdateItemRequestParametersBuilder<T> 
    where T : class
  {

    public IUpdateItemRequestParametersBuilder<T> Version(int? itemVersion)
    {
      BaseValidator.AssertPositiveNumber(itemVersion, this.GetType().Name + ".Version");

      BaseValidator.CheckForTwiceSetAndThrow(this.itemSourceAccumulator.VersionNumber, this.GetType().Name + ".Version");

      this.itemSourceAccumulator = new ItemSourcePOD(
        this.itemSourceAccumulator.Database,
        this.itemSourceAccumulator.Language,
        itemVersion);

      return this;
    }

    new public IUpdateItemRequestParametersBuilder<T> AddFieldsRawValuesByNameToSet(IDictionary<string, string> fieldsRawValuesByName)
    {
      return (IUpdateItemRequestParametersBuilder<T>)base.AddFieldsRawValuesByNameToSet(fieldsRawValuesByName);
    }

    new public IUpdateItemRequestParametersBuilder<T> AddFieldsRawValuesByNameToSet(string fieldName, string fieldValue)
    {
      return (IUpdateItemRequestParametersBuilder<T>)base.AddFieldsRawValuesByNameToSet(fieldName, fieldValue);
    }

    new public IUpdateItemRequestParametersBuilder<T> Database(string sitecoreDatabase)
    {
      return (IUpdateItemRequestParametersBuilder<T>)base.Database(sitecoreDatabase);
    }

    new public IUpdateItemRequestParametersBuilder<T> Language(string itemLanguage)
    {
      return (IUpdateItemRequestParametersBuilder<T>)base.Language(itemLanguage);
    }

    new public IUpdateItemRequestParametersBuilder<T> AddFieldsToRead(IEnumerable<string> fields)
    {
      return (IUpdateItemRequestParametersBuilder<T>)base.AddFieldsToRead(fields);
    }

    new public IUpdateItemRequestParametersBuilder<T> AddFieldsToRead(params string[] fieldParams)
    {
      return (IUpdateItemRequestParametersBuilder<T>)base.AddFieldsToRead(fieldParams);
    }
  }
}

