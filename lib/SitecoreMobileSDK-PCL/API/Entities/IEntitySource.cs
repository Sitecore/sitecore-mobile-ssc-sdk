namespace Sitecore.MobileSDK.API.Entities
{
  public interface IEntitySource
  {
    IEntitySource ShallowCopy();

    string Namespase { get; }
    string Controller { get; }
    string Id { get; }
    string Action { get; }
  }
}
