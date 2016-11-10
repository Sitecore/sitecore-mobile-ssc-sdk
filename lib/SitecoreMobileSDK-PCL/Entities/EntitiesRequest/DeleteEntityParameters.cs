
namespace Sitecore.MobileSDK.Entities
{
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Request.Entity;

  public class DeleteEntityParameters : ReadEntityByIdParameters, IDeleteEntityRequest
  {
    public DeleteEntityParameters(string id, IEntitySource entitySource)
    : base(id, entitySource)
    {

    }

    public DeleteEntityParameters(string id, IEntitySource entitySource, ISessionConfig sessionSettings)
    : base(id, entitySource, sessionSettings)
    {

    }

    public IDeleteEntityRequest DeepCopyDeleteEntityRequest()
    {
      IEntitySource entitySource = null;

      if (null != this.EntitySource) {
        entitySource = this.EntitySource.ShallowCopy();
      }

      return new DeleteEntityParameters(this.EntityID, entitySource);
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
