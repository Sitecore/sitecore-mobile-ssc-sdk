using System;
using Sitecore.MobileSDK.API;

namespace SSCExtensions
{
  public class DeleteItemsBySitecoreQueryRequest : IDeleteItemsBySitecoreQueryRequest
  {
    public DeleteItemsBySitecoreQueryRequest(ISessionConfig sessionConfig, string database, string sitecoreQuery)
    {
      this.SessionConfig = sessionConfig;
      this.Database = database;
      this.sitecoreQuery = sitecoreQuery;
    }

    public ISessionConfig SessionConfig { get; private set; }
    public string Database { get; private set; }
    public string sitecoreQuery { get; private set; }

    public IDeleteItemsBySitecoreQueryRequest DeepCopyDeleteItemsBySitecoreQuery()
    {
      ISessionConfig sessionConfig = null;
      string database = null;
      string query = null;

      if (null != this.SessionConfig) {
        sessionConfig = this.SessionConfig.SessionConfigShallowCopy();
      }

      if (null != this.Database) {
        database = this.Database;
      }

      if (null != this.sitecoreQuery) {
        query = this.sitecoreQuery;
      }

      return new DeleteItemsBySitecoreQueryRequest(sessionConfig, database, query);
    }
  }
}
