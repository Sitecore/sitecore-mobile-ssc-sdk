namespace Sitecore.MobileSDK.MockObjects
{
  using Sitecore.MobileSDK.PasswordProvider.Interface;

  public class SSCCredentialsPOD : IScCredentials
  {
    public SSCCredentialsPOD(string username, string password, string domain)
    {
      this.Username = username;
      this.Password = password;
      this.Domain = domain;
    }

    public IScCredentials CredentialsShallowCopy()
    {
      return new SSCCredentialsPOD(this.Username, this.Password, this.Domain);
    }

    public void Dispose()
    {
      this.Username = null;
      this.Password = null;
      this.Domain = null;
    }

    public string Username
    {
      get;
      private set;
    }

    public string Password
    {
      get;
      private set;
    }

    public string Domain {
      get;
      private set;
    }
  }
}

