
namespace Sitecore.MobileSDK.API.Request.Entity
{
  using System;
  using System.Collections.Generic;

  public interface ICreateEntityRequest : IReadEntityByIdRequest
  {
    ICreateEntityRequest DeepCopyCreateEntityRequest();
    IDictionary<string, string> FieldsRawValuesByName { get; }
  }
}
