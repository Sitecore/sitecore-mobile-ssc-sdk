namespace Sitecore.MobileSDK.Items
{
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Request.Parameters;

  public class ReadItemByPathParameters : IReadItemsByPathRequest
  {
    public ReadItemByPathParameters(
      ISessionConfig sessionSettings,
      IItemSource itemSource,
      IQueryParameters queryParameters,
      bool includeStandardFields,
      string itemPath)
    {
      this.SessionSettings = sessionSettings;
      this.ItemSource = itemSource;
      this.ItemPath = itemPath;
      this.QueryParameters = queryParameters;
      this.IncludeStandardTemplateFields = includeStandardFields;
    }

    public virtual IReadItemsByPathRequest DeepCopyGetItemByPathRequest()
    {
      ISessionConfig connection = null;
      IItemSource itemSrc = null;
      IQueryParameters payload = null;

      if (null != this.SessionSettings)
      {
        connection = this.SessionSettings.SessionConfigShallowCopy();
      }

      if (null != this.ItemSource)
      {
        itemSrc = this.ItemSource.ShallowCopy();
      }

      if (null != this.QueryParameters)
      {
        payload = this.QueryParameters.DeepCopy();
      }

      return new ReadItemByPathParameters(connection, itemSrc, payload, this.IncludeStandardTemplateFields, this.ItemPath);
    }

    public virtual IBaseItemRequest DeepCopyBaseGetItemRequest()
    {
      return this.DeepCopyGetItemByPathRequest();
    }

    public string ItemPath { get; private set; }

    public IItemSource ItemSource { get; private set; }

    public ISessionConfig SessionSettings { get; private set; }

    public IQueryParameters QueryParameters { get; private set; }

    public bool IncludeStandardTemplateFields { get; private set; }
  }
}