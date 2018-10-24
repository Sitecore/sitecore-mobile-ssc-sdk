namespace Sitecore.MobileSDK.UrlBuilder.Rest
{
  public class RestServiceGrammar : IRestServiceGrammar
  {
    public static RestServiceGrammar ItemSSCV2Grammar()
    {
      RestServiceGrammar result = new RestServiceGrammar();
      result.KeyValuePairSeparator = "=";
      result.FieldSeparator = "&";
      result.HostAndArgsSeparator = "?";
      result.PathComponentSeparator = "/";
      result.ItemFieldSeparator = ",";
      result.SearchFieldSeparator = "|";
      return result;
    }

    private RestServiceGrammar()
    {
    }

    public string KeyValuePairSeparator { get; private set; }
    public string FieldSeparator { get; private set; }
    public string HostAndArgsSeparator { get; private set; }
    public string PathComponentSeparator { get; private set; }
    public string ItemFieldSeparator { get; private set; }
    public string SearchFieldSeparator { get; private set; }
  }
}

