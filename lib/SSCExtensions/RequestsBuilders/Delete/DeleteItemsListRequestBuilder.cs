using System.Collections.Generic;
using Sitecore.MobileSDK.API.Items;
using Sitecore.MobileSDK.UserRequest.DeleteRequest;
using Sitecore.MobileSDK.Validators;

namespace SSCExtensions
{
 public class DeleteItemsListRequestBuilder : AbstractDeleteItemRequestBuilder<IDeleteItemsListRequest>
  {
    private IEnumerable<ISitecoreItem> ItemsList;

    public DeleteItemsListRequestBuilder(IEnumerable<ISitecoreItem> itemsList)
    {
      foreach (ISitecoreItem item in itemsList) {
        ItemIdValidator.ValidateItemId(item.Id, this.GetType().Name + ".ItemId");
      }

      this.ItemsList = itemsList;
    }
    public override IDeleteItemsListRequest Build()
    {
      return new DeleteItemsListRequest(null, this.database, this.ItemsList);
    }
  }
}
