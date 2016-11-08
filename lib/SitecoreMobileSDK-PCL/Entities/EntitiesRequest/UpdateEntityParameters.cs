
namespace Sitecore.MobileSDK.Entities
{
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Request.Entity;

  public class UpdateEntityParameters : ChangeEntitiesParameters, IUpdateEntityRequest
  {
    public UpdateEntityParameters(string id, IDictionary<string, string> fieldsRawValuesByName, IEntitySource entitySource)
    : base(id, fieldsRawValuesByName, entitySource)
    { 
    
    }

    public UpdateEntityParameters(string id, IDictionary<string, string> fieldsRawValuesByName, IEntitySource entitySource, ISessionConfig sessionSettings)
    : base(id, fieldsRawValuesByName, entitySource, sessionSettings)
    {

    }

    public IUpdateEntityRequest DeepCopyUpdateEntityRequest()
    {
      IEntitySource entitySource = null;

      if (null != this.EntitySource) {
        entitySource = this.EntitySource.ShallowCopy();
      }

      return new UpdateEntityParameters(this.EntityID, this.FieldsRawValuesByName, entitySource);
    }

    new public virtual ICreateEntityRequest DeepCopyCreateEntityRequest()
    {
      return this.DeepCopyUpdateEntityRequest();
    }

    public virtual  IReadEntityByIdRequest DeepCopyReadEntitiesByIdRequest()
    {
      return this.DeepCopyUpdateEntityRequest();
    }

    new public virtual IBaseItemRequest DeepCopyBaseGetItemRequest()
    {
      return this.DeepCopyUpdateEntityRequest();
    }


  }
}
