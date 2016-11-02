
namespace Sitecore.MobileSDK.Entities
{
  using System;
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.API.Request.Parameters;

  public class CreateEntityParameters : ICreateEntityRequest
  {
    public CreateEntityParameters(string id, IDictionary<string, string> fieldsRawValuesByName, IEntitySource entitySource)
    {
      this.EntitySource = entitySource;
      this.EntityId = id;
      this.FieldsRawValuesByName = fieldsRawValuesByName;
    }

    public CreateEntityParameters(string id, IDictionary<string, string> fieldsRawValuesByName, IEntitySource entitySource, ISessionConfig sessionSettings)
    {
      this.EntitySource = entitySource;
      this.EntityId = id;
      this.FieldsRawValuesByName = fieldsRawValuesByName;
      this.SessionSettings = sessionSettings;
    }

    public ICreateEntityRequest DeepCopyCreateEntityRequest() { 
      IEntitySource entitySource = null;

      if (null != this.EntitySource) {
        entitySource = this.EntitySource.ShallowCopy();
      }

      return new CreateEntityParameters(this.EntityId, this.FieldsRawValuesByName, entitySource);
    }

    public string EntityId { get; private set; }
    public IDictionary<string, string> FieldsRawValuesByName { get; private set; }

    public IEntitySource EntitySource { get; private set; }

    //FIXME: @igk exclude IBaseItemRequest from parents and properties below

    public virtual IBaseItemRequest DeepCopyBaseGetItemRequest()
    {
      return this.DeepCopyCreateEntityRequest();
    }

    public string ItemPath { get; private set; }
    public IItemSource ItemSource { get; private set; }
    public ISessionConfig SessionSettings { get; private set; }
    public IQueryParameters QueryParameters { get; private set; }
    public bool IncludeStandardTemplateFields { get; private set; }
  }
}
