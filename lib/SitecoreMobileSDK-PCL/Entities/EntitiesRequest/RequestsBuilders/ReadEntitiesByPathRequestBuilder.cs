
namespace Sitecore.MobileSDK.UserRequest.ReadRequest.Entities
{
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.Entities;
  using Sitecore.MobileSDK.Items;

  public class ReadEntitiesByPathRequestBuilder : IBaseEntityRequestParametersBuilder<IReadEntitiesByPathRequest>
  {
    public ReadEntitiesByPathRequestBuilder()
    {
    }

    public IBaseEntityRequestParametersBuilder<IReadEntitiesByPathRequest> Namespace(string entityNamespace) {
      this.entityNamespace = entityNamespace;

      return this;
    }

    public IBaseEntityRequestParametersBuilder<IReadEntitiesByPathRequest> Controller(string entityController) {
      this.entityController = entityController;

      return this;
    }

    public IBaseEntityRequestParametersBuilder<IReadEntitiesByPathRequest> Id(string entityId) { 
      this.entityId = entityId;

      return this;
    }

    public IBaseEntityRequestParametersBuilder<IReadEntitiesByPathRequest> Action(string entityAction) { 
      this.entityAction = entityAction;

      return this;
    }

    public IReadEntitiesByPathRequest Build()
    {
      IEntitySource entitySource = new EntitySource(
        this.entityNamespace,
        this.entityController,
        this.entityId,
        this.entityAction
      );

      ReadEntitiesByPathParameters result = new ReadEntitiesByPathParameters(entitySource, null);

      return result;
    }

    private string entityNamespace = null;
    private string entityController = null;
    private string entityId = null;
    private string entityAction = null;
  }
}

