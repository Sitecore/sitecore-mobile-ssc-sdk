
namespace Sitecore.MobileSDK.API.Request.Entity
{
  using System.Collections.Generic;

  public interface IBaseEntityRequestParametersBuilder<T> where T : class
  {

    IBaseEntityRequestParametersBuilder<T> Namespace(string entityNamespace);
    IBaseEntityRequestParametersBuilder<T> Controller(string entityController);
    IBaseEntityRequestParametersBuilder<T> TaskId(string entityId);
    IBaseEntityRequestParametersBuilder<T> Action(string entityAction);

    IBaseEntityRequestParametersBuilder<T> AddParametersRawValues(IDictionary<string, string> parametersValuesByName);
    IBaseEntityRequestParametersBuilder<T> AddParametersRawValues(string parameterName, string parameterValue);

    T Build();
  }
}
