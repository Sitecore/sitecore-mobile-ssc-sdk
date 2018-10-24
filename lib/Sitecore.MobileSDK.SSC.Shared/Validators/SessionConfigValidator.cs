namespace Sitecore.MobileSDK.Validators
{
  public class SessionConfigValidator
  {
    private SessionConfigValidator()
    {
    }

    public static string AutocompleteInstanceUrl(string url)
    {
      if (IsValidSchemeOfInstanceUrl(url))
      {
        return url;
      }

      char[] slashes = { '/' };

      string result = "http://" + url;
      result = result.TrimEnd(slashes);

      return result;
    }

    public static string AutocompleteInstanceUrlWithHttps(string url)
    {      
      if (IsValidSchemeOfInstanceUrl(url)) {
        string lowercaseUrl = url.ToLowerInvariant();
        if (!lowercaseUrl.StartsWith("https://", System.StringComparison.CurrentCulture)) {
          lowercaseUrl = lowercaseUrl.Insert(4, "s");
        }
        return lowercaseUrl;
      }

      char[] slashes = { '/' };

      string result = "https://" + url;
      result = result.TrimEnd(slashes);

      return result;
    }

    public static bool IsValidSchemeOfInstanceUrl(string url)
    {
      string lowercaseUrl = url.ToLowerInvariant();

      bool isHttps = lowercaseUrl.StartsWith("https://", System.StringComparison.CurrentCulture);
      bool isHttp = lowercaseUrl.StartsWith("http://", System.StringComparison.CurrentCulture);
      bool result = (isHttps || isHttp);

      return result;
    }

  }
}

