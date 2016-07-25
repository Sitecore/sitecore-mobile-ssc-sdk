namespace Sitecore.MobileSDK.Validators
{
  using Sitecore.MobileSDK.PasswordProvider.Interface;

  public static class SSCCredentialsValidator
  {
    public static bool IsValidCredentials(IScCredentials credentials)
    {
      if (null == credentials)
      {
        return false;
      }
      else if (string.IsNullOrWhiteSpace(credentials.Username))
      {
        // anonymous session
        return true;
      }
      else
      {
        if (string.IsNullOrEmpty(credentials.Password))
        {
          return false;
        }
        else
        {
          return true;
        }
      }
    }

    public static bool IsAnonymousSession(IScCredentials credentials)
    {
      if (!IsValidCredentials(credentials))
      {
        return false;
      }

      return string.IsNullOrWhiteSpace(credentials.Username);
    }
  }
}

