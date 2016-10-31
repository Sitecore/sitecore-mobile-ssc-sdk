
namespace Sitecore.MobileSDK.API.Request.Entity
{
  using System;
  using System.Collections.Generic;

  public interface ICreateEntityRequest : IBaseEntityRequest
  {
    ICreateEntityRequest DeepCopyCreateEntityRequest();
    string EntityId { get; }
    IDictionary<string, string> FieldsRawValuesByName { get; }
  }
}
