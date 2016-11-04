
namespace Sitecore.MobileSDK.API.Entities
{
  public class ScCreateEntityResponse
  {
    public ScCreateEntityResponse(ISitecoreEntity entity, int statusCode)
    {
      this.CreatedEntity = entity;
      this.StatusCode = statusCode;
    }

    public bool Created {
      get {
        return (this.CreatedEntity != null);
      }
    }

    public ISitecoreEntity CreatedEntity {
      get {
        return this.sreatedEntity;
        }
      
      private set {
        this.sreatedEntity = value;
      }
    }

    public int StatusCode {
      get {
        return this.statusCode;
      }

      private set {
        this.statusCode = value;
      }
    }

    private int statusCode = 0;
    private ISitecoreEntity sreatedEntity = null;
  }
}
