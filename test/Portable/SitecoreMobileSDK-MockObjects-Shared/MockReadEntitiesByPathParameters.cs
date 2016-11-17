
namespace Sitecore.MobileSDK.MockObjects
{
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.API.Request.Parameters;

  public class MockReadEntitiesByPathParameters : IReadEntitiesByPathRequest
  {
    public MockReadEntitiesByPathParameters()
    {
    }

    public virtual IReadEntitiesByPathRequest DeepCopyReadEntitiesByPathRequest()
    {
      return this;
    }

    public IEntitySource EntitySource { get; set; }
    public IDictionary<string, string> ParametersRawValuesByName { get; set; }

    public virtual IBaseItemRequest DeepCopyBaseGetItemRequest()
    {
      return this.DeepCopyReadEntitiesByPathRequest();
    }

    public string ItemPath { get; set; }
    public IItemSource ItemSource { get; set; }
    public ISessionConfig SessionSettings { get; set; }
    public IQueryParameters QueryParameters { get; set; }
    public bool IncludeStandardTemplateFields { get; set; }
  }
}