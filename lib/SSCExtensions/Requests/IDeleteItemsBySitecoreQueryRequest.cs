using System;
using Sitecore.MobileSDK.API.Request;

namespace SSCExtensions
{

  public interface IDeleteItemsBySitecoreQueryRequest : IBaseDeleteItemRequest
  {
    IDeleteItemsBySitecoreQueryRequest DeepCopyDeleteItemsBySitecoreQuery();

    string sitecoreQuery { get; }
  }
}
