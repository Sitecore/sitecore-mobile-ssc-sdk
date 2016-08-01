namespace Sitecore.MobileSDK.PublicKey
{
  using System;
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API.Exceptions;
  using Sitecore.MobileSDK.Session;

  public class ScAuthResponse 
  {

    public ScAuthResponse(string number)
    {
      int result;
      if (Int32.TryParse(number, out result)) {
        this.StatusCode = result;
      } else {
        throw new ParserException(TaskFlowErrorMessages.PARSER_EXCEPTION_MESSAGE);
      }

    }

    public bool IsSuccessful {
        get{
          return this.StatusCode == 200;
        }
    }

    public int StatusCode {
      get;
      private set;
    }
  }
}
