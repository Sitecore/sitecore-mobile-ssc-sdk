
namespace Sitecore.MobileSDK.Entities
{
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Request.Entity;

  public class DeleteEntityParameters : ReadEntityByIdParameters, IDeleteEntityRequest
  {
    public DeleteEntityParameters(string id, IEntitySource entitySource, IDictionary<string, string> parametersRawValuesByName)
    : base(id, entitySource, parametersRawValuesByName)
    {

    }

    public DeleteEntityParameters(string id, IEntitySource entitySource, IDictionary<string, string> parametersRawValuesByName, ISessionConfig sessionSettings)
    : base(id, entitySource, parametersRawValuesByName, sessionSettings)
    {

    }

    public IDeleteEntityRequest DeepCopyDeleteEntityRequest()
    {
      IEntitySource entitySource = null;

      if (null != this.EntitySource) {
        entitySource = this.EntitySource.ShallowCopy();
      }

      return new DeleteEntityParameters(this.EntityID, entitySource, this.ParametersRawValuesByName);
    }


    new public virtual IReadEntityByIdRequest DeepCopyReadEntitiesByIdRequest()
    {
      return this.DeepCopyDeleteEntityRequest();
    }

    new public virtual IBaseItemRequest DeepCopyBaseGetItemRequest()
    {
      return this.DeepCopyDeleteEntityRequest();
    }


  }
}
