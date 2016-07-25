namespace Sitecore.MobileSdkUnitTest
{
  using System;
  using NUnit.Framework;
  using Sitecore.MobileSDK.PasswordProvider;

  [TestFixture]
  public class PortablePasswordStorageTest
  {
    [Test]
    public void TestPasswordStorageReturnsSameValues()
    {
      const string login = "admin";
      const string password = "bimba";
      const string domain = "sitecore";

      using (var passwordStorage = new ScUnsecuredCredentialsProvider(login, password, domain))
      {
        Assert.AreEqual(login, passwordStorage.Username);
        Assert.AreEqual(password, passwordStorage.Password);
      }
    }


    [Test]
    public void TestPasswordStorageCopyReturnsSameValues()
    {
      const string login = "duncan";
      const string password = "There can be only one";
      const string domain = "sitecore";

      using (var passwordStorage = new ScUnsecuredCredentialsProvider(login, password, domain))
      {
        Assert.AreEqual(login, passwordStorage.Username);
        Assert.AreEqual(password, passwordStorage.Password);

        using (var passwordStorageCopy = passwordStorage.CredentialsShallowCopy())
        {
          Assert.AreNotSame(passwordStorage, passwordStorageCopy);

          Assert.AreEqual(login, passwordStorageCopy.Username);
          Assert.AreEqual(password, passwordStorageCopy.Password);
        }
      }
    }

    [Test]
    public void TestPasswordStorageRejectsNullLogin()
    {
      const string login = null;
      const string password = "bimba";
      const string domain = "sitecore";

      Assert.Throws<ArgumentException>(
        () => new ScUnsecuredCredentialsProvider(login, password, domain),
        "Exception for empty login expected");
    }

    [Test]
    public void TestPasswordStorageRejectsEmptyLogin()
    {
      const string login = "";
      const string password = "bimba";
      const string domain = "sitecore";

      Assert.Throws<ArgumentException>(
        () => new ScUnsecuredCredentialsProvider(login, password, domain),
        "Exception for empty login expected");
    }

    [Test]
    public void TestPasswordStorageAllowsWhitespaceLogin()
    {
      const string login = "     ";
      const string password = "bimba";
      const string domain = "sitecore";

      using (var passwordStorage = new ScUnsecuredCredentialsProvider(login, password, domain))
      {
        Assert.AreEqual(login, passwordStorage.Username);
        Assert.AreEqual(password, passwordStorage.Password);
      }
    }


    [Test]
    public void TestPasswordStorageAllowsEmptyPassword()
    {
      const string login = "ashot";
      const string password = "";
      const string domain = "sitecore";

      using (var passwordStorage = new ScUnsecuredCredentialsProvider(login, password, domain))
      {
        Assert.AreEqual(login, passwordStorage.Username);
        Assert.AreEqual(password, passwordStorage.Password);
      }
    }


    [Test]
    public void TestPasswordStorageAllowsNullPassword()
    {
      const string login = "arnold";
      const string password = null;
      const string domain = "sitecore";

      using (var passwordStorage = new ScUnsecuredCredentialsProvider(login, password, domain))
      {
        Assert.AreEqual(login, passwordStorage.Username);
        Assert.AreEqual(password, passwordStorage.Password);
      }
    }



    [Test]
    public void TestPasswordStorageAllowsWhitespacePassword()
    {
      const string login = "whitespace";
      const string password = "   ";
      const string domain = "sitecore";

      using (var passwordStorage = new ScUnsecuredCredentialsProvider(login, password, domain))
      {
        Assert.AreEqual(login, passwordStorage.Username);
        Assert.AreEqual(password, passwordStorage.Password);
      }
    }
  }
}

