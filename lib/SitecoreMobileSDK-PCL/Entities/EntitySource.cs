
namespace Sitecore.MobileSDK.API.Entities
{

  public class EntitySource : IEntitySource
  {

    public EntitySource(string entityNamespace, string controller, string id, string action)
    {
      this.EntityNamespace  = entityNamespace;
      this.EntityController = controller;
      this.EntityId         = id;
      this.EntityAction     = action;
    }

    public virtual IEntitySource ShallowCopy()
    {
      return new EntitySource(this.EntityNamespace, this.EntityController, this.EntityId, this.EntityAction);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals(this, obj))
      {
        return true;
      }

      EntitySource other = (EntitySource)obj;
      if (null == other)
      {
        return false;
      }

      bool isNamespaceEqual = object.Equals(this.EntityNamespace, other.EntityNamespace);
      bool isControllerEqual = object.Equals(this.EntityController, other.EntityController);
      bool isIdEqual = object.Equals(this.EntityId, other.EntityId);
      bool isActionEqual = object.Equals(this.EntityAction, other.EntityAction);

      return isNamespaceEqual && isControllerEqual && isIdEqual && isActionEqual;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() 
                 + this.EntityNamespace.GetHashCode() 
                 + this.EntityController.GetHashCode() 
                 + this.EntityId.GetHashCode()
                 + this.EntityAction.GetHashCode();
    }

    public string EntityNamespace  { get; protected set; }
    public string EntityController { get; protected set; }
    public string EntityId         { get; protected set; }
    public string EntityAction     { get; protected set; }
  }
}

