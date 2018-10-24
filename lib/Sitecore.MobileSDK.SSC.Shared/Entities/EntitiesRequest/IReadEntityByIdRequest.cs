
namespace Sitecore.MobileSDK.API.Request.Entity
{
  public interface IReadEntityByIdRequest : IBaseEntityRequest
  {
    
    IReadEntityByIdRequest DeepCopyReadEntitiesByIdRequest();
    string EntityID { get; }

  }
}
