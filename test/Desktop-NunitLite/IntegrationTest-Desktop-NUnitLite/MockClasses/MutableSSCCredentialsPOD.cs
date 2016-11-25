namespace Sitecore.MobileSDK.MockObjects
{
  using Sitecore.MobileSDK.PasswordProvider.Interface;

  public class MutableSSCCredentialsPOD : IScCredentials
  {
    public MutableSSCCredentialsPOD(string username, string password, string domain)
    {
      this.Username = username;
      this.Password = password;
      this.Domain = domain;
    }

    public IScCredentials CredentialsShallowCopy()
    {
      return new MutableSSCCredentialsPOD(this.Username, this.Password, this.Domain);
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
      set;
    }

    public string Password
    {
      get;
      set;
    }  

    public string Domain {
      get;
      set;
    }
  }
}

