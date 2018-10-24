namespace Sitecore.MobileSDK.API.Items
{
  using System;
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API.Exceptions;
  using Sitecore.MobileSDK.Session;
  using Sitecore.MobileSDK.Validators;

  public class ScCreateItemResponse 
  {

    public ScCreateItemResponse(string itemId, int statusCode)
    {
        this.ItemId = itemId;
        this.StatusCode = statusCode;
    }

    public bool Created {
        get{
        return (this.ItemId != null) && (this.ItemId.Length>0);
        }
    }

    public string ItemId {
      get;
      private set;
    }

    public int StatusCode {
      get;
      private set;
    }
  }
}
