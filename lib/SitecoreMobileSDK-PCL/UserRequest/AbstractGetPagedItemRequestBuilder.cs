namespace Sitecore.MobileSDK.UserRequest
{
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Request.Parameters;
  using Sitecore.MobileSDK.Items;
  using Sitecore.MobileSDK.Validators;

  public abstract class AbstractGetPagedItemRequestBuilder<T> : 
          AbstractGetVersionedItemRequestBuilder<T>,
          IPagedRequestParametersBuilder<T>
  where T : class
  {

    new public IPagedRequestParametersBuilder<T> PageNumber(int pageNumber)
    {
      return (IPagedRequestParametersBuilder<T>)base.PageNumber(pageNumber);
    }

    new public IPagedRequestParametersBuilder<T> ItemsPerPage(int itemsCountPerPage)
    {
      return (IPagedRequestParametersBuilder<T>)base.ItemsPerPage(itemsCountPerPage);
    }


    #region Compatibility Casts

    new public IPagedRequestParametersBuilder<T> Version(int? itemVersion)
    {
      return (IPagedRequestParametersBuilder<T>)base.Version(itemVersion);
    }

    new public IPagedRequestParametersBuilder<T> Database(string sitecoreDatabase)
    {
      return (IPagedRequestParametersBuilder<T>)base.Database(sitecoreDatabase);
    }

    new public IPagedRequestParametersBuilder<T> Language(string itemLanguage)
    {
      return (IPagedRequestParametersBuilder<T>)base.Language(itemLanguage);
    }

    new public IPagedRequestParametersBuilder<T> AddFieldsToRead(IEnumerable<string> fields)
    {
      return (IPagedRequestParametersBuilder<T>)base.AddFieldsToRead(fields);
    }

    new public IPagedRequestParametersBuilder<T> AddFieldsToRead(params string[] fieldParams)
    {
      return (IPagedRequestParametersBuilder<T>)base.AddFieldsToRead(fieldParams);
    }

    new public IPagedRequestParametersBuilder<T> IncludeStandardTemplateFields(bool include)
    {
      return (IPagedRequestParametersBuilder<T>)base.IncludeStandardTemplateFields(include);
    }
    #endregion Compatibility Casts

  }
}

