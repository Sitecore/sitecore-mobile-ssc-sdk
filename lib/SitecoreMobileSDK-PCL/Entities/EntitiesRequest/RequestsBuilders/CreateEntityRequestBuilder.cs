
namespace Sitecore.MobileSDK.UserRequest.ReadRequest.Entities
{
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.Entities;

  public class CreateEntityRequestBuilder<T> : AbstractChangeEntityRequestBuilder<T>
  where T : class, ICreateEntityRequest
  {
    public CreateEntityRequestBuilder(string entityId)
    {
      this.entityId = entityId;
    }

    public override T Build()
    {
      IEntitySource entitySource = new EntitySource(
        this.entityNamespace,
        this.entityController,
        this.taskId,
        this.entityAction
      );

      ChangeEntitiesParameters result = 
        new ChangeEntitiesParameters(this.entityId, this.FieldsRawValuesByName, this.ParametersRawValuesByName, entitySource);

      return result as T;
    }
  }
}
