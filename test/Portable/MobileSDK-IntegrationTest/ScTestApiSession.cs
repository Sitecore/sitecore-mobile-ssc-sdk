namespace MobileSDKIntegrationTest
{
  using System.Threading;
  using System.Threading.Tasks;
  using Sitecore.MobileSDK;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.MediaItem;
  using Sitecore.MobileSDK.Items;
  using Sitecore.MobileSDK.PasswordProvider.Interface;
  using Sitecore.MobileSDK.PublicKey;

  public class ScTestApiSession : ScApiSession
  {
    public ScTestApiSession(
      ISessionConfig config,
      IScCredentials credentials,
      IMediaLibrarySettings mediaSettings,
      ItemSource defaultSource = null) :
      base(config, credentials, mediaSettings, defaultSource)
    {
      this.GetPublicKeyInvocationsCount = 0;
    }

    public async Task GetPublicKeyAsyncPublic(CancellationToken cancelToken = default(CancellationToken))
    {
      await this.GetPublicKeyAsync(cancelToken);
    }



    protected override async Task GetPublicKeyAsync(CancellationToken cancelToken = default(CancellationToken))
    {
      ++this.GetPublicKeyInvocationsCount;
      await base.GetPublicKeyAsync(cancelToken);
    }

   

    public int GetPublicKeyInvocationsCount { get; private set; }
  }
}

