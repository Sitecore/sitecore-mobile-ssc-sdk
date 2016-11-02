using System;
namespace Sitecore.MobileSDK.API.Entities
{
  public class ScCreateEntityResponse
  {
    public ScCreateEntityResponse(ISitecoreEntity entity)
    {
      this.CreatedEntity = entity;
    }

    public bool Created {
      get {
        return (this.CreatedEntity != null);
      }
    }

    public ISitecoreEntity createdEntity {
      get {
        return this.CreatedEntity;
        }
      
      private set {
        this.CreatedEntity = value;
      }
    }

    private ISitecoreEntity CreatedEntity = null;
  }
}
