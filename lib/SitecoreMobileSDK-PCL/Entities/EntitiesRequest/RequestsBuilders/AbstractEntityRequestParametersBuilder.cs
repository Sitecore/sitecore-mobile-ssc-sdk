
namespace Sitecore.MobileSDK.UserRequest.ReadRequest.Entities
{
  using System;
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.Validators;

  public abstract class AbstractEntityRequestParametersBuilder<T> : IBaseEntityRequestParametersBuilder<T>
  where T : class
  {
    public IBaseEntityRequestParametersBuilder<T> Namespace(string entityNamespace) {
      this.entityNamespace = entityNamespace;

      return this;
    }

    public IBaseEntityRequestParametersBuilder<T> Controller(string entityController) {
      this.entityController = entityController;

      return this;
    }

    public IBaseEntityRequestParametersBuilder<T> TaskId(string entityId) { 
      this.taskId = entityId;

      return this;
    }

    public IBaseEntityRequestParametersBuilder<T> Action(string entityAction) { 
      this.entityAction = entityAction;

      return this;
    }

    public IBaseEntityRequestParametersBuilder<T> AddParametersRawValues(IDictionary<string, string> parametersValuesByName)
    {
      BaseValidator.CheckNullAndThrow(parametersValuesByName, this.GetType().Name + ".AddParametersRawValues");

      if (parametersValuesByName.Count == 0) {
        return this;
      }

      foreach (var paramElem in parametersValuesByName) {
        this.AddParametersRawValues(paramElem.Key, paramElem.Value);
      }

      return this;
    }

    public IBaseEntityRequestParametersBuilder<T> AddParametersRawValues(string parameterName, string parameterValue)
    {
      BaseValidator.CheckForNullAndEmptyOrThrow(parameterName, this.GetType().Name + ".parameterName");
      BaseValidator.CheckForNullAndEmptyOrThrow(parameterValue, this.GetType().Name + ".parameterValue");

      if (null == this.ParametersRawValuesByName) {
        Dictionary<string, string> newParameters = new Dictionary<string, string>(StringComparer.Ordinal);
        this.ParametersRawValuesByName = newParameters;
      }

      bool keyIsDuplicated = DuplicateEntryValidator.IsDuplicatedFieldsInTheDictionary(this.ParametersRawValuesByName, parameterName);
      if (keyIsDuplicated) {
        throw new InvalidOperationException(this.GetType().Name + ".AddParametersRawValues : duplicate fields are not allowed");
      }

      this.ParametersRawValuesByName.Add(parameterName, parameterValue);

      return this;
    }


    abstract public T Build();

    protected string entityNamespace = null;
    protected string entityController = null;
    protected string taskId = null;
    protected string entityAction = null;

    protected IDictionary<string, string> ParametersRawValuesByName;
  }
}

