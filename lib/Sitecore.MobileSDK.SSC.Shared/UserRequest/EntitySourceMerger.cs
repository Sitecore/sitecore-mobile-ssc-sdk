namespace Sitecore.MobileSDK.UserRequest
{
  using Sitecore.MobileSDK.API.Entities;

  public class EntitySourceMerger
  {
    public EntitySourceMerger(IEntitySource defaultSource)
    {
      if (null == defaultSource)
      {
        return;
      }

      this.defaultSource = defaultSource.ShallowCopy();
    }

    public IEntitySource FillEntitySourceGaps(IEntitySource userSource)
    {
      bool isNullSource = (null == this.defaultSource);
      bool isNullInput = (null == userSource);

      if (isNullSource && isNullInput)
      {
        return null;
      }
      else if (isNullInput)
      {
        return this.defaultSource.ShallowCopy();
      }
      else if (isNullSource)
      {
        return userSource.ShallowCopy();
      }

      string entityNamespace = (null != userSource.EntityNamespace) ? userSource.EntityNamespace : this.defaultSource.EntityNamespace;
      string entityController = (null != userSource.EntityController) ? userSource.EntityController : this.defaultSource.EntityController;
      string entityId = (null != userSource.EntityId) ? userSource.EntityId : this.defaultSource.EntityId;
      string entityAction = (null != userSource.EntityAction) ? userSource.EntityAction : this.defaultSource.EntityAction;

      return new EntitySource(entityNamespace, entityController, entityId, entityAction);
    }

    public IEntitySource DefaultSource
    {
      get
      {
        return this.defaultSource;
      }
    }

    private readonly IEntitySource defaultSource;
  }
}

