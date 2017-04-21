namespace Sitecore.MobileSDK.API.Request
{
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API.Request.Paging;


  /// <summary>
  /// Interface represents basic flow for creation of requets that have ability to specify paging for request.
  /// </summary>
  /// <typeparam name="T">Type of request</typeparam>
  public interface IPagedRequestParametersBuilder<T> : IGetVersionedItemRequestParametersBuilder<T>
    where T : class
  {
    /// <summary>
    /// Specifies a number of the page to download.
    /// It should be a positive number or zero.
    /// It should be in a range of "TotalItemsInResponse" / "ItemsPerPage"
    /// 
    /// The parameter is optional. However, it requires "ItemsPerPage" parameters once specified.
    /// On a repeated invocation an InvalidOperationException is thrown.
    /// 
    /// </summary>
    /// <param name="pageNumber">Index of a page to download.
    /// An ArgumentException is thrown if negative number is submitted.
    /// </param>
    /// <returns>
    /// An object capable of setting ItemsPerPage parameter. Usually it is same as "this" object. 
    /// It is done to ensure that either both parameters are used ore none of them is specified.
    /// </returns>
    IPageNumberAccumulator<T> PageNumber(int pageNumber);



    new IPagedRequestParametersBuilder<T> Version(int? itemVersion);

    /// <summary>
    /// Specifies Sitecore database.
    /// For example: "web" 
    /// 
    /// The value is case insensitive.     
    /// </summary>
    /// <param name="sitecoreDatabase">The Sitecore database.</param>
    /// <returns>
    /// this
    /// </returns>
    new IPagedRequestParametersBuilder<T> Database(string sitecoreDatabase);

    /// <summary>
    /// Specifies item language.
    /// For example: "en"
    /// 
    /// The value is case insensitive.
    /// </summary>
    /// <param name="itemLanguage">The item language.</param>
    /// <returns>
    /// this
    /// </returns>
    new IPagedRequestParametersBuilder<T> Language(string itemLanguage);

    /// <summary>
    /// Adds the fields that will be read from the server.
    /// </summary>
    /// <param name="fields">The fields.</param>
    /// <returns>
    /// this
    /// </returns>
    new IPagedRequestParametersBuilder<T> AddFieldsToRead(IEnumerable<string> fields);

    /// <summary>
    /// Adds the fields that will be read from the server.
    /// </summary>
    /// <param name="fieldParams">The field parameters.</param>
    /// <returns>
    /// this
    /// </returns>
    /// <seealso cref="AddFieldsToRead(System.Collections.Generic.IEnumerable{string})" />
    new IPagedRequestParametersBuilder<T> AddFieldsToRead(params string[] fieldParams);

    new IPagedRequestParametersBuilder<T> IncludeStandardTemplateFields(bool include);

  }
}
