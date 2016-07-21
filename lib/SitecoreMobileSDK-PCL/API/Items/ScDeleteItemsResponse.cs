namespace Sitecore.MobileSDK.API.Items
{
  using System;
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API.Exceptions;
  using Sitecore.MobileSDK.Session;

  /// <summary>
  /// Class represents server response for delete items request.
  /// It contains a list of ids for the successfully removed items.
  /// </summary>
  public class ScDeleteItemsResponse
  {
   public ScDeleteItemsResponse(string number)
    {
      int result;
      if (Int32.TryParse(number, out result)) {
        this.StatusCode = result;
      } else {
        throw new ParserException(TaskFlowErrorMessages.PARSER_EXCEPTION_MESSAGE);
      }

    }

    public bool Deleted {
      get {
        return this.StatusCode == 204;
      }
    }

    public int StatusCode {
      get;
      private set;
    }
  }
}
