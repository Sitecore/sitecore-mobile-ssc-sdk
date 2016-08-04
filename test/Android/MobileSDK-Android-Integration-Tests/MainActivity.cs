namespace MobileSDKAndroidTests
{
  using System.Net;
  using Android.App;
  using Android.OS;

  [Activity(Label = "MobileSDK-Android-Tests", MainLauncher = true)]
  public class MainActivity : ConfigurableTestActivity
  {

    protected override void OnCreate(Bundle bundle)
    {
      // You may use ServicePointManager here
      ServicePointManager
          .ServerCertificateValidationCallback +=
          (sender, cert, chain, sslPolicyErrors) => true;

      base.OnCreate(bundle);

    }
  }
}

