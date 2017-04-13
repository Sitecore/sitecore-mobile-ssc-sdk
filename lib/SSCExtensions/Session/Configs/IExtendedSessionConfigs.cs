namespace SSCExtensions
{
  public interface IExtendedSessionConfigs
  {
    string FolderForTempItems {
	      get;
    }

    string QueryItemTemplateItemId {
    	get;
    }

    string SearchItemName {
	    get;
    }

    string QueryFieldName {
	    get;
    }
  }
}