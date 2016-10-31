
namespace Sitecore.MobileSDK.API.Request.Entity
{
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request;

  //FIXME: @igk exclude IBaseItemRequest
  public interface IBaseEntityRequest : IBaseItemRequest
  {
    IEntitySource EntitySource { get; }
  }
}
