
using Sitecore.MobileSDK.API.Request;
using Sitecore.MobileSDK.UserRequest.ReadRequest;

namespace SSCExtensions
{
  public class ExtendedRequestBuilder
  {
    private ExtendedRequestBuilder()
    {
    }

    public static IGetVersionedItemRequestParametersBuilder<IReadItemsByIdRequest> ReadItemsRequestWithId(string itemId)
    {
      return new ReadItemByIdRequestBuilder(itemId);
    }

  }
}
