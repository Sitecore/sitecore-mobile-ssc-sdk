namespace Sitecore.MobileSDK.API.Items
{
  using System;
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API.Exceptions;
  using Sitecore.MobileSDK.Session;

  public class ScUpdateItemResponse 
  {

    public ScUpdateItemResponse(string number)
    {
      int result;
      if (Int32.TryParse(number, out result)) {
        this.StatusCode = result;
      } else {
        throw new ParserException(TaskFlowErrorMessages.PARSER_EXCEPTION_MESSAGE);
      }

    }

    public bool Updated {
        get{
          return this.StatusCode == 204;
        }
    }

    public int StatusCode {
      get;
      private set;
    }
  }
}
