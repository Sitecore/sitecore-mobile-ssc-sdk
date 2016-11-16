namespace Sitecore.MobileSDK.API.Request
{
  /// <summary>
  /// Interface represents basic create item request parameters.
  /// <seealso cref="ICreateItemByIdRequest"/>
  /// <seealso cref="ICreateItemByPathRequest"/>
  /// </summary>
  public interface IBaseCreateItemRequest : IBaseChangeItemRequest
  {
    /// <summary>
    /// Gets the name of the item. Represents name of the item in the content tree.
    /// 
    /// The value is case sensitive.
    /// </summary>
    /// <returns>
    /// The name of the item.
    /// </returns>
    string ItemName { get; }

    /// <summary>
    /// Item's template id. 
    /// 
    /// The value is case insensitive.
    /// </summary>
    /// <returns>
    /// The item template id.
    /// </returns>
    string ItemTemplateId { get; }
  }
}

