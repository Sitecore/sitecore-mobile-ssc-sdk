using System;
using Sitecore.MobileSDK.API.Entities;
using Sitecore.MobileSDK.API.Request.Entity;
using Sitecore.MobileSDK.Entities;

namespace Sitecore.MobileSDK.UserRequest.ReadRequest.Entities
{
  public class CreateEntityRequestBuilder : AbstractChangeEntityRequestBuilder<ICreateEntityRequest>
  {
    public CreateEntityRequestBuilder(int entityId)
    {
      this.entityId = entityId.ToString();
    }

    public override ICreateEntityRequest Build()
    {
      IEntitySource entitySource = new EntitySource(
        this.entityNamespace,
        this.entityController,
        this.taskId,
        this.entityAction
      );

      CreateEntityParameters result = new CreateEntityParameters(this.entityId, this.FieldsRawValuesByName, entitySource);

      return result;
    }
  }
}
