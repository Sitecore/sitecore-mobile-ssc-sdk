namespace Sitecore.MobileSDK.PasswordProvider.Interface
{
  using System;

  /// <summary>
  /// A data provider for user's credentials.
  /// A secure implementation must be submitted by the user
  /// </summary>
  public interface IScCredentials : IDisposable
  {

    /// <summary>
    /// Creates a data provider's copy.
    /// This method will be executed once the object is submitted to the session
    /// </summary>
    /// <returns>A copy of the current instance</returns>
    IScCredentials CredentialsShallowCopy();

    /// <summary>
    /// Gets the username from the keychain.
    /// </summary>
    /// <value>
    /// The username to authenticate against the sitecore instance. 
    /// It should contain the domain information if you will not have "site" information.
    /// For example: "sitecore/admin"
    /// </value>
    string Username
    {
      get;
    }

    /// <summary>
    /// Gets the password from the keychain.
    /// </summary>
    /// <value>
    /// The password to authenticate against the sitecore instance.
    /// </value>
    string Password
    {
      get;
    }

    string Domain {
      get;
    }
  }
}

