
namespace Sitecore.MobileSDK.UserRequest.ReadRequest.Entities
{
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.Entities;

  public class UpdateEntityRequestBuilder : AbstractChangeEntityRequestBuilder<IUpdateEntityRequest>
  {
    public UpdateEntityRequestBuilder(string entityId)
    {
      this.entityId = entityId;
    }

    public override IUpdateEntityRequest Build()
    {
      IEntitySource entitySource = new EntitySource(
        this.entityNamespace,
        this.entityController,
        this.taskId,
        this.entityAction
      );

      ChangeEntitiesParameters result = new ChangeEntitiesParameters(this.entityId, this.FieldsRawValuesByName, entitySource);

      return result;
    }
  }
}
