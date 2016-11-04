
namespace Sitecore.MobileSDK.API.Request.Entity
{
  public interface IUpdateEntityRequest : ICreateEntityRequest
  {
    IUpdateEntityRequest DeepCopyUpdateEntityRequest();
  }
}
