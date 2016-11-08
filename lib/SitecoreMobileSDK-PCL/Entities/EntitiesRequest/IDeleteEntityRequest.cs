
namespace Sitecore.MobileSDK.API.Request.Entity
{
  public interface IDeleteEntityRequest : IReadEntityByIdRequest
  {
    IDeleteEntityRequest DeepCopyDeleteEntityRequest();
  }
}

