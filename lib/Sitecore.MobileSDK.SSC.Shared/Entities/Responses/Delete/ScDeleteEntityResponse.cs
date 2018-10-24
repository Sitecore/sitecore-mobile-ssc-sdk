namespace Sitecore.MobileSDK.API.Entities
{
  public class ScDeleteEntityResponse
  {
    public ScDeleteEntityResponse(int statusCode)
    {
      this.StatusCode = statusCode;
    }

    public bool Deleted {
      get {
        return (this.StatusCode == 204);
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

  }
}
