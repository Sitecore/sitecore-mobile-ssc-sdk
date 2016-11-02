
namespace Sitecore.MobileSDK.API
{
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.UserRequest.ReadRequest.Entities;

  public class EntitySSCRequestBuilder
  {
    private EntitySSCRequestBuilder()
    {
    }

    public static IBaseEntityRequestParametersBuilder<IReadEntitiesByPathRequest> ReadEntitiesRequestWithPath()
    {
      return new ReadEntitiesByPathRequestBuilder();
    }

    public static IChangeEntityParametersBuilder<ICreateEntityRequest> CreateEntityRequest(int entityId)
    {
      return new CreateEntityRequestBuilder(entityId);
    }
  }
}

