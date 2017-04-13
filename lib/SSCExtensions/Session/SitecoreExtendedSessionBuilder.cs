using System;
using Sitecore.MobileSDK.API.Session;
using Sitecore.MobileSDK.Validators;

namespace SSCExtensions
{
  class SitecoreExtendedSessionBuilder : ISitecoreExtendedSessionBuilder
  {
    public SitecoreExtendedSessionBuilder(ISitecoreSSCSession session)
    {
      this.session = session;
    }

    public ISitecoreExtendedSessionBuilder PathForTemporaryItems(string path)
    {
      this.FolderForTempItemsValue = path;
      return this;
    }

    public ISitecoreExtendedSessionBuilder DefaultTemporaryItemName(string name)
    {
      this.SearchItemNameValue = name;
      return this;
    }


    public ISitecoreExtendedSessionBuilder QueryItemTemplateItemId(string id)
    {
      ItemIdValidator.ValidateItemId(id, this.GetType().Name + ".QueryItemTemplateItemId");
      this.QueryItemTemplateItemIdValue = id;
      return this;
    }

    public ISitecoreExtendedSessionBuilder QueryFieldName(string fieldName)
    {
      this.QueryFieldNameValue = fieldName;
      return this;
    }

    public IExtendedSession Build()
    {
      IExtendedSessionConfigs configs =  new ExtendedSessionConfigs(this.FolderForTempItemsValue,
                                                                    this.QueryItemTemplateItemIdValue,
                                                                    this.SearchItemNameValue,
                                                                    this.QueryFieldNameValue);
      
      IExtendedSession extendedSession = new SSCExtendedSession(this.session, configs);

      return extendedSession;
    }

    private ISitecoreSSCSession session;

    private string FolderForTempItemsValue       = null;      
    private string QueryItemTemplateItemIdValue  = null;
    private string SearchItemNameValue           = null;
    private string QueryFieldNameValue           = null;
  }
}