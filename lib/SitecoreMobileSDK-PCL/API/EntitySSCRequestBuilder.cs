
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

    public static IBaseEntityRequestParametersBuilder<IReadEntityByIdRequest> ReadEntityRequestById(string entityId)
    {
      return new ReadEntityByIdRequestBuilder<IReadEntityByIdRequest>(entityId);
    }

    public static IChangeEntityParametersBuilder<ICreateEntityRequest> CreateEntityRequest(string entityId)
    {
      return new CreateEntityRequestBuilder<ICreateEntityRequest>(entityId);
    }

    public static IChangeEntityParametersBuilder<IUpdateEntityRequest> UpdateEntityRequest(string entityId)
    {
      return new CreateEntityRequestBuilder<IUpdateEntityRequest>(entityId);
    }

    public static IBaseEntityRequestParametersBuilder<IDeleteEntityRequest> DeleteEntityRequest(string entityId)
    {
      return new DeleteEntityRequestBuilder(entityId);
    }
  }
}

