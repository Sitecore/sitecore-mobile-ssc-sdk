namespace Sitecore.MobileSDK.API.Request
{
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API.Request.Paging;


  /// <summary>
  /// Interface represents basic flow for creation of requets that have ability to specify scope for request.
  /// </summary>
  /// <typeparam name="T">Type of request</typeparam>
  public interface IScopedRequestParametersBuilder<T> where T : class
  {
    /// <summary>
    /// Specifies item database.
    /// For example: "web"
    /// 
    /// The value is case insensitive.
    /// </summary>
    /// <param name="sitecoreDatabase">The Sitecore database.</param>
    /// <returns>
    /// this
    /// </returns>
    IScopedRequestParametersBuilder<T> Database(string sitecoreDatabase);

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
    IScopedRequestParametersBuilder<T> Language(string itemLanguage);

    /// <summary>
    /// Adds the fields that will be read from the server.
    /// 
    /// The values is case insensitive.
    /// </summary>
    /// <param name="fields">The fields names.</param>
    /// <returns>
    /// this
    /// </returns>
    IScopedRequestParametersBuilder<T> AddFieldsToRead(IEnumerable<string> fields);

    /// <summary>
    /// Adds the fields that will be read from the server.
    /// 
    /// The values is case insensitive.
    /// </summary>
    /// <param name="fieldParams">The field names.</param>
    /// <returns>
    /// this
    /// </returns>
    /// <seealso cref="AddFieldsToRead(System.Collections.Generic.IEnumerable{string})" />
    IScopedRequestParametersBuilder<T> AddFieldsToRead(params string[] fieldParams);

    IScopedRequestParametersBuilder<T> IncludeStandardTemplateFields(bool include);

    /// <summary>
    /// Builds request with specified parameters.
    /// </summary>
    /// <returns>request</returns>
    T Build();
  }
}
