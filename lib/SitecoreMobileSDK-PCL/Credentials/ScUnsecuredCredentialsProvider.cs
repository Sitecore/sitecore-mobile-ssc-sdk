using System;
using Sitecore.MobileSDK.PasswordProvider.Interface;

namespace Sitecore.MobileSDK.PasswordProvider
{
  public class ScUnsecuredCredentialsProvider: IScCredentials
  {
    private string unencryptedLogin;
    private string unencryptedPassword;
    private string unencryptedDomain;

    private ScUnsecuredCredentialsProvider()
    {
    }

    public ScUnsecuredCredentialsProvider(string login, string password, string domain)
    {
      if (string.IsNullOrEmpty(login)) {
        throw new ArgumentException("[PasswordProvider] : username cannot be null or empty");
      }
      this.unencryptedLogin = login;

      if (null != password) {
        this.unencryptedPassword = password;
      }

      if (null != domain) {
        this.unencryptedDomain = domain;
      }
    }

    public void Dispose()
    {
      this.unencryptedLogin = null;
      this.unencryptedPassword = null;
      this.unencryptedDomain = null;
    }

    #region IScCredentials
    public ScUnsecuredCredentialsProvider PasswordProviderCopy()
    {
      ScUnsecuredCredentialsProvider result = 
        new ScUnsecuredCredentialsProvider(this.unencryptedLogin, this.unencryptedPassword, this.unencryptedDomain);
    
      return result;
    }

    public IScCredentials CredentialsShallowCopy()
    {
      return this.PasswordProviderCopy();
    }

    public string Username {
      get {
        return this.unencryptedLogin;
      }
    }

    public string Password {
      get {
        return this.unencryptedPassword;
      }
    }

    public string Domain {
      get {
        return this.unencryptedDomain;
      }
    }
    #endregion IScCredentials
  }
}

