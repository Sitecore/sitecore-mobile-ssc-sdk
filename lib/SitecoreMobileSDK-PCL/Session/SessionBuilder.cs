namespace Sitecore.MobileSDK.Session
{
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.MediaItem;
  using Sitecore.MobileSDK.API.Session;
  using Sitecore.MobileSDK.Items;
  using Sitecore.MobileSDK.PasswordProvider;
  using Sitecore.MobileSDK.PasswordProvider.Interface;
  using Sitecore.MobileSDK.SessionSettings;
  using Sitecore.MobileSDK.Validators;
    using System.Net.Http;

    internal class SessionBuilder : IAuthenticatedSessionBuilder, IAnonymousSessionBuilder, IEntitySessionBuilder
  {
    #region Main Logic
    public ISitecoreSSCSession BuildSession(HttpClientHandler handler,
      HttpClient httpClient)
    {
      string optionalMediaRoot = this.OptionalMediaRoot();
      string optionalMediaExtension = this.OptionalMediaExtension();
      string optionalMediaPrefix = this.OptionalMediaPrefix();


      ////////
      SessionConfig conf = new SessionConfig(this.instanceUrl);

      var mediaSettings = new MediaLibrarySettings(
        optionalMediaRoot,
        optionalMediaExtension,
        optionalMediaPrefix);

      var itemSource = new ItemSource(
        this.itemSourceAccumulator.Database,
        this.itemSourceAccumulator.Language,
        this.itemSourceAccumulator.VersionNumber);

      var entitySource = new EntitySource(
        this.entitySourceAccumulator.EntityNamespace,
        this.entitySourceAccumulator.EntityController,
        this.entitySourceAccumulator.EntityId,
        this.entitySourceAccumulator.EntityAction);


      var result = new ScApiSession(conf, entitySource, this.credentials, mediaSettings, handler, httpClient, itemSource);
      return result;
    }

    public ISitecoreSSCReadonlySession BuildReadonlySession(HttpClientHandler handler,
      HttpClient httpClient)
    {
      return this.BuildSession(handler, httpClient);
    }

    private string OptionalMediaRoot()
    {
      string optionalMediaRoot = this.mediaRoot;
      if (null == optionalMediaRoot)
      {
        optionalMediaRoot = "/sitecore/media library";
      }

      return optionalMediaRoot;
    }

    private string OptionalMediaExtension()
    {
      string optionalMediaExtension = this.mediaExtension;
      if (null == optionalMediaExtension)
      {
        optionalMediaExtension = "ashx";
      }

      return optionalMediaExtension;
    }

    private string OptionalMediaPrefix()
    {
      string optionalMediaPrefix = this.mediaPrefix;
      if (null == optionalMediaPrefix)
      {
        optionalMediaPrefix = "~/media";
      }

      return optionalMediaPrefix;
    }
    #endregion Main Logic

    #region Constructor
    private SessionBuilder()
    {
    }

    public static SessionBuilder SessionBuilderWithHost(string instanceUrl)
    {
      BaseValidator.CheckForNullAndEmptyOrThrow(instanceUrl, typeof(SessionBuilder).Name + ".InstanceUrl");

      var result = new SessionBuilder
      {
        instanceUrl = instanceUrl
      };

      return result;
    }
    #endregion Constructor

    #region IAuthenticatedSessionBuilder
    public IBaseSessionBuilder Credentials(IScCredentials credentials)
    {
      // @adk : won't be invoked more than once.
      // No validation needed.
      BaseValidator.CheckForNullAndEmptyOrThrow(credentials.Username, this.GetType().Name + ".Credentials.Username");
      BaseValidator.CheckForNullAndEmptyOrThrow(credentials.Password, this.GetType().Name + ".Credentials.Password");

      this.credentials = credentials.CredentialsShallowCopy();
      return this;
    }
    #endregion

    #region IAnonymousSessionBuilder
    public IBaseSessionBuilder Site(string site)
    {
      if (string.IsNullOrEmpty(site))
      {
        return this;
      }

      BaseValidator.CheckForTwiceSetAndThrow(this.site, this.GetType().Name + ".Site");
      BaseValidator.CheckForNullEmptyAndWhiteSpaceOrThrow(site, this.GetType().Name + ".Site");

      this.site = site;
      return this;
    }

    public IBaseSessionBuilder SSCVersion(string sscVersion)
    {
      BaseValidator.CheckForTwiceSetAndThrow(this.sscVersion, this.GetType().Name + ".SSCVersion");
      BaseValidator.CheckForNullEmptyAndWhiteSpaceOrThrow(sscVersion, this.GetType().Name + ".SSCVersion");

      this.sscVersion = sscVersion;
      return this;
    }

    public IBaseSessionBuilder DefaultDatabase(string defaultDatabase)
    {
      if (string.IsNullOrEmpty(defaultDatabase))
      {
        return this;
      }

      BaseValidator.CheckForTwiceSetAndThrow(this.itemSourceAccumulator.Database,
        this.GetType().Name + ".DefaultDatabase");
      BaseValidator.CheckForNullEmptyAndWhiteSpaceOrThrow(defaultDatabase,
        this.GetType().Name + ".DefaultDatabase");

      this.itemSourceAccumulator =
        new ItemSourcePOD(
          defaultDatabase,
          this.itemSourceAccumulator.Language,
          itemSourceAccumulator.VersionNumber);

      return this;
    }

    public IBaseSessionBuilder DefaultLanguage(string defaultLanguage)
    {
      if (string.IsNullOrEmpty(defaultLanguage))
      {
        return this;
      }

      BaseValidator.CheckForTwiceSetAndThrow(this.itemSourceAccumulator.Language,
        this.GetType().Name + ".DefaultLanguage");
      BaseValidator.CheckForNullEmptyAndWhiteSpaceOrThrow(defaultLanguage,
        this.GetType().Name + ".DefaultLanguage");

      this.itemSourceAccumulator =
        new ItemSourcePOD(
          this.itemSourceAccumulator.Database,
          defaultLanguage,
          itemSourceAccumulator.VersionNumber);

      return this;
    }

    public IBaseSessionBuilder MediaLibraryRoot(string mediaLibraryRootItem)
    {
      if (string.IsNullOrEmpty(mediaLibraryRootItem))
      {
        return this;
      }

      BaseValidator.CheckForTwiceSetAndThrow(this.mediaRoot,
        this.GetType().Name + ".MediaLibraryRoot");
      BaseValidator.CheckForNullEmptyAndWhiteSpaceOrThrow(mediaLibraryRootItem,
        this.GetType().Name + ".MediaLibraryRoot");

      this.mediaRoot = mediaLibraryRootItem;
      return this;
    }

    public IBaseSessionBuilder DefaultMediaResourceExtension(string defaultExtension)
    {
      if (string.IsNullOrEmpty(defaultExtension))
      {
        return this;
      }

      BaseValidator.CheckForTwiceSetAndThrow(this.mediaExtension,
        this.GetType().Name + ".DefaultMediaResourceExtension");
      BaseValidator.CheckForNullEmptyAndWhiteSpaceOrThrow(defaultExtension,
        this.GetType().Name + ".DefaultMediaResourceExtension");

      this.mediaExtension = defaultExtension;
      return this;
    }

    public IBaseSessionBuilder MediaPrefix(string mediaPrefix)
    {
      if (string.IsNullOrEmpty(mediaPrefix))
      {
        return this;
      }

      BaseValidator.CheckForTwiceSetAndThrow(this.mediaPrefix,
        this.GetType().Name + ".MediaPrefix");
      BaseValidator.CheckForNullEmptyAndWhiteSpaceOrThrow(mediaPrefix,
        this.GetType().Name + ".MediaPrefix");

      this.mediaPrefix = mediaPrefix;
      return this;
    }

    #endregion IAnonymousSessionBuilder

    #region Entity
    public IBaseSessionBuilder EntityRouteNamespace(string entityNamespace)
    {
      if (string.IsNullOrEmpty(entityNamespace)) {
        return this;
      }

      BaseValidator.CheckForTwiceSetAndThrow(this.entitySourceAccumulator.EntityNamespace,
        this.GetType().Name + ".EntityRouteNamespace");
      BaseValidator.CheckForNullEmptyAndWhiteSpaceOrThrow(entityNamespace,
        this.GetType().Name + ".EntityRouteNamespace");

      this.entitySourceAccumulator =
            new EntitySource( 
                             entityNamespace,
                             this.entitySourceAccumulator.EntityController,
                             this.entitySourceAccumulator.EntityId,
                             this.entitySourceAccumulator.EntityAction);

      return this;
    }

    public IBaseSessionBuilder EntityRouteController(string entityController)
    {
      if (string.IsNullOrEmpty(entityController)) {
        return this;
      }

      BaseValidator.CheckForTwiceSetAndThrow(this.entitySourceAccumulator.EntityController,
        this.GetType().Name + ".EntityRouteController");
      BaseValidator.CheckForNullEmptyAndWhiteSpaceOrThrow(entityController,
        this.GetType().Name + ".EntityRouteController");

      this.entitySourceAccumulator =
            new EntitySource(
              this.entitySourceAccumulator.EntityNamespace,
              entityController,
              this.entitySourceAccumulator.EntityId,
              this.entitySourceAccumulator.EntityAction);

      return this;
    }

    public IBaseSessionBuilder EntityRouteId(string entityId)
    {
      if (string.IsNullOrEmpty(entityId)) {
        return this;
      }

      BaseValidator.CheckForTwiceSetAndThrow(this.entitySourceAccumulator.EntityId,
        this.GetType().Name + ".EntityRouteId");
      BaseValidator.CheckForNullEmptyAndWhiteSpaceOrThrow(entityId,
        this.GetType().Name + ".EntityRouteId");

      this.entitySourceAccumulator =
            new EntitySource(
              this.entitySourceAccumulator.EntityNamespace,
              this.entitySourceAccumulator.EntityController,
              entityId,
              this.entitySourceAccumulator.EntityAction);

      return this;
    }

    public IBaseSessionBuilder EntityRouteAction(string entityAction)
    {
      if (string.IsNullOrEmpty(entityAction)) {
        return this;
      }

      BaseValidator.CheckForTwiceSetAndThrow(this.entitySourceAccumulator.EntityAction,
        this.GetType().Name + ".EntityRouteAction");
      BaseValidator.CheckForNullEmptyAndWhiteSpaceOrThrow(entityAction,
        this.GetType().Name + ".EntityRouteAction");

      this.entitySourceAccumulator =
            new EntitySource(
              this.entitySourceAccumulator.EntityNamespace,
              this.entitySourceAccumulator.EntityController,
              this.entitySourceAccumulator.EntityId,
              entityAction);

      return this;
    }
    #endregion Entity

    #region State
    private string instanceUrl;
    private string sscVersion;
    private string site;
    private string mediaRoot;
    private string mediaExtension;
    private string mediaPrefix;

    private IScCredentials credentials = null;
    private ItemSourcePOD itemSourceAccumulator = new ItemSourcePOD(null, null, null);
    private IEntitySource entitySourceAccumulator = new EntitySource(null, null, null, null);
    #endregion State
  }
}

