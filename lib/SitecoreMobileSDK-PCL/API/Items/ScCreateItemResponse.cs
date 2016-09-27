namespace Sitecore.MobileSDK.API.Items
{
  using System;
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API.Exceptions;
  using Sitecore.MobileSDK.Session;
  using Sitecore.MobileSDK.Validators;

  public class ScCreateItemResponse 
  {

    public ScCreateItemResponse(string itemId)
    {
      try {
        ItemIdValidator.ValidateItemId(itemId, this.GetType().Name + ".ItemId");
        this.ItemId = itemId;
      } catch {
        throw new ParserException(TaskFlowErrorMessages.PARSER_EXCEPTION_MESSAGE);
      }
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
  }
}
