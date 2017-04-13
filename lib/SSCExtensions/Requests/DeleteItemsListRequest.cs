using System;
using System.Collections.Generic;
using Sitecore.MobileSDK.API;
using Sitecore.MobileSDK.API.Items;

namespace SSCExtensions
{
  public class DeleteItemsListRequest : IDeleteItemsListRequest
  {
    public DeleteItemsListRequest(ISessionConfig sessionConfig, string database, IEnumerable<ISitecoreItem> itemsList)
    {
      this.SessionConfig = sessionConfig;
      this.Database = database;
      this.ItemsList = itemsList;
    }

    public ISessionConfig SessionConfig { get; private set; }
    public string Database { get; private set; }
    public IEnumerable<ISitecoreItem> ItemsList { get; private set; }

    public IDeleteItemsListRequest DeepCopyDeleteItemListRequest()
    {
      ISessionConfig sessionConfig = null;
      string database = null;
      IEnumerable<ISitecoreItem> itemsList = null;

      if (null != this.SessionConfig) {
        sessionConfig = this.SessionConfig.SessionConfigShallowCopy();
      }

      if (null != this.Database) {
        database = this.Database;
      }

      if (null != this.ItemsList) {
        itemsList = this.ItemsList;
      }

      return new DeleteItemsListRequest(sessionConfig, database, itemsList);
    }
  }
}
