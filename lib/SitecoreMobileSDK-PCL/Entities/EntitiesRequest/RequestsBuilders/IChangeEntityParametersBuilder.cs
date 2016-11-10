
namespace Sitecore.MobileSDK.API.Request.Entity
{
  using System.Collections.Generic;

  public interface IChangeEntityParametersBuilder<T> : IBaseEntityRequestParametersBuilder<T>
  where T : class
  {
    new IChangeEntityParametersBuilder<T> Namespace(string entityNamespace);
    new IChangeEntityParametersBuilder<T> Controller(string entityController);
    new IChangeEntityParametersBuilder<T> TaskId(string entityId);
    new IChangeEntityParametersBuilder<T> Action(string entityAction);

    new IChangeEntityParametersBuilder<T> AddParametersRawValues(IDictionary<string, string> parametersValuesByName);
    new IChangeEntityParametersBuilder<T> AddParametersRawValues(string parameterName, string parameterValue);

    IChangeEntityParametersBuilder<T> AddFieldsRawValuesByNameToSet(IDictionary<string, string> fieldsRawValuesByName);
    IChangeEntityParametersBuilder<T> AddFieldsRawValuesByNameToSet(string fieldName, string fieldValue);
  }
}
