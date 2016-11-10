namespace Sitecore.MobileSDK.API.Session
{

  /// <summary>
  /// Interface represents session builder extension for entities needs. 
  /// </summary>
  public interface IEntitySessionBuilder
  {
    
    IBaseSessionBuilder EntityRouteNamespace(string entityNamespace);
    IBaseSessionBuilder EntityRouteController(string entityController);
    IBaseSessionBuilder EntityRouteId(string entityId);
    IBaseSessionBuilder EntityRouteAction(string entityAction);

  }
}

