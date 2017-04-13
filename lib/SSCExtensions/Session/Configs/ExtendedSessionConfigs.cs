using System;

namespace SSCExtensions
{
  public class ExtendedSessionConfigs: IExtendedSessionConfigs
  {
    private static string DefaultFolderForTempItems       = "/sitecore/system";
    private static string DefaultQueryItemTemplateItemId  = "79E0E28F-5591-410E-A086-754EDC7CEF88";
    private static string DefaultSearchItemName           = "tempQueryItemName";
    private static string DefaultQueryFieldName           = "Query";

    private string FolderForTempItemsValue      = null;      
    private string QueryItemTemplateItemIdValue = null;
    private string SearchItemNameValue          = null;
    private string QueryFieldNameValue          = null;

    public ExtendedSessionConfigs(string folderForTempItems, 
                                  string queryItemTemplateItemId,
                                  string searchItemName,
                                  string queryFieldName)
    {
      this.FolderForTempItemsValue      = folderForTempItems;
      this.QueryItemTemplateItemIdValue = queryItemTemplateItemId;
      this.SearchItemNameValue          = searchItemName;
      this.QueryFieldNameValue          = queryFieldName;
    }


    string IExtendedSessionConfigs.FolderForTempItems {
      get {
        if (FolderForTempItemsValue != null){
          return FolderForTempItemsValue;
        }

        return DefaultFolderForTempItems;
      }
    }

    string IExtendedSessionConfigs.QueryItemTemplateItemId {
      get {
        if (QueryItemTemplateItemIdValue != null){
          return QueryItemTemplateItemIdValue;
        }

        return DefaultQueryItemTemplateItemId;
      }
    }

    string IExtendedSessionConfigs.SearchItemName {
      get {
        if (SearchItemNameValue != null){
          return SearchItemNameValue;
        }

        return DefaultSearchItemName;
      }
    }

    string IExtendedSessionConfigs.QueryFieldName {
      get {
        if (QueryFieldNameValue != null){
          return QueryFieldNameValue;
        }

        return DefaultQueryFieldName;
      }
    }
  }
}
