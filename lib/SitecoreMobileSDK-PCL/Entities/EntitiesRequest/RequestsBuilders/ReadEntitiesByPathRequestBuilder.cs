
namespace Sitecore.MobileSDK.UserRequest.ReadRequest.Entities
{
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.Entities;

  public class ReadEntitiesByPathRequestBuilder : AbstractEntityRequestParametersBuilder<IReadEntitiesByPathRequest>
  {
    public ReadEntitiesByPathRequestBuilder()
    {
    }

    public override IReadEntitiesByPathRequest Build()
    {
      IEntitySource entitySource = new EntitySource(
        this.entityNamespace,
        this.entityController,
        this.taskId,
        this.entityAction
      );

      ReadEntitiesByPathParameters result = new ReadEntitiesByPathParameters(entitySource, null);

      return result;
    }
  }
}
