using System;
using System.Collections.Generic;
using Sitecore.MobileSDK.API.Items;

namespace SSCExtensions
{
  public class ScDeleteItemsListResponse
  {
    private List<ISitecoreItem> DeletedItemsPrivate;
    private List<ISitecoreItem> NotDeletedItemsPrivate;

    public ScDeleteItemsListResponse(List<ISitecoreItem> deletedItems, List<ISitecoreItem> notDeletedItems)
    {
      this.DeletedItemsPrivate = deletedItems;
      this.NotDeletedItemsPrivate = notDeletedItems;
    }

    public bool IsItemDeleted(ISitecoreItem item)
    {
      if (DeletedItemsPrivate.Contains(item)) {
        return true;
      }

      return false;
    }

    public int DeletedCount() {
      return DeletedItemsPrivate.Count;
    }

    public int NotDeletedCount()
    {
      return NotDeletedItemsPrivate.Count;
    }

    public int TotalCount()
    {
      return NotDeletedItemsPrivate.Count + DeletedItemsPrivate.Count;
    }

    public ISitecoreItem this[int notDeletedIndex] {
      get {
        return this.NotDeletedItemsPrivate[notDeletedIndex];
      }
    }
  }
}
