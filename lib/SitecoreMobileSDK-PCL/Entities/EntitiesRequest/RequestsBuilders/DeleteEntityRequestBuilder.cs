
namespace Sitecore.MobileSDK.UserRequest.ReadRequest.Entities
{
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.Entities;


  public class DeleteEntityRequestBuilder : ReadEntityByIdRequestBuilder<IDeleteEntityRequest>
  {
    public DeleteEntityRequestBuilder(string entityId)
      : base(entityId)
    {
    }

    public override IDeleteEntityRequest Build()
    {
      IEntitySource entitySource = new EntitySource(
        this.entityNamespace,
        this.entityController,
        this.taskId,
        this.entityAction
      );

      DeleteEntityParameters result = new DeleteEntityParameters(this.entityId, entitySource);

      return result;
    }
  }
}
