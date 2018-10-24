
namespace Sitecore.MobileSDK.UserRequest.ReadRequest.Entities
{
  using System;
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.Validators;

  public abstract class AbstractChangeEntityRequestBuilder<T> : AbstractEntityRequestParametersBuilder<T>, IChangeEntityParametersBuilder<T>
  where T : class
  {

    new public IChangeEntityParametersBuilder<T> Namespace(string entityNamespace)
    {
      return base.Namespace(entityNamespace) as IChangeEntityParametersBuilder<T>;
    }

    new public IChangeEntityParametersBuilder<T> Controller(string entityController)
    {
      return base.Controller(entityController) as IChangeEntityParametersBuilder<T>;
    }

    new public IChangeEntityParametersBuilder<T> TaskId(string entityId)
    {
      return base.TaskId(entityId) as IChangeEntityParametersBuilder<T>;
    }

    new public IChangeEntityParametersBuilder<T> Action(string entityAction)
    {
      return base.Action(entityAction) as IChangeEntityParametersBuilder<T>;
    }

    new public IChangeEntityParametersBuilder<T> AddParametersRawValues(IDictionary<string, string> parametersValuesByName)
    {
      return base.AddParametersRawValues(parametersValuesByName) as IChangeEntityParametersBuilder<T>;
    }

    new public IChangeEntityParametersBuilder<T> AddParametersRawValues(string parameterName, string parameterValue)
    {
      return base.AddParametersRawValues(parameterName, parameterValue) as IChangeEntityParametersBuilder<T>;
    }

    public IChangeEntityParametersBuilder<T> AddFieldsRawValuesByNameToSet(IDictionary<string, string> fieldsRawValuesByName)
    {
      BaseValidator.CheckNullAndThrow(fieldsRawValuesByName, this.GetType().Name + ".FieldsRawValuesByName");

      if (fieldsRawValuesByName.Count == 0) {
        return this;
      }

      foreach (var fieldElem in fieldsRawValuesByName) {
        this.AddFieldsRawValuesByNameToSet(fieldElem.Key, fieldElem.Value);
      }

      return this;
    }

    public IChangeEntityParametersBuilder<T> AddFieldsRawValuesByNameToSet(string fieldName, string fieldValue)
    {
      BaseValidator.CheckForNullAndEmptyOrThrow(fieldName, this.GetType().Name + ".fieldName");

      if (null == this.FieldsRawValuesByName) {
        Dictionary<string, string> newFields = new Dictionary<string, string>(StringComparer.Ordinal);
        this.FieldsRawValuesByName = newFields;
      }

      bool keyIsDuplicated = DuplicateEntryValidator.IsDuplicatedFieldsInTheDictionary(this.FieldsRawValuesByName, fieldName);
      if (keyIsDuplicated) {
        throw new InvalidOperationException(this.GetType().Name + ".FieldsRawValuesByName : duplicate fields are not allowed");
      }

      this.FieldsRawValuesByName.Add(fieldName, fieldValue);

      return this;
    }

    protected IDictionary<string, string> FieldsRawValuesByName;

    protected string entityId = null;
  }
}
