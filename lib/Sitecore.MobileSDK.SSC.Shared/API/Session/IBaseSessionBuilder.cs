namespace Sitecore.MobileSDK.API.Session
{
  using Sitecore.MobileSDK.API.MediaItem;


  /// <summary>
  /// Interface represents base session builder. 
  /// </summary>
  public interface IBaseSessionBuilder : IEntitySessionBuilder
  {
    /// <summary>
    /// Builds session.
    /// </summary>
    /// <returns>
    ///   <seealso cref="ISitecoreSSCSession" />
    /// </returns>
    ISitecoreSSCSession BuildSession();

    /// <summary>
    /// Builds session which contain read only operations.
    /// </summary>
    /// <returns>
    ///   <seealso cref="ISitecoreSSCReadonlySession" />
    /// </returns>
    ISitecoreSSCReadonlySession BuildReadonlySession();

    /// <summary>
    /// Specifies Item Web API version, 'v1' by default.
    /// This parameter value will be used if appropriate parameter will be missed in request object.
    /// 
    /// The value is case insensitive.
    /// </summary>
    /// <param name="sscVersion">Item Web API version</param>
    /// <returns>
    /// this
    /// </returns>
    IBaseSessionBuilder SSCVersion(string sscVersion);

    /// <summary>
    /// Specifies default database.
    /// This parameter value will be used if appropriate parameter will be missed in request object.
    /// For example: "web".
    /// 
    /// The value is case insensitive.
    /// </summary>
    /// <param name="defaultDatabase">Database</param>
    /// <returns>
    /// this
    /// </returns>
    IBaseSessionBuilder DefaultDatabase(string defaultDatabase);

    /// <summary>
    /// Specifies default language.
    /// This parameter value will be used if appropriate parameter will be missed in request object.
    /// For example: "en".
    /// 
    /// The value is case insensitive.
    /// </summary>
    /// <param name="defaultLanguage">Language</param>
    /// <returns>
    /// this
    /// </returns>
    IBaseSessionBuilder DefaultLanguage(string defaultLanguage);

    /// <summary>
    /// Specifies path to media lybrary root, "/sitecore/media library" by default.
    /// </summary>
    /// <param name="mediaLibraryRootItem">Media lybrary root</param>
    /// <returns>
    /// this
    /// </returns>
    IBaseSessionBuilder MediaLibraryRoot(string mediaLibraryRootItem);

    /// <summary>
    /// Specifies the media resource extension, "ashx" by default.
    /// </summary>
    /// <param name="defaultExtension">The default extension.</param>
    /// <returns>
    /// this
    /// </returns>
    IBaseSessionBuilder DefaultMediaResourceExtension(string defaultExtension);

    /// <summary>
    /// Prefix to build request for resource download, "~/media" by default.
    /// </summary>
    /// <param name="mediaPrefix">The media prefix.</param>
    /// <returns>
    /// this
    /// </returns>
    IBaseSessionBuilder MediaPrefix(string mediaPrefix);
  }
}

