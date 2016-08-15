using System.Reflection;

using Android.App;
using Android.OS;
using Xamarin.Android.NUnitLite;
using Android.Views;
using MobileSDKAndroidTests;
using System.Net;

namespace MobileSDKAndroidIntegrationNugetUpdated
{
  [Activity(Label = "MobileSDK-Android-Integration-Nuget-Updated", MainLauncher = true)]
  public class MainActivity : ConfigurableTestActivity
  {
    protected override void OnCreate(Bundle bundle)
    {
      //this.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
      // tests can be inside the main assembly
      //AddTest(Assembly.GetExecutingAssembly());
      // or in any reference assemblies
//      AddTest (typeof (MobileSDKIntegrationTest.AuthenticateTest).Assembly);

      // Once you called base.OnCreate(), you cannot add more assemblies.
      base.OnCreate(bundle);

      ServicePointManager
        .ServerCertificateValidationCallback +=
          (sender, cert, chain, sslPolicyErrors) => true;

    }
  }
}

