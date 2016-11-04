namespace Sitecore.MobileSDK.API.Entities
{
  public class ScUpdateEntityResponse
  {
    public ScUpdateEntityResponse(int statusCode)
    {
      this.StatusCode = statusCode;
    }

    public bool Updated {
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
