
namespace Sitecore.MobileSDK.UserRequest.ReadRequest.Entities
{
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.Entities;

  public class ReadEntityByIdRequestBuilder<T> : AbstractEntityRequestParametersBuilder<T>
  where T : class, IReadEntityByIdRequest
  {
    public ReadEntityByIdRequestBuilder(string entityId)
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

      ReadEntityByIdParameters result = new ReadEntityByIdParameters(this.entityId, entitySource, this.ParametersRawValuesByName);

      return result as T;
    }

    protected string entityId;
  }
}
