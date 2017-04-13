using System.Collections.Generic;
using Sitecore.MobileSDK.API.Items;
using Sitecore.MobileSDK.UserRequest.DeleteRequest;
using Sitecore.MobileSDK.Validators;

namespace SSCExtensions
{
 public class DeleteItemsBySitecorQueryRequestBuilder : AbstractDeleteItemRequestBuilder<IDeleteItemsBySitecoreQueryRequest>
  {
    private string query;

    public DeleteItemsBySitecorQueryRequestBuilder(string sitecoreQuery)
    {
      if (string.IsNullOrEmpty(sitecoreQuery)) {
        throw new System.NullReferenceException(this.GetType().Name + " : Sitecore query must not be null or empty");
      }

      this.query = sitecoreQuery;
    }

    public override IDeleteItemsBySitecoreQueryRequest Build()
    {
      return new DeleteItemsBySitecoreQueryRequest(null, this.database, this.query);
    }
  }
}
