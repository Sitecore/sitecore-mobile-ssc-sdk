
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
      this.EntityID = id;
      this.FieldsRawValuesByName = fieldsRawValuesByName;
    }

    public CreateEntityParameters(string id, IDictionary<string, string> fieldsRawValuesByName, IEntitySource entitySource, ISessionConfig sessionSettings)
    {
      this.EntitySource = entitySource;
      this.EntityID = id;
      this.FieldsRawValuesByName = fieldsRawValuesByName;
      this.SessionSettings = sessionSettings;
    }

    public ICreateEntityRequest DeepCopyCreateEntityRequest() { 
      IEntitySource entitySource = null;

      if (null != this.EntitySource) {
        entitySource = this.EntitySource.ShallowCopy();
      }

      return new CreateEntityParameters(this.EntityID, this.FieldsRawValuesByName, entitySource);
    }

    public virtual IReadEntityByIdRequest DeepCopyReadEntitiesByIdRequest()
    {
      return this.DeepCopyCreateEntityRequest();
    }

    public string EntityID { get; protected set; }
    public IDictionary<string, string> FieldsRawValuesByName { get; protected set; }

    public IEntitySource EntitySource { get; protected set; }

    //FIXME: @igk exclude IBaseItemRequest from parents and properties below

    public virtual IBaseItemRequest DeepCopyBaseGetItemRequest()
    {
      return this.DeepCopyCreateEntityRequest();
    }

    public string ItemPath { get; protected set; }
    public IItemSource ItemSource { get; protected set; }
    public ISessionConfig SessionSettings { get; protected set; }
    public IQueryParameters QueryParameters { get; protected set; }
    public bool IncludeStandardTemplateFields { get; protected set; }
  }
}
