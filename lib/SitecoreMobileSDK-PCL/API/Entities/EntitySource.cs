
namespace Sitecore.MobileSDK.Entities
{
  using Sitecore.MobileSDK.API.Entities;


  public class EntitySource : IEntitySource
  {

    public EntitySource(string namespase, string controller, string id, string action)
    {
      this.Namespase  = namespase;
      this.Controller = controller;
      this.Id         = id;
      this.Action     = action;
    }

    public virtual IEntitySource ShallowCopy()
    {
      return new EntitySource(this.Namespase, this.Controller, this.Id, this.Action);
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

      bool isNamespaseEqual = object.Equals(this.Namespase, other.Namespase);
      bool isControllerEqual = object.Equals(this.Controller, other.Controller);
      bool isIdEqual = object.Equals(this.Id, other.Id);
      bool isActionEqual = object.Equals(this.Action, other.Action);

      return isNamespaseEqual && isControllerEqual && isIdEqual && isActionEqual;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() 
                 + this.Namespase.GetHashCode() 
                 + this.Controller.GetHashCode() 
                 + this.Id.GetHashCode()
                 + this.Action.GetHashCode();
    }

    public string Namespase  { get; protected set; }
    public string Controller { get; protected set; }
    public string Id         { get; protected set; }
    public string Action     { get; protected set; }
  }
}

