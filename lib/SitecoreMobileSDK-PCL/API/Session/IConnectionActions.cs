namespace Sitecore.MobileSDK.API.Session
{
  using System.Threading;
  using System.Threading.Tasks;
  using Sitecore.MobileSDK.PublicKey;

  /// <summary>
  /// Interface represents connection actions like authenticate.
  /// </summary>
  public interface IConnectionActions
  {
    /// <summary>
    /// Performs asynchronous authentication.
    /// </summary>
    /// <param name="cancelToken">The cancel token, should be called in case when you want to terminate request execution.</param>
    /// <returns><see cref="Task{TResult}"/> returns YES if appropriate user exist on server and NO in other cases.</returns>
    Task<ScAuthResponse> AuthenticateAsync(CancellationToken cancelToken = default(CancellationToken));
  }
}

