
namespace Sitecore.MobileSDK.API.Request.Entity
{
  using Sitecore.MobileSDK.API.Entities;

  //FIXME: @igk exclude IBaseItemRequest
  public interface IReadEntitiesByPathRequest : IBaseItemRequest
  {

    IReadEntitiesByPathRequest DeepCopyReadEntitiesByPathRequest();

    IEntitySource EntitySource { get; }
    //ISessionConfig SessionSettings { get; }
  }
}
