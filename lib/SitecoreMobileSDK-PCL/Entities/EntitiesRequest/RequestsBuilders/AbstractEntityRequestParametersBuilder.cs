
namespace Sitecore.MobileSDK.UserRequest.ReadRequest.Entities
{
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.Entities;
  using Sitecore.MobileSDK.Items;

  public abstract class AbstractEntityRequestParametersBuilder<T> : IBaseEntityRequestParametersBuilder<T>
  where T : class
  {
    public IBaseEntityRequestParametersBuilder<T> Namespace(string entityNamespace) {
      this.entityNamespace = entityNamespace;

      return this;
    }

    public IBaseEntityRequestParametersBuilder<T> Controller(string entityController) {
      this.entityController = entityController;

      return this;
    }

    public IBaseEntityRequestParametersBuilder<T> TaskId(string entityId) { 
      this.taskId = entityId;

      return this;
    }

    public IBaseEntityRequestParametersBuilder<T> Action(string entityAction) { 
      this.entityAction = entityAction;

      return this;
    }

    abstract public T Build();

    protected string entityNamespace = null;
    protected string entityController = null;
    protected string taskId = null;
    protected string entityAction = null;
  }
}

