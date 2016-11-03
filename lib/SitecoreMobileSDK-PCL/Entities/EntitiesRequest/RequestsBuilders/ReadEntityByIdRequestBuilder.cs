using System;
using Sitecore.MobileSDK.API.Entities;
using Sitecore.MobileSDK.API.Request.Entity;
using Sitecore.MobileSDK.Entities;

namespace Sitecore.MobileSDK.UserRequest.ReadRequest.Entities
{
  public class ReadEntityByIdRequestBuilder : AbstractEntityRequestParametersBuilder<IReadEntityByIdRequest>
  {
    public ReadEntityByIdRequestBuilder(string entityId)
    {
      this.entityId = entityId;
    }

    public override IReadEntityByIdRequest Build()
    {
      IEntitySource entitySource = new EntitySource(
        this.entityNamespace,
        this.entityController,
        this.taskId,
        this.entityAction
      );

      ReadEntityByIdParameters result = new ReadEntityByIdParameters(this.entityId, entitySource);

      return result;
    }

    private string entityId;
  }
}
