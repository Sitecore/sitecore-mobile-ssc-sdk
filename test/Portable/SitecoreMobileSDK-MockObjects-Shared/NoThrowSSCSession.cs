namespace Sitecore.MobileSDK.MockObjects
{
  using System;
  using System.Diagnostics;
  using System.IO;
  using System.Threading;
  using System.Threading.Tasks;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.API.MediaItem;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.API.Session;
  using Sitecore.MobileSDK.PasswordProvider.Interface;
  using Sitecore.MobileSDK.PublicKey;

  public class NoThrowSSCSession : ISitecoreSSCSession
  {
    private ISitecoreSSCSession workerSession;

    public NoThrowSSCSession(ISitecoreSSCSession workerSession)
    {
      this.workerSession = workerSession;
    }

    public void Dispose()
    {
      if (null != this.workerSession)
      {
        this.workerSession.Dispose();
        this.workerSession = null;
      }
    }

    private async Task<TResult> InvokeNoThrow<TResult>(Task<TResult> task)
      where TResult : class
    {
      try
      {
        return await task;
      }
      catch (Exception ex)
      {
        Debug.WriteLine("Suppressed exception : " + Environment.NewLine + ex.ToString());
        return null;
      }
    }

    #region Getter Properties
    public IItemSource DefaultSource
    {
      get
      {
        return this.workerSession.DefaultSource;
      }
    }

    public ISessionConfig Config
    {
      get
      {
        return this.workerSession.Config;
      }
    }

    public IScCredentials Credentials
    {
      get
      {
        return this.workerSession.Credentials;
      }
    }

    public IMediaLibrarySettings MediaLibrarySettings 
    {
      get
      {
        return this.workerSession.MediaLibrarySettings;
      }
    }
    #endregion 
   
    #region GetItems

    public async Task<ScItemsResponse> ReadItemAsync(IReadItemsByIdRequest request, CancellationToken cancelToken = default(CancellationToken))
    {
      return await this.InvokeNoThrow(this.workerSession.ReadItemAsync(request, cancelToken));
    }

    public async Task<ScItemsResponse> ReadItemAsync(IReadItemsByPathRequest request, CancellationToken cancelToken = default(CancellationToken))
    {
      return await this.InvokeNoThrow(this.workerSession.ReadItemAsync(request, cancelToken));
    }

    public async Task<Stream> DownloadMediaResourceAsync(IMediaResourceDownloadRequest request, CancellationToken cancelToken = default(CancellationToken))
    {
      return await this.InvokeNoThrow(this.workerSession.DownloadMediaResourceAsync(request, cancelToken));
    }

    #endregion GetItems

    #region CreateItems

    public async Task<ScCreateItemResponse> CreateItemAsync(ICreateItemByPathRequest request, CancellationToken cancelToken = default(CancellationToken))
    {
      return await this.InvokeNoThrow(this.workerSession.CreateItemAsync(request, cancelToken));
    }

    public async Task<ScItemsResponse> ReadChildrenAsync(IReadItemsByIdRequest request, CancellationToken cancelToken = default(CancellationToken))
    { 
      return await this.InvokeNoThrow(this.workerSession.ReadChildrenAsync(request, cancelToken));
    }

    #endregion CreateItems

    #region Update Items

    public async Task<ScUpdateItemResponse> UpdateItemAsync(IUpdateItemByIdRequest request, CancellationToken cancelToken = default(CancellationToken))
    {
      return await this.InvokeNoThrow(this.workerSession.UpdateItemAsync(request, cancelToken));
    }

    #endregion Update Items

    #region Search Items

    public async Task<ScItemsResponse> RunSearchAsync(ISitecoreStoredSearchRequest request, CancellationToken cancelToken = default(CancellationToken))
    {
      return await this.InvokeNoThrow(this.workerSession.RunSearchAsync(request, cancelToken));
    }

    public async Task<ScItemsResponse> RunSearchAsync(ISitecoreSearchRequest request, CancellationToken cancelToken = default(CancellationToken))
    {
      return await this.InvokeNoThrow(this.workerSession.RunSearchAsync(request, cancelToken));
    }

    public async Task<ScItemsResponse> RunStoredQueryAsync(ISitecoreStoredSearchRequest request, CancellationToken cancelToken = default(CancellationToken))
    {
      return await this.InvokeNoThrow(this.workerSession.RunStoredQueryAsync(request, cancelToken));
    }

    #endregion Search Items

    #region DeleteItems

    public async Task<ScDeleteItemsResponse> DeleteItemAsync(IDeleteItemsByIdRequest request, CancellationToken cancelToken = default(CancellationToken))
    {
      return await this.InvokeNoThrow(this.workerSession.DeleteItemAsync(request, cancelToken));
    }

    #endregion DeleteItems

    #region Authentication

    public async Task<ScAuthResponse> AuthenticateAsync(CancellationToken cancelToken = default(CancellationToken))
    {
      try
      {
        return await this.workerSession.AuthenticateAsync(cancelToken);
      }
      catch (Exception ex)
      {
        Debug.WriteLine("Suppressed exception : " + Environment.NewLine + ex.ToString());
        return new ScAuthResponse(ex.ToString());
      }
    }

    #endregion Authentication

    #region Entity

    public async Task<ScEntityResponse> ReadEntityAsync(IReadEntitiesByPathRequest request, CancellationToken cancelToken = default(CancellationToken))
    {
      return await this.InvokeNoThrow(this.workerSession.ReadEntityAsync(request, cancelToken));
    }

    public async Task<ScEntityResponse> ReadEntityAsync(IReadEntityByIdRequest request, CancellationToken cancelToken = default(CancellationToken))
    {
      return await this.InvokeNoThrow(this.workerSession.ReadEntityAsync(request, cancelToken));
    }

    public async Task<ScCreateEntityResponse> CreateEntityAsync(ICreateEntityRequest request, CancellationToken cancelToken = default(CancellationToken))
    {
      return await this.InvokeNoThrow(this.workerSession.CreateEntityAsync(request, cancelToken));
    }

    public async Task<ScUpdateEntityResponse> UpdateEntityAsync(IUpdateEntityRequest request, CancellationToken cancelToken = default(CancellationToken))
    {
      return await this.InvokeNoThrow(this.workerSession.UpdateEntityAsync(request, cancelToken));
    }

    public async Task<ScDeleteEntityResponse> DeleteEntityAsync(IDeleteEntityRequest request, CancellationToken cancelToken = default(CancellationToken))
    {
      return await this.InvokeNoThrow(this.workerSession.DeleteEntityAsync(request, cancelToken));
    }

    #endregion Entity
  }
}

