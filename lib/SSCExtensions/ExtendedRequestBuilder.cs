
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
