namespace Sitecore.MobileSDK.API.Entities
{
  public interface IEntitySource
  {
    IEntitySource ShallowCopy();

    string EntityNamespace { get; }
    string EntityController { get; }
    string EntityId { get; }
    string EntityAction { get; }
  }
}
